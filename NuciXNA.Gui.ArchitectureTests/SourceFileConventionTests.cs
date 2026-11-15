using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciXNA.Gui.ArchitectureTests
{
    /// <summary>
    /// Roslyn-based structural source-file convention checks over the handwritten sources.
    /// </summary>
    [TestFixture]
    public sealed class SourceFileConventionTests
    {
        private static int MaxParameterCount => 7;

        private static int MaxSignatureLength => 96;

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingDeclaredTypes_ThenEachFileDeclaresExactlyOneType(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] declaredTypes = GetDeclaredTypeNames(tree).ToArray();

                if (declaredTypes.Length != 1)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: expected exactly one declared type, but found {declaredTypes.Length}.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingTypeNames_ThenEachMatchesItsFileNameExactly(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string fileNameStem = SourceParsing.GetFileNameStem(tree.FilePath);

                foreach (string typeName in GetDeclaredTypeNames(tree))
                {
                    if (!string.Equals(typeName, fileNameStem, StringComparison.Ordinal))
                    {
                        violations.Add($"Violated convention in {tree.FilePath}: declared type '{typeName}' does not case-sensitively match the file name stem '{fileNameStem}'.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNamespaces_ThenExactlyOneBlockNamespaceIsUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

                if (root.Members.OfType<NamespaceDeclarationSyntax>().Count() != 1)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: exactly one block namespace is required.");
                }

                if (root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().Any())
                {
                    violations.Add($"Violated convention in {tree.FilePath}: file-scoped namespaces are prohibited.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingProhibitedDeclarations_ThenNoneIsUsed(string sourcePrefix)
        {
            List<string> violations = [];
            bool isProductionSource = string.Equals(
                sourcePrefix,
                ArchitectureResources.ProductionSourcePrefix,
                StringComparison.Ordinal);

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<SyntaxNode> offenders = tree.GetRoot()
                    .DescendantNodes()
                    .Where(node => IsProhibitedDeclaration(node, isProductionSource));

                violations.AddRange(offenders.Select(node => SourceParsing.Describe(
                    tree,
                    node,
                    "var, ternaries, tuples, init accessors, partial types, and const fields are prohibited.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNullableDirectives_ThenNoneIsUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                SyntaxTrivia[] directives = tree.GetRoot()
                    .DescendantTrivia()
                    .Where(trivia => trivia.IsKind(SyntaxKind.NullableDirectiveTrivia))
                    .ToArray();

                if (directives.Length > 0)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: the '#nullable' directive is prohibited.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingControlFlowBodies_ThenEveryBodyUsesBraces(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<StatementSyntax> unbraced = tree.GetRoot()
                    .DescendantNodes()
                    .Select(GetUnbracedBody)
                    .OfType<StatementSyntax>();

                violations.AddRange(unbraced.Select(body => SourceParsing.Describe(
                    tree,
                    body,
                    "every control-flow body must use braces.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingParameterLists_ThenAtMostSevenParametersAreUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<ParameterListSyntax> oversized = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ParameterListSyntax>()
                    .Where(parameters => parameters.Parameters.Count > MaxParameterCount);

                violations.AddRange(oversized.Select(parameters => SourceParsing.Describe(
                    tree,
                    parameters,
                    "methods, constructors, and lambdas accept at most seven parameters.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingParameterModifiers_ThenRefOutAndDefaultsAreAbsent(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<ParameterSyntax> offenders = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ParameterSyntax>()
                    .Where(parameter => parameter.Modifiers.Any(SyntaxKind.RefKeyword)
                        || parameter.Modifiers.Any(SyntaxKind.OutKeyword)
                        || parameter.Default is not null);

                violations.AddRange(offenders.Select(parameter => SourceParsing.Describe(
                    tree,
                    parameter,
                    "use overloads; ref, out, and optional parameters are prohibited.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingLineLength_ThenLongSignaturesAreWrapped(string sourcePrefix)
        {
            List<string> violations = [];
            bool isProductionSource = string.Equals(
                sourcePrefix,
                ArchitectureResources.ProductionSourcePrefix,
                StringComparison.Ordinal);

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<ParameterListSyntax> oversizedParameters = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ParameterListSyntax>()
                    .Where(parameters => parameters.Parameters.Count > 1)
                    .Where(parameters => parameters.ToString().Length > MaxSignatureLength)
                    .Where(parameters => !parameters.ToString().Contains('\n', StringComparison.Ordinal));

                violations.AddRange(oversizedParameters.Select(parameters => SourceParsing.Describe(
                    tree,
                    parameters,
                    $"a parameter list exceeding {MaxSignatureLength} characters must place every item on its own line.")));

                if (!isProductionSource)
                {
                    continue;
                }

                IEnumerable<ArgumentListSyntax> oversizedArguments = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ArgumentListSyntax>()
                    .Where(arguments => arguments.Arguments.Count > 1)
                    .Where(arguments => arguments.ToString().Length > MaxSignatureLength)
                    .Where(arguments => !arguments.ToString().Contains('\n', StringComparison.Ordinal));

                violations.AddRange(oversizedArguments.Select(arguments => SourceParsing.Describe(
                    tree,
                    arguments,
                    $"an argument list exceeding {MaxSignatureLength} characters must place every item on its own line.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingUsingDirectives_ThenGroupsAreOrderedAndSeparated(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                violations.AddRange(FindUsingViolations(tree).Select(reason =>
                    $"Violated convention in {tree.FilePath}: {reason}"));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        private static IEnumerable<string> GetDeclaredTypeNames(SyntaxTree tree)
            => tree.GetRoot()
                .DescendantNodes()
                .OfType<BaseTypeDeclarationSyntax>()
                .Select(declaration => declaration.Identifier.ValueText)
                .Concat(tree.GetRoot()
                    .DescendantNodes()
                    .OfType<DelegateDeclarationSyntax>()
                    .Select(delegateDeclaration => delegateDeclaration.Identifier.ValueText));

        private static bool IsProhibitedDeclaration(SyntaxNode node, bool isProductionSource)
            => node is VariableDeclarationSyntax declaration && declaration.Type.IsVar
                || node is ConditionalExpressionSyntax
                || node is TupleTypeSyntax
                || node is TupleExpressionSyntax
                || node is AccessorDeclarationSyntax accessor && accessor.IsKind(SyntaxKind.InitAccessorDeclaration)
                || node is BaseTypeDeclarationSyntax typeDeclaration && typeDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)
                || isProductionSource && node is FieldDeclarationSyntax field && field.Modifiers.Any(SyntaxKind.ConstKeyword);

        private static StatementSyntax? GetUnbracedBody(SyntaxNode node)
        {
            StatementSyntax? body = node switch
            {
                IfStatementSyntax ifStatement => ifStatement.Statement,
                ForStatementSyntax forStatement => forStatement.Statement,
                ForEachStatementSyntax forEachStatement => forEachStatement.Statement,
                WhileStatementSyntax whileStatement => whileStatement.Statement,
                DoStatementSyntax doStatement => doStatement.Statement,
                LockStatementSyntax lockStatement => lockStatement.Statement,
                UsingStatementSyntax usingStatement => usingStatement.Statement,
                _ => null
            };

            if (body is BlockSyntax)
            {
                return null;
            }

            return body;
        }

        private static IEnumerable<string> FindUsingViolations(SyntaxTree tree)
        {
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            List<string> violations = [];
            List<int> lines = [];
            List<string> keys = [];
            List<string> namespaces = [];

            foreach (UsingDirectiveSyntax directive in root.Usings)
            {
                string? namespaceName = GetImportedNamespace(directive);

                if (namespaceName is null)
                {
                    violations.Add("every using directive must import a namespace.");

                    continue;
                }

                lines.Add(directive.GetLocation().GetLineSpan().StartLinePosition.Line);
                keys.Add(GetUsingGroupKey(namespaceName, directive.StaticKeyword.IsKind(SyntaxKind.StaticKeyword)));
                namespaces.Add(namespaceName);
            }

            if (lines.Count == 0)
            {
                return violations;
            }

            for (int index = 1; index < lines.Count; index += 1)
            {
                string previousKey = keys[index - 1];
                string previousNamespace = namespaces[index - 1];
                string currentKey = keys[index];
                string currentNamespace = namespaces[index];

                if (string.CompareOrdinal(previousKey, currentKey) > 0)
                {
                    violations.Add($"'{previousNamespace}' must not precede '{currentNamespace}'; group order is System, Microsoft, one alphabetised group per third-party root, and then static imports.");
                }

                if (string.Equals(previousKey, currentKey, StringComparison.Ordinal)
                    && string.CompareOrdinal(previousNamespace.ToLowerInvariant(), currentNamespace.ToLowerInvariant()) > 0)
                {
                    violations.Add($"'{currentNamespace}' must be alphabetised after '{previousNamespace}'.");
                }

                int gap = lines[index] - lines[index - 1] - 1;
                bool sameGroup = string.Equals(previousKey, currentKey, StringComparison.Ordinal);

                if (sameGroup && gap != 0)
                {
                    violations.Add($"'{previousNamespace}' and '{currentNamespace}' belong to the same import group and must not be separated by a blank line.");
                }

                if (!sameGroup && gap != 1)
                {
                    violations.Add($"using groups must be separated by exactly one blank line before '{currentNamespace}'.");
                }
            }

            NamespaceDeclarationSyntax? declaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            if (declaration is not null)
            {
                int namespaceLine = declaration.GetLocation().GetLineSpan().StartLinePosition.Line;
                int lastUsingLine = root.Usings.Last().GetLocation().GetLineSpan().StartLinePosition.Line;

                if (namespaceLine - lastUsingLine - 1 != 1)
                {
                    violations.Add("exactly one blank line must separate the using directives from the namespace.");
                }
            }

            return violations;
        }

        private static string? GetImportedNamespace(UsingDirectiveSyntax directive)
        {
            if (directive.Name is null)
            {
                return null;
            }

            return directive.Name.ToString();
        }

        private static string GetUsingGroupKey(string namespaceName, bool isStaticImport)
        {
            if (isStaticImport)
            {
                return "4:static";
            }

            if (string.Equals(namespaceName, "System", StringComparison.Ordinal)
                || namespaceName.StartsWith("System.", StringComparison.Ordinal))
            {
                return "0:system";
            }

            if (string.Equals(namespaceName, "Microsoft", StringComparison.Ordinal)
                || namespaceName.StartsWith("Microsoft.", StringComparison.Ordinal))
            {
                return "1:microsoft";
            }

            if (namespaceName.StartsWith("NuciXNA.Gui", StringComparison.Ordinal))
            {
                return "3:nucixna.gui";
            }

            string rootNamespace = namespaceName.Split('.')[0];

            return "2:" + rootNamespace.ToLowerInvariant();
        }
    }
}
