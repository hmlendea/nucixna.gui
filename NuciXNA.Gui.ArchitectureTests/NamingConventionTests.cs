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
    /// Naming and declaration checks over the handwritten sources.
    /// </summary>
    [TestFixture]
    public sealed class NamingConventionTests
    {
        private static readonly string[] BuiltInTypeNames =
        [
            "String", "Int32", "Boolean", "Object", "Byte", "Char",
            "Int64", "Double", "Single", "Decimal", "UInt32", "UInt64",
            "Int16", "UInt16"
        ];

        private static readonly string[] ForbiddenBooleanNames = ["flag", "check", "result"];

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingTypeDeclarations_ThenPascalCaseIsUsed(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] misnamed = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(name => !IsPascalCase(name))
                    .ToArray();

                Assert.That(
                    misnamed,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: every type must use PascalCase.");
            }
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingInterfaceNames_ThenTheIPrefixAndPascalCaseAreUsed(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] misnamed = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<InterfaceDeclarationSyntax>()
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(name => name.Length < 2 || name[0] != 'I' || !char.IsUpper(name[1]))
                    .ToArray();

                Assert.That(
                    misnamed,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: every interface must use the 'I' prefix followed by PascalCase.");
            }
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingMethodNames_ThenPascalCaseIsUsed(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] misnamed = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Select(method => method.Identifier.ValueText)
                    .Where(name => !IsPascalCase(name))
                    .ToArray();

                Assert.That(
                    misnamed,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: every method must use PascalCase.");
            }
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingPrivateFields_ThenCamelCaseWithoutUnderscoreIsUsed(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                VariableDeclaratorSyntax[] prefixed = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<FieldDeclarationSyntax>()
                    .Where(field => field.Modifiers.Any(SyntaxKind.PrivateKeyword))
                    .SelectMany(field => field.Declaration.Variables)
                    .Where(variable => variable.Identifier.ValueText.StartsWith('_'))
                    .ToArray();

                Assert.That(
                    prefixed,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: private fields must use camelCase without an underscore prefix.");
            }
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingPublicFieldsAndProperties_ThenTheyStartUppercase(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] misnamed = tree.GetRoot()
                    .DescendantNodes()
                    .Where(IsPublicOrProtectedFieldOrProperty)
                    .SelectMany(GetDeclaredIdentifiers)
                    .Where(name => !IsPascalCase(name))
                    .ToArray();

                Assert.That(
                    misnamed,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: public and protected members must start uppercase.");
            }
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingBooleanProperties_ThenPermissibleFormsAreUsed(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] forbidden = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .Where(property => property.Type is PredefinedTypeSyntax predefined
                        && predefined.Keyword.IsKind(SyntaxKind.BoolKeyword))
                    .Select(property => property.Identifier.ValueText)
                    .Where(name => ForbiddenBooleanNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                Assert.That(
                    forbidden,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: boolean names must not use flag, check, or result forms.");
            }
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingBuiltInTypes_ThenAliasesAreUsed(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] bclNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Select(identifier => identifier.Identifier.ValueText)
                    .Where(name => BuiltInTypeNames.Contains(name, StringComparer.Ordinal))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();

                Assert.That(
                    bclNames,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: built-in type aliases must be used instead of {string.Join(", ", bclNames)}.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenCheckingEventArgsTypes_ThenTheEventArgsSuffixIsUsed()
        {
            string[] misnamed = SourceParsing.CollectParseTrees(ArchitectureResources.ProductionSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                .Where(declaration => declaration.BaseList?.Types.Any(baseType =>
                    string.Equals(baseType.Type.ToString(), "EventArgs", StringComparison.Ordinal)) == true)
                .Select(declaration => declaration.Identifier.ValueText)
                .Where(name => !name.EndsWith("EventArgs", StringComparison.Ordinal))
                .ToArray();

            Assert.That(
                misnamed,
                Is.Empty,
                $"Violated convention: {string.Join(", ", misnamed)} must use the EventArgs suffix.");
        }

        [Test]
        public void GivenUnitTestFixtures_WhenCheckingClassNames_ThenEachEndsWithTheTestsSuffix()
        {
            string[] fixtureNames = GetUnitTestFixtureNames().ToArray();

            Assert.That(fixtureNames, Is.Not.Empty, "Vacuous-selector guard: no unit test classes were found.");
            Assert.That(
                fixtureNames.Where(name => !name.EndsWith("Tests", StringComparison.Ordinal)),
                Is.Empty,
                "Violated convention: every unit test class name must end with the 'Tests' suffix.");
        }

        [Test]
        public void GivenUnitTestFixtures_WhenCheckingEachName_ThenAnExportedProductionSubjectExists()
        {
            string[] productionTypeNames = ProductionArchitecture.ProductionAssembly
                .GetExportedTypes()
                .Select(type => type.Name)
                .ToArray();

            string[] documentedSubjectAliases = ["Control"];
            List<string> violations = [];

            foreach (string fixtureName in GetUnitTestFixtureNames())
            {
                string subjectName = fixtureName.Substring(0, fixtureName.Length - "Tests".Length);
                bool matchesProductionSubject = productionTypeNames.Any(typeName =>
                    subjectName.StartsWith(typeName, StringComparison.Ordinal));
                bool matchesDocumentedAlias = documentedSubjectAliases.Any(aliasName =>
                    string.Equals(subjectName, aliasName, StringComparison.Ordinal)
                    || subjectName.StartsWith(aliasName, StringComparison.Ordinal));

                if (matchesProductionSubject || matchesDocumentedAlias)
                {
                    continue;
                }

                violations.Add(fixtureName);
            }

            Assert.That(
                violations,
                Is.Empty,
                $"Violated convention: every unit test fixture must target an exported production type ({string.Join(", ", violations)}).");
        }

        private static IEnumerable<string> GetUnitTestFixtureNames()
            => SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                .Where(declaration => declaration.AttributeLists
                    .SelectMany(list => list.Attributes)
                    .Any(attribute => string.Equals(attribute.Name.ToString(), "TestFixture", StringComparison.Ordinal)))
                .Select(declaration => declaration.Identifier.ValueText);

        private static bool IsPascalCase(string name)
            => !string.IsNullOrEmpty(name) && char.IsUpper(name[0]);

        private static bool IsPublicOrProtectedFieldOrProperty(SyntaxNode node)
        {
            if (node is FieldDeclarationSyntax field)
            {
                return field.Modifiers.Any(SyntaxKind.PublicKeyword)
                    || field.Modifiers.Any(SyntaxKind.ProtectedKeyword);
            }

            if (node is PropertyDeclarationSyntax property)
            {
                return property.Modifiers.Any(SyntaxKind.PublicKeyword)
                    || property.Modifiers.Any(SyntaxKind.ProtectedKeyword);
            }

            return false;
        }

        private static IEnumerable<string> GetDeclaredIdentifiers(SyntaxNode node)
            => node switch
            {
                FieldDeclarationSyntax field => field.Declaration.Variables.Select(variable => variable.Identifier.ValueText),
                PropertyDeclarationSyntax property => [property.Identifier.ValueText],
                _ => []
            };
    }
}
