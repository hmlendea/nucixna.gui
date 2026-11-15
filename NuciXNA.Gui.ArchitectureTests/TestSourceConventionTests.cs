using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciXNA.Gui.ArchitectureTests
{
    /// <summary>
    /// NuGet-convention-verified rules over the unit-test sources.
    /// </summary>
    [TestFixture]
    public sealed class TestSourceConventionTests
    {
        private const int MinParameterisedCases = 2;

        [Test]
        public void GivenUnitTestFixtures_WhenInspected_ThenEachDeclaresTheTestFixtureAttribute()
        {
            string[] missing = SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                .Where(declaration => declaration.Identifier.ValueText is not "DummyControl" and not "DummyScreen")
                .Where(declaration => !HasTestFixtureAttribute(declaration))
                .Select(declaration => declaration.Identifier.ValueText)
                .ToArray();

            Assert.That(missing, Is.Empty, $"{string.Join(", ", missing)} must declare [TestFixture].");
        }

        [Test]
        public void GivenUnitTestMethods_WhenNamed_ThenEachUsesGivenWhenThenForm()
        {
            string[] nonConforming = SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>())
                .Where(IsUnitTestMethod)
                .Select(method => method.Identifier.ValueText)
                .Where(name => !name.StartsWith("Given", StringComparison.Ordinal)
                    || !name.Contains("_When", StringComparison.Ordinal)
                    || !name.Contains("_Then", StringComparison.Ordinal))
                .ToArray();

            Assert.That(
                nonConforming,
                Is.Empty,
                $"Violated convention: every unit test must use Given..._When..._Then... naming ({string.Join(", ", nonConforming)}).");
        }

        [Test]
        public void GivenUnitTestMethods_WhenInspected_ThenNoneReflectsNonPublicMembers()
        {
            string[] violations = SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
                .Where(call => call.Expression.ToString().Contains("GetMethod", StringComparison.Ordinal)
                    && call.ArgumentList?.Arguments.Any(argument => argument.ToString().Contains("NonPublic", StringComparison.Ordinal)) == true)
                .Select(call => call.ToString().Substring(0, Math.Min(call.ToString().Length, 60)))
                .ToArray();

            Assert.That(
                violations,
                Is.Empty,
                $"Violated convention: unit tests must not reflect over non-public members ({string.Join(", ", violations)}).");
        }

        [Test]
        public void GivenUnitTestMethods_WhenParameterised_ThenEachDeclaresAtLeastTwoCases()
        {
            string[] underCase = SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>())
                .Where(method => HasTestCaseAttribute(method))
                .Where(method =>
                    method.AttributeLists.SelectMany(list => list.Attributes)
                        .Count(attribute => string.Equals(attribute.Name.ToString(), "TestCase", StringComparison.Ordinal))
                        < MinParameterisedCases)
                .Select(method => method.Identifier.ValueText)
                .ToArray();

            Assert.That(
                underCase,
                Is.Empty,
                $"Violated convention: every parameterised unit test must declare at least {MinParameterisedCases} cases ({string.Join(", ", underCase)}).");
        }

        private static bool HasTestFixtureAttribute(ClassDeclarationSyntax declaration)
            => declaration.AttributeLists.SelectMany(list => list.Attributes)
                .Any(attribute => string.Equals(attribute.Name.ToString(), "TestFixture", StringComparison.Ordinal));

        private static bool IsUnitTestMethod(MethodDeclarationSyntax method)
            => method.AttributeLists.SelectMany(list => list.Attributes).Any(attribute =>
                string.Equals(attribute.Name.ToString(), "Test", StringComparison.Ordinal)
                || string.Equals(attribute.Name.ToString(), "TestCase", StringComparison.Ordinal));

        private static bool HasTestCaseAttribute(MethodDeclarationSyntax method)
            => method.AttributeLists.SelectMany(list => list.Attributes)
                .Any(attribute => string.Equals(attribute.Name.ToString(), "TestCase", StringComparison.Ordinal));
    }
}
