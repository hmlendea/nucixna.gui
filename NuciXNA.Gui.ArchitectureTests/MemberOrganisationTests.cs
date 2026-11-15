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
    /// Member organisation, indentation, and formatting checks over the handwritten sources.
    /// </summary>
    [TestFixture]
    public sealed class MemberOrganisationTests
    {
        private static readonly Regex TrailingWhitespacePattern = new("[ \t]+(\\r?\\n)", RegexOptions.Compiled);

        private static readonly Regex ConsecutiveBlankLinePattern = new("\\n[ \t]*\\n[ \t]*\\n", RegexOptions.Compiled);

        private static readonly Regex BlankLineBeforeClosingDelimiterPattern = new("\\n[ \t]*\\n[ \t]*[}\\])]", RegexOptions.Compiled);

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingAccessibility_ThenEveryMemberDeclaresItExplicitly(string sourcePrefix)
        {
            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                SyntaxNode[] members = tree.GetRoot()
                    .DescendantNodes()
                    .Where(SourceParsing.IsAccessibilityScopedMember)
                    .Where(member => !SourceParsing.DeclaresAccessibility(member))
                    .ToArray();

                Assert.That(
                    members,
                    Is.Empty,
                    $"Violated convention in {tree.FilePath}: every field, property, event, constructor, and method must declare explicit accessibility.");
            }
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingMemberKindOrder_ThenNonMethodsPrecedeMethods(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (TypeDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    MemberDeclarationSyntax[] members = declaration.Members
                        .Where(SourceParsing.IsAccessibilityScopedMember)
                        .ToArray();

                    int lastMethodIndex = Array.FindLastIndex(members, member => member is MethodDeclarationSyntax);
                    int lastNonMethodIndex = Array.FindLastIndex(members, member => member is not MethodDeclarationSyntax);

                    if (lastMethodIndex < 0 || lastNonMethodIndex < 0 || lastNonMethodIndex < lastMethodIndex)
                    {
                        continue;
                    }

                    violations.Add(SourceParsing.Describe(
                        tree,
                        members[lastNonMethodIndex],
                        $"fields, properties, events, and constructors of '{declaration.Identifier.ValueText}' must appear before its methods."));
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingSingleStatementMethods_ThenTheyAreExpressionBodied(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<MethodDeclarationSyntax> offenders = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(method => method.ExpressionBody is null)
                    .Where(method => method.Body is not null && method.Body.Statements.Count == 1)
                    .Where(method => method.Body!.Statements[0] is ReturnStatementSyntax
                        || method.Body!.Statements[0] is ThrowStatementSyntax);

                violations.AddRange(offenders.Select(method => SourceParsing.Describe(
                    tree,
                    method,
                    "single-statement methods must be expression-bodied.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingFormatting_ThenWhitespaceConventionsHold(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string source = SourceParsing.ReadSource(sourcePrefix, tree.FilePath);

                if (source.IndexOf('\t') >= 0)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: tabs are prohibited; use four spaces.");
                }

                if (TrailingWhitespacePattern.IsMatch(source))
                {
                    violations.Add($"Violated convention in {tree.FilePath}: trailing whitespace is prohibited.");
                }

                if (ConsecutiveBlankLinePattern.IsMatch(source))
                {
                    violations.Add($"Violated convention in {tree.FilePath}: consecutive blank lines are prohibited.");
                }

                if (BlankLineBeforeClosingDelimiterPattern.IsMatch(source))
                {
                    violations.Add($"Violated convention in {tree.FilePath}: no blank line may precede a closing delimiter.");
                }

                if (!source.EndsWith(Environment.NewLine, StringComparison.Ordinal))
                {
                    violations.Add($"Violated convention in {tree.FilePath}: every file must end with an empty line.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingIndentation_ThenFourSpacesAreUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                string[] lines = SourceParsing.ReadSource(sourcePrefix, tree.FilePath)
                    .Replace("\r\n", "\n", StringComparison.Ordinal)
                    .Split('\n');

                for (int index = 0; index < lines.Length; index += 1)
                {
                    string line = lines[index];
                    int indentation = line.Length - line.TrimStart(' ').Length;

                    if (line.Trim().Length == 0)
                    {
                        continue;
                    }

                    if (indentation % 4 != 0)
                    {
                        violations.Add($"Violated convention in {tree.FilePath}:{index + 1}: indentation must use four spaces.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingUsingPlacement_ThenAllUsingsAreOutsideTheNamespace(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                UsingDirectiveSyntax[] nested = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<NamespaceDeclarationSyntax>()
                    .SelectMany(declaration => declaration.Usings)
                    .ToArray();

                if (nested.Length > 0)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: all using directives must be placed above and outside the namespace.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingNamespaceSpacing_ThenExactlyOneBlankLinePrecedesIt(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                NamespaceDeclarationSyntax? declaration = root.DescendantNodes()
                    .OfType<NamespaceDeclarationSyntax>()
                    .FirstOrDefault();

                if (declaration is null || root.Usings.Count == 0)
                {
                    continue;
                }

                int namespaceLine = declaration.GetLocation().GetLineSpan().StartLinePosition.Line;
                int lastUsingLine = root.Usings.Last().GetLocation().GetLineSpan().EndLinePosition.Line;

                if (namespaceLine - lastUsingLine - 1 != 1)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: exactly one blank line must separate the using directives from the namespace.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingFieldOrder_ThenStaticReadonlyFieldsPrecedeInstanceFields(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                foreach (TypeDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    int lastRank = -1;

                    foreach (FieldDeclarationSyntax field in declaration.Members.OfType<FieldDeclarationSyntax>())
                    {
                        int rank = GetFieldRank(field);

                        if (rank < lastRank)
                        {
                            violations.Add(SourceParsing.Describe(
                                tree,
                                field,
                                "fields must be ordered static readonly, static mutable, instance readonly, then instance mutable."));
                        }

                        lastRank = Math.Max(lastRank, rank);
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingStateProperties_ThenAutoPropertiesAreUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<PropertyDeclarationSyntax> statefulProperties = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .Where(property => property.AccessorList is not null)
                    .Where(property => property.AccessorList!.Accessors.Count == 2)
                    .Where(property => property.AccessorList!.Accessors.All(accessor => accessor.Body is null));

                foreach (PropertyDeclarationSyntax property in statefulProperties)
                {
                    if (property.AccessorList!.Accessors.Any(accessor => accessor.Body is not null))
                    {
                        violations.Add(SourceParsing.Describe(tree, property, "state properties must use auto-property syntax."));
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        public void GivenProductionSources_WhenCheckingProperties_ThenNoInitAccessorsAreUsed(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                IEnumerable<AccessorDeclarationSyntax> initAccessors = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<AccessorDeclarationSyntax>()
                    .Where(accessor => accessor.IsKind(SyntaxKind.InitAccessorDeclaration));

                violations.AddRange(initAccessors.Select(accessor => SourceParsing.Describe(
                    tree,
                    accessor,
                    "init accessors are prohibited; use get/set auto-properties.")));
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        private static int GetFieldRank(FieldDeclarationSyntax field)
        {
            bool isStatic = field.Modifiers.Any(SyntaxKind.StaticKeyword);
            bool isReadonly = field.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);

            if (isStatic && isReadonly)
            {
                return 0;
            }

            if (isStatic)
            {
                return 1;
            }

            if (isReadonly)
            {
                return 2;
            }

            return 3;
        }

        [TestCase(ArchitectureResources.ProductionSourcePrefix)]
        [TestCase(ArchitectureResources.UnitTestSourcePrefix)]
        [TestCase(ArchitectureResources.ArchitectureTestSourcePrefix)]
        public void GivenSources_WhenCheckingRegions_ThenNoneAreDeclared(string sourcePrefix)
        {
            List<string> violations = [];

            foreach (SyntaxTree tree in SourceParsing.CollectParseTrees(sourcePrefix))
            {
                SyntaxTrivia[] regions = tree.GetRoot()
                    .DescendantTrivia()
                    .Where(trivia => trivia.IsKind(SyntaxKind.RegionDirectiveTrivia)
                        || trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia))
                    .ToArray();

                if (regions.Length > 0)
                {
                    violations.Add($"Violated convention in {tree.FilePath}: regions are prohibited.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }
    }
}
