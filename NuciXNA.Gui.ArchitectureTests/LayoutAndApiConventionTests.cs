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
    /// Namespace, layout, and public-API shape checks over the handwritten sources.
    /// </summary>
    [TestFixture]
    public sealed class LayoutAndApiConventionTests
    {
        private static readonly string[] DocumentedRootNamespaces = ["NuciXNA.Gui", "NuciXNA.Gui.Controls", "NuciXNA.Gui.Screens"];

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingNamespaces_ThenEachMatchesItsFolder(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                NamespaceDeclarationSyntax? declaration = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<NamespaceDeclarationSyntax>()
                    .FirstOrDefault();

                if (declaration is null)
                {
                    continue;
                }

                string folder = SourceParsing.GetFolderName(tree.FilePath);
                string expectedName = GetExpectedNamespace(sourcePrefix, folder);
                string actualName = declaration.Name.ToString();

                if (!string.Equals(actualName, expectedName, StringComparison.Ordinal))
                {
                    violations.Add($"Violated convention in {tree.FilePath}: namespace '{actualName}' must match its folder as '{expectedName}'.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNamespaces_ThenOnlyDocumentedRootsAreUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (NamespaceDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>())
                {
                    string namespaceName = declaration.Name.ToString();
                    bool isDocumented = DocumentedRootNamespaces.Any(root =>
                        string.Equals(namespaceName, root, StringComparison.Ordinal)
                        || namespaceName.StartsWith(root + ".", StringComparison.Ordinal)
                        || namespaceName.StartsWith("NuciXNA.Gui.UnitTests", StringComparison.Ordinal)
                        || namespaceName.StartsWith("NuciXNA.Gui.ArchitectureTests", StringComparison.Ordinal));

                    if (!isDocumented)
                    {
                        violations.Add($"Violated convention in {tree.FilePath}: '{namespaceName}' falls outside the documented namespace roots.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNestedTypes_ThenNoneIsDeclared(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<BaseTypeDeclarationSyntax> nested = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Where(declaration => declaration.Parent is BaseTypeDeclarationSyntax);

                violations.AddRange(nested.Select(declaration => SourceParsing.Describe(
                    tree,
                    declaration,
                    "one type per file: extract nested types into their own identically named files.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingEmptyBodies_ThenTheyAreWrittenInline(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<BlockSyntax> multilineEmptyBlocks = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BlockSyntax>()
                    .Where(block => block.Statements.Count == 0)
                    .Where(block => block.ToString().Contains('\n', StringComparison.Ordinal));

                violations.AddRange(multilineEmptyBlocks.Select(block => SourceParsing.Describe(
                    tree,
                    block,
                    "empty bodies must be written as '{ }' on a single line.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingGlobalUsingDirectives_ThenNoneIsUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<UsingDirectiveSyntax> globalUsings = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<UsingDirectiveSyntax>()
                    .Where(directive => directive.GlobalKeyword.IsKind(SyntaxKind.GlobalKeyword));

                if (globalUsings.Any())
                {
                    violations.Add($"Violated convention in {tree.FilePath}: global using directives are prohibited; import per file.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNullablePatterns_ThenEqualityComparisonsAreNotUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<BinaryExpressionSyntax> nullComparisons = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BinaryExpressionSyntax>()
                    .Where(expression => expression.IsKind(SyntaxKind.EqualsExpression)
                        || expression.IsKind(SyntaxKind.NotEqualsExpression))
                    .Where(expression => expression.Left.IsKind(SyntaxKind.NullLiteralExpression)
                        || expression.Right.IsKind(SyntaxKind.NullLiteralExpression));

                violations.AddRange(nullComparisons.Select(expression => SourceParsing.Describe(
                    tree,
                    expression,
                    "use 'is null' or 'is not null' instead of equality comparisons.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingExceptionHandling_ThenEmptyCatchAndRethrowPatternsAreAbsent(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (CatchClauseSyntax clause in tree.GetRoot().DescendantNodes().OfType<CatchClauseSyntax>())
                {
                    if (clause.Block.Statements.Count == 0)
                    {
                        violations.Add(SourceParsing.Describe(tree, clause, "empty catch blocks are prohibited."));
                    }

                    if (clause.Declaration is null)
                    {
                        continue;
                    }

                    foreach (ThrowStatementSyntax statement in clause.Block.DescendantNodes().OfType<ThrowStatementSyntax>())
                    {
                        if (statement.Expression is IdentifierNameSyntax identifier
                            && string.Equals(identifier.Identifier.ValueText, clause.Declaration.Identifier.ValueText, StringComparison.Ordinal))
                        {
                            violations.Add(SourceParsing.Describe(tree, statement, "use a bare 'throw;' to preserve the stack trace."));
                        }
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingWarningsSuppression_ThenNoPragmaDisableIsUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                SyntaxTrivia[] pragmas = tree.GetRoot()
                    .DescendantTrivia()
                    .Where(trivia => trivia.IsKind(SyntaxKind.PragmaWarningDirectiveTrivia))
                    .ToArray();

                if (pragmas.Length > 0)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: '#pragma warning disable' is prohibited.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingLayout_ThenNoBlankLinePrecedesTypeClosingBrace(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (TypeDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    if (declaration.CloseBraceToken.LeadingTrivia.Any(trivia => trivia.IsKind(SyntaxKind.EndOfLineTrivia))
                        && declaration.CloseBraceToken.LeadingTrivia.ToString().Trim().Length == 0
                        && declaration.CloseBraceToken.LeadingTrivia.Count > 1)
                    {
                        violations.Add($"Violated convention in {tree.FilePath}: '{declaration.Identifier.ValueText}' must not end with a blank line before its closing brace.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingOverloads_ThenTheyAreAdjacentAndOrderedByParameterCount(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (TypeDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    List<MethodDeclarationSyntax> methods = declaration.Members
                        .OfType<MethodDeclarationSyntax>()
                        .ToList();

                    IEnumerable<IGrouping<string, MethodDeclarationSyntax>> overloadGroups = methods
                        .GroupBy(method => method.Identifier.ValueText, StringComparer.Ordinal)
                        .Where(group => group.Count() > 1);

                    foreach (IGrouping<string, MethodDeclarationSyntax> group in overloadGroups)
                    {
                        int[] indices = group.Select(method => methods.IndexOf(method)).OrderBy(index => index).ToArray();

                        for (int index = 1; index < indices.Length; index += 1)
                        {
                            if (indices[index] - indices[index - 1] != 1)
                            {
                                violations.Add($"Violated convention in {tree.FilePath}: the '{group.Key}' overloads must be declared consecutively.");
                            }
                        }

                        int[] counts = group.Select(method => method.ParameterList.Parameters.Count).ToArray();

                        for (int index = 1; index < counts.Length; index += 1)
                        {
                            if (counts[index] < counts[index - 1])
                            {
                                violations.Add($"Violated convention in {tree.FilePath}: the '{group.Key}' overloads must be ordered from fewest to most parameters.");
                            }
                        }
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingCollections_ThenListTypesAreNotExposedInParametersOrReturns(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<MethodDeclarationSyntax> offendingMethods = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(method => method.Modifiers.Any(SyntaxKind.PublicKeyword))
                    .Where(method => method.ReturnType.ToString().StartsWith("List<", StringComparison.Ordinal)
                        || method.ParameterList.Parameters.Any(parameter =>
                            parameter.Type?.ToString().StartsWith("List<", StringComparison.Ordinal) == true));

                violations.AddRange(offendingMethods.Select(method => SourceParsing.Describe(
                    tree,
                    method,
                    $"'{method.Identifier.ValueText}' must expose IEnumerable<T> instead of List<T> in its parameters and return type.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNullCoalescing_ThenTheNullCoalescingOperatorIsAbsent(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<BinaryExpressionSyntax> coalesceExpressions = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BinaryExpressionSyntax>()
                    .Where(expression => expression.IsKind(SyntaxKind.CoalesceExpression));

                violations.AddRange(coalesceExpressions.Select(expression => SourceParsing.Describe(
                    tree,
                    expression,
                    "use explicit control flow instead of the '??' operator; '??=' remains permitted in lazy initialisation.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        private static string GetExpectedNamespace(string sourcePrefix, string folder)
        {
            string projectName = SourceParsing.GetProjectName(sourcePrefix, folder);

            if (string.IsNullOrEmpty(folder))
            {
                return projectName;
            }

            return projectName + "." + folder;
        }
    }
}
