using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciXNA.Gui.ArchitectureTests
{
    /// <summary>
    /// Type, interface, and documentation declaration checks over the handwritten sources.
    /// </summary>
    [TestFixture]
    public sealed class TypeDeclarationConventionTests
    {
        private static readonly string[] DocumentedContracts = ["IGuiControl", "IScreen"];

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingInterfaces_ThenEachUsesTheIPrefix(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<InterfaceDeclarationSyntax> misnamed = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<InterfaceDeclarationSyntax>()
                    .Where(declaration => IsMisnamedInterface(declaration.Identifier.ValueText));

                violations.AddRange(misnamed.Select(declaration => SourceParsing.Describe(
                    tree,
                    declaration,
                    "each interface must use the 'I' prefix followed by PascalCase.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public void GivenProductionSources_WhenCheckingContracts_ThenEachDocumentedContractIsImplemented()
        {
            SyntaxTree[] trees = SourceParsing.CollectParseTrees(ArchitectureResources.ProductionSourcePrefix).ToArray();
            List<string> violations = [];

            foreach (string contract in DocumentedContracts)
            {
                bool hasImplementer = trees
                    .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                    .Any(declaration => declaration.BaseList?.Types.Any(baseType =>
                        string.Equals(baseType.Type.ToString(), contract, StringComparison.Ordinal)) == true);

                if (!hasImplementer)
                {
                    violations.Add($"'{contract}' has no production implementer.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public void GivenProductionSources_WhenCheckingContracts_ThenEachIsColocatedWithItsImplementations()
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(ArchitectureResources.ProductionSourcePrefix))
            {
                foreach (InterfaceDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>())
                {
                    string folder = SourceParsing.GetFolderName(tree.FilePath);

                    if (!string.Equals(folder, "Screens", StringComparison.Ordinal)
                        && !string.Equals(folder, "Controls", StringComparison.Ordinal))
                    {
                        violations.Add(SourceParsing.Describe(
                            tree,
                            declaration,
                            $"'{declaration.Identifier.ValueText}' must be colocated in the Controls or Screens folder."));
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNamespaces_ThenNoInterfacesNamespaceIsUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (NamespaceDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>())
                {
                    string namespaceName = declaration.Name.ToString();

                    if (namespaceName.EndsWith(".Interfaces", StringComparison.Ordinal))
                    {
                        violations.Add(SourceParsing.Describe(
                            tree,
                            declaration,
                            $"'{namespaceName}' must be replaced by colocated interfaces without a dedicated Interfaces namespace."));
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public void GivenProductionSources_WhenCheckingPublicSurface_ThenTypesAndMembersAreDocumented()
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(ArchitectureResources.ProductionSourcePrefix))
            {
                string[] lines = SourceParsing.ReadSource(ArchitectureResources.ProductionSourcePrefix, tree.FilePath)
                    .Replace("\r\n", "\n", StringComparison.Ordinal)
                    .Split('\n');

                foreach (BaseTypeDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                {
                    if (!IsPubliclyVisible(declaration))
                    {
                        continue;
                    }

                    if (!HasDocumentation(lines, SourceParsing.GetLineNumber(declaration) - 1))
                    {
                        violations.Add(SourceParsing.Describe(
                            tree,
                            declaration,
                            $"'{declaration.Identifier.ValueText}' must carry an XML summary because it is part of the public package surface."));
                    }
                }

                foreach (MemberDeclarationSyntax member in tree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>())
                {
                    if (!IsAccessibilityScopedMember(member) || !IsPubliclyVisible(member))
                    {
                        continue;
                    }

                    if (member is MethodDeclarationSyntax method
                        && (method.ExplicitInterfaceSpecifier is not null || IsOverride(method)))
                    {
                        continue;
                    }

                    if (!HasDocumentation(lines, SourceParsing.GetLineNumber(member) - 1))
                    {
                        violations.Add(SourceParsing.Describe(
                            tree,
                            member,
                            "public and protected members of the NuGet package must carry an XML summary."));
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        private static bool IsMisnamedInterface(string name)
            => name.Length < 2 || name[0] != 'I' || !char.IsUpper(name[1]);

        private static bool IsAccessibilityScopedMember(MemberDeclarationSyntax member)
            => member is FieldDeclarationSyntax
                || member is PropertyDeclarationSyntax
                || member is EventFieldDeclarationSyntax
                || member is ConstructorDeclarationSyntax
                || member is MethodDeclarationSyntax;

        private static bool IsPubliclyVisible(MemberDeclarationSyntax member)
            => member.Modifiers.Any(SyntaxKind.PublicKeyword)
                || member.Modifiers.Any(SyntaxKind.ProtectedKeyword);

        private static bool IsOverride(MethodDeclarationSyntax method)
            => method.Modifiers.Any(SyntaxKind.OverrideKeyword);

        private static bool HasDocumentation(string[] lines, int declarationLineIndex)
        {
            for (int index = declarationLineIndex - 1; index >= 0 && index >= declarationLineIndex - 6; index -= 1)
            {
                string line = lines[index].Trim();

                if (line.StartsWith("///", StringComparison.Ordinal))
                {
                    return true;
                }

                if (line.Length != 0 && !line.StartsWith("[", StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return false;
        }
    }
}
