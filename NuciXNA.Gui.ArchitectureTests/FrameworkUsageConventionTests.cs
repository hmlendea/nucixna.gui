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
    /// Framework-usage, dummy-ownership, and assertion-form checks that complement the structural rules.
    /// </summary>
    [TestFixture]
    public sealed class FrameworkUsageConventionTests
    {
        private static readonly string[] ProhibitedMonoGameApis = ["LoadTexture2D", "DrawString", "SpriteFont", "Texture2D"];

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingMonoGameApis_ThenOnlyNuciXNAWrappersAreUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (IdentifierNameSyntax identifier in tree.GetRoot().DescendantNodes().OfType<IdentifierNameSyntax>())
                {
                    string name = identifier.Identifier.ValueText;

                    if (!ProhibitedMonoGameApis.Contains(name, StringComparer.Ordinal))
                    {
                        continue;
                    }

                    // Texture2D is permitted only as a draw-method parameter type.
                    bool isPermittedParameter = identifier.Parent is ParameterSyntax;

                    if (isPermittedParameter)
                    {
                        continue;
                    }

                    violations.Add(SourceParsing.Describe(
                        tree,
                        identifier,
                        $"raw MonoGame API '{name}' must be replaced by the NuciXNA GuiImage/GuiText wrappers."));
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingAsyncUsage_ThenNoAsyncOrCancellationIsIntroduced(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<SyntaxToken> asyncModifiers = tree.GetRoot()
                    .DescendantTokens()
                    .Where(token => token.IsKind(SyntaxKind.AsyncKeyword));

                IEnumerable<IdentifierNameSyntax> cancellationTokens = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Where(identifier => string.Equals(identifier.Identifier.ValueText, "CancellationToken", StringComparison.Ordinal));

                violations.AddRange(asyncModifiers.Select(token => $"Violated convention in {tree.FilePath}: asynchronous members are prohibited; the library is synchronous."));
                violations.AddRange(cancellationTokens.Select(identifier => SourceParsing.Describe(
                    tree,
                    identifier,
                    "CancellationToken is prohibited unless explicitly required.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingZeroValueGeometry_ThenTheEmptyConstantsAreUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<ObjectCreationExpressionSyntax> zeroConstructions = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ObjectCreationExpressionSyntax>()
                    .Where(creation => creation.Type.ToString() is "Point2D" or "Size2D" or "Scale2D")
                    .Where(creation => creation.ArgumentList?.Arguments.Count is 0 or 1 or 2)
                    .Where(creation => creation.ArgumentList?.Arguments.All(argument =>
                        string.Equals(argument.ToString(), "0", StringComparison.Ordinal)) == true);

                violations.AddRange(zeroConstructions.Select(creation => SourceParsing.Describe(
                    tree,
                    creation,
                    "use Point2D.Empty or Size2D.Zero instead of constructing zero-valued geometry.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenSources_WhenCheckingConstants_ThenStaticPropertiesAreUsedInProduction(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<FieldDeclarationSyntax> constantFields = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<FieldDeclarationSyntax>()
                    .Where(field => field.Modifiers.Any(SyntaxKind.ConstKeyword));

                violations.AddRange(constantFields.Select(field => SourceParsing.Describe(
                    tree,
                    field,
                    "express constants as 'static' members instead of 'const'.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public void GivenUnitTestDummies_WhenInspected_ThenEachIsInternalAndTopLevel()
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix))
            {
                foreach (ClassDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    string name = declaration.Identifier.ValueText;

                    if (!name.StartsWith("Dummy", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (!declaration.Modifiers.Any(SyntaxKind.InternalKeyword))
                    {
                        violations.Add(SourceParsing.Describe(tree, declaration, $"'{name}' must be an internal dummy implementation."));
                    }

                    if (declaration.Parent is TypeDeclarationSyntax)
                    {
                        violations.Add(SourceParsing.Describe(tree, declaration, $"'{name}' must be a top-level type in its own file."));
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public void GivenUnitTestMethods_WhenAssertingBooleans_ThenRedundantTrueConstraintsAreAbsent()
        {
            string[] violations = SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
                .Where(call => string.Equals(call.Expression.ToString(), "Assert.That", StringComparison.Ordinal))
                .Where(call => call.ArgumentList.Arguments.Count >= 2)
                .Where(call => string.Equals(call.ArgumentList.Arguments[1].ToString(), "Is.True", StringComparison.Ordinal))
                .Select(call => call.ToString().Substring(0, Math.Min(call.ToString().Length, 70)))
                .ToArray();

            Assert.That(
                violations,
                Is.Empty,
                $"Violated convention: use Assert.That(actual) instead of Assert.That(actual, Is.True) ({string.Join(", ", violations)}).");
        }

        [Test]
        public void GivenUnitTestMethods_WhenSettingUp_ThenTheSetUpMethodIsNamedSetUp()
        {
            string[] misnamed = SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>())
                .Where(method => method.AttributeLists.SelectMany(list => list.Attributes)
                    .Any(attribute => string.Equals(attribute.Name.ToString(), "SetUp", StringComparison.Ordinal)))
                .Where(method => !string.Equals(method.Identifier.ValueText, "SetUp", StringComparison.Ordinal))
                .Select(method => method.Identifier.ValueText)
                .ToArray();

            Assert.That(misnamed, Is.Empty, $"Violated convention: the [SetUp] method must be named SetUp ({string.Join(", ", misnamed)}).");
        }

        [Test]
        public void GivenAssemblyAttributes_WhenInspected_ThenNoInternalsVisibleToIsDeclared()
        {
            string[] violations = SourceParsing.CollectParseTrees(ArchitectureResources.ProductionSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<AttributeSyntax>())
                .Where(attribute => string.Equals(attribute.Name.ToString(), "InternalsVisibleTo", StringComparison.Ordinal))
                .Select(attribute => attribute.ToString())
                .ToArray();

            Assert.That(violations, Is.Empty, $"Violated boundary: production code must not declare InternalsVisibleTo ({string.Join(", ", violations)}).");
        }

        [Test]
        public void GivenUnitTestSources_WhenCheckingFailures_ThenNoIgnoredTestsAreDeclared()
        {
            string[] violations = SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix)
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<AttributeSyntax>())
                .Where(attribute => string.Equals(attribute.Name.ToString(), "Ignore", StringComparison.Ordinal))
                .Select(attribute => attribute.ToString())
                .ToArray();

            Assert.That(violations, Is.Empty, $"Violated convention: tests must not be ignored ({string.Join(", ", violations)}).");
        }

        [Test]
        public void GivenUnitTestMethods_WhenAsserting_ThenSingleStatementTestsAreExpressionBodied()
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(ArchitectureResources.UnitTestSourcePrefix))
            {
                IEnumerable<MethodDeclarationSyntax> singleAssertionBlocks = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(method => method.ExpressionBody is null)
                    .Where(method => method.ParameterList.Parameters.Count == 0)
                    .Where(method => method.Body is not null && method.Body.Statements.Count == 1)
                    .Where(method => method.Body!.Statements[0] is ExpressionStatementSyntax statement
                        && statement.Expression.ToString().StartsWith("Assert.", StringComparison.Ordinal));

                violations.AddRange(singleAssertionBlocks.Select(method => SourceParsing.Describe(
                    tree,
                    method,
                    $"'{method.Identifier.ValueText}' contains a single assertion and must be expression-bodied.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenScreenSources_WhenCheckingLifecycleOwnership_ThenControlLifecycleIsNotDrivenManually(string sourcePrefix)
        {
            string[] screenTypes = ["Screen", "MenuScreen"];
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<InvocationExpressionSyntax> manualCalls = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Where(call => call.Expression is MemberAccessExpressionSyntax accessExpression
                        && accessExpression.Name.Identifier.ValueText is "LoadContent" or "UnloadContent")
                    .Where(call => call.Ancestors().OfType<ClassDeclarationSyntax>().Any(declaration =>
                        screenTypes.Contains(declaration.Identifier.ValueText, StringComparer.Ordinal)))
                    .Where(call => !IsManagerReceiver(((MemberAccessExpressionSyntax)call.Expression).Expression));

                violations.AddRange(manualCalls.Select(call => SourceParsing.Describe(
                    tree,
                    call,
                    "screens must delegate control lifecycle to GuiManager instead of invoking control LoadContent/UnloadContent directly.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        public void GivenUnitTestSources_WhenCheckingRegistration_ThenControlsAreNotRegisteredWithTheManager(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<InvocationExpressionSyntax> registrations = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Where(call => call.Expression is MemberAccessExpressionSyntax access
                        && access.Name.Identifier.ValueText is "RegisterControl" or "RegisterControls");

                violations.AddRange(registrations.Select(call => SourceParsing.Describe(
                    tree,
                    call,
                    "unit tests must exercise controls directly through protected child registration instead of the global GuiManager registry.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        private static bool IsManagerReceiver(ExpressionSyntax receiver)
            => receiver is MemberAccessExpressionSyntax access
                && string.Equals(access.Name.Identifier.ValueText, "Instance", StringComparison.Ordinal)
                && string.Equals(access.Expression.ToString(), "GuiManager", StringComparison.Ordinal);
    }
}
