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
    /// Enumeration, documentation, and architecture-suite self-checks.
    /// </summary>
    [TestFixture]
    public sealed class TypeSurfaceConventionTests
    {
        private static readonly string[] RequiredSuitePackages = ["Microsoft.CodeAnalysis.CSharp", "NUnit", "TngTech.ArchUnitNET"];

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingEnums_ThenEveryVariantDeclaresAnExplicitValue(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<EnumMemberDeclarationSyntax> implicitValues = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<EnumMemberDeclarationSyntax>()
                    .Where(member => member.EqualsValue is null);

                violations.AddRange(implicitValues.Select(member => SourceParsing.Describe(
                    tree,
                    member,
                    $"enum variant '{member.Identifier.ValueText}' must declare an explicit numeric value.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingEnums_ThenNumericValuesAreUnique(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (EnumDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>())
                {
                    string[] duplicated = declaration.Members
                        .Where(member => member.EqualsValue is not null)
                        .GroupBy(member => member.EqualsValue!.Value.ToString(), StringComparer.Ordinal)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key)
                        .ToArray();

                    if (duplicated.Length > 0)
                    {
                        violations.Add(SourceParsing.Describe(
                            tree,
                            declaration,
                            $"enum '{declaration.Identifier.ValueText}' declares duplicate numeric values ({string.Join(", ", duplicated)})."));
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingContracts_ThenEveryInterfaceMemberIsDocumented(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] lines = SourceParsing.ReadSource(sourcePrefix, tree.FilePath)
                    .Replace("\r\n", "\n", StringComparison.Ordinal)
                    .Split('\n');

                foreach (InterfaceDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>())
                {
                    foreach (MemberDeclarationSyntax member in declaration.Members)
                    {
                        int lineIndex = SourceParsing.GetLineNumber(member) - 1;

                        if (!HasDocumentation(lines, lineIndex))
                        {
                            violations.Add(SourceParsing.Describe(
                                tree,
                                member,
                                $"interface members of '{declaration.Identifier.ValueText}' must carry an XML summary because the package is published to NuGet."));
                        }
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingStaticFields_ThenNoneArePubliclyMutable(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<FieldDeclarationSyntax> mutableStatics = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<FieldDeclarationSyntax>()
                    .Where(field => field.Modifiers.Any(SyntaxKind.PublicKeyword))
                    .Where(field => field.Modifiers.Any(SyntaxKind.StaticKeyword))
                    .Where(field => !field.Modifiers.Any(SyntaxKind.ReadOnlyKeyword))
                    .Where(field => !field.Modifiers.Any(SyntaxKind.ConstKeyword));

                violations.AddRange(mutableStatics.Select(field => SourceParsing.Describe(
                    tree,
                    field,
                    "public mutable static fields are prohibited; expose read-only state.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingDocumentedRoles_ThenNoDedicatedLayerNamespaceIsUsed(string sourcePrefix)
        {
            string[] prohibitedSuffixes = [".Interfaces", ".Enumerations", ".Enums", ".Utilities", ".Helpers"];

            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (NamespaceDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>())
                {
                    string namespaceName = declaration.Name.ToString();
                    string? prohibitedSuffix = prohibitedSuffixes.FirstOrDefault(suffix =>
                        namespaceName.EndsWith(suffix, StringComparison.Ordinal));

                    if (prohibitedSuffix is null)
                    {
                        continue;
                    }

                    violations.Add(SourceParsing.Describe(
                        tree,
                        declaration,
                        $"'{namespaceName}' must not use the generic '{prohibitedSuffix}' layer suffix; colocate the types with their responsibility."));
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public void GivenArchitectureTestProject_WhenInspectingPackages_ThenTheFrameworkSetIsComplete()
        {
            string projectFile = ArchitectureResources.ReadResource(
                ArchitectureResources.ProjectFilePrefix,
                "NuciXNA.Gui.ArchitectureTests/NuciXNA.Gui.ArchitectureTests.csproj");

            string[] missing = RequiredSuitePackages
                .Where(package => !projectFile.Contains($"\"{package}\"", StringComparison.Ordinal))
                .ToArray();

            Assert.That(
                missing,
                Is.Empty,
                $"Violated convention: the architecture suite requires NUnit, ArchUnitNET, and Roslyn ({string.Join(", ", missing)} missing).");
        }

        [Test]
        public void GivenArchitectureTestSources_WhenCheckingFriendAssemblies_ThenNoneIsDeclared()
        {
            string[] violations = SourceParsing.CollectParseTrees(ArchitectureResources.ArchitectureTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<AttributeSyntax>())
                .Where(attribute => string.Equals(attribute.Name.ToString(), "InternalsVisibleTo", StringComparison.Ordinal))
                .Select(attribute => attribute.ToString())
                .ToArray();

            Assert.That(violations, Is.Empty, $"Violated boundary: architecture tests must not declare InternalsVisibleTo ({string.Join(", ", violations)}).");
        }

        [Test]
        public void GivenArchitectureTestFixtures_WhenInspected_ThenEachDeclaresTheTestFixtureAttribute()
        {
            string[] fixtures = SourceParsing.CollectParseTrees(ArchitectureResources.ArchitectureTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                .Where(declaration => declaration.Members.OfType<MethodDeclarationSyntax>().Any(HasTestAttribute))
                .Select(declaration => declaration.Identifier.ValueText)
                .ToArray();

            Assert.That(fixtures, Is.Not.Empty, "Vacuous-selector guard: no architecture fixtures were found.");

            string[] missing = SourceParsing.CollectParseTrees(ArchitectureResources.ArchitectureTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                .Where(declaration => declaration.Members.OfType<MethodDeclarationSyntax>().Any(HasTestAttribute))
                .Where(declaration => !declaration.AttributeLists.SelectMany(list => list.Attributes)
                    .Any(attribute => string.Equals(attribute.Name.ToString(), "TestFixture", StringComparison.Ordinal)))
                .Select(declaration => declaration.Identifier.ValueText)
                .ToArray();

            Assert.That(missing, Is.Empty, $"{string.Join(", ", missing)} must declare [TestFixture].");
        }

        private static bool HasTestAttribute(MethodDeclarationSyntax method)
            => method.AttributeLists.SelectMany(list => list.Attributes)
                .Any(attribute => string.Equals(attribute.Name.ToString(), "Test", StringComparison.Ordinal)
                    || string.Equals(attribute.Name.ToString(), "TestCase", StringComparison.Ordinal));

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
