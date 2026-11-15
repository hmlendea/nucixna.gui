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
    /// Shared parsing helpers for the Roslyn source-convention checks.
    /// </summary>
    internal static class SourceParsing
    {
        internal static IEnumerable<SyntaxTree> CollectParseTrees(string sourcePrefix)
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(sourcePrefix);

            Assert.That(
                sources,
                Is.Not.Empty,
                $"Violated convention: no source files were found under '{sourcePrefix}'.");

            return sources.Select(source => ParseTree(sourcePrefix, source));
        }

        internal static SyntaxTree ParseTree(string sourcePrefix, string source)
            => CSharpSyntaxTree.ParseText(
                ArchitectureResources.ReadResource(sourcePrefix, source),
                path: source);

        internal static string ReadSource(string sourcePrefix, string source)
            => ArchitectureResources.ReadResource(sourcePrefix, source);

        internal static string GetFileNameStem(string path)
        {
            string fileName = path.Substring(path.LastIndexOf('/') + 1);

            return fileName.Substring(0, fileName.LastIndexOf('.'));
        }

        internal static string GetFolderName(string path)
        {
            string[] segments = path.Replace('\\', '/').Split('/');

            if (segments.Length < 2)
            {
                return string.Empty;
            }

            return segments[segments.Length - 2];
        }

        internal static string GetProjectName(string path)
        {
            string[] segments = path.Replace('\\', '/').Split('/');

            if (segments.Length == 0)
            {
                return string.Empty;
            }

            return segments[0];
        }

        internal static string GetProjectName(string prefix, string path)
        {
            if (string.Equals(prefix, ArchitectureResources.UnitTestSourcePrefix, StringComparison.Ordinal))
            {
                return "NuciXNA.Gui.UnitTests";
            }

            if (string.Equals(prefix, ArchitectureResources.ArchitectureTestSourcePrefix, StringComparison.Ordinal))
            {
                return "NuciXNA.Gui.ArchitectureTests";
            }

            return "NuciXNA.Gui";
        }

        internal static string GetSourcePrefix(string path)
        {
            string normalisedPath = path.Replace('\\', '/');

            if (normalisedPath.StartsWith("NuciXNA.Gui.ArchitectureTests/", StringComparison.Ordinal))
            {
                return ArchitectureResources.ArchitectureTestSourcePrefix;
            }

            if (normalisedPath.StartsWith("NuciXNA.Gui.UnitTests/", StringComparison.Ordinal))
            {
                return ArchitectureResources.UnitTestSourcePrefix;
            }

            return ArchitectureResources.ProductionSourcePrefix;
        }

        internal static bool IsAccessibilityScopedMember(SyntaxNode member)
            => (member is FieldDeclarationSyntax
                    || member is PropertyDeclarationSyntax
                    || member is EventFieldDeclarationSyntax
                    || member is ConstructorDeclarationSyntax
                    || member is MethodDeclarationSyntax)
                && !member.Ancestors().Any(ancestor => ancestor is InterfaceDeclarationSyntax or EnumDeclarationSyntax);

        internal static bool DeclaresAccessibility(SyntaxNode member)
        {
            SyntaxTokenList modifiers = member switch
            {
                FieldDeclarationSyntax field => field.Modifiers,
                PropertyDeclarationSyntax property => property.Modifiers,
                EventFieldDeclarationSyntax eventField => eventField.Modifiers,
                ConstructorDeclarationSyntax constructor => constructor.Modifiers,
                MethodDeclarationSyntax method => method.Modifiers,
                _ => default
            };

            return modifiers.Any(modifier =>
                modifier.IsKind(SyntaxKind.PublicKeyword) ||
                modifier.IsKind(SyntaxKind.ProtectedKeyword) ||
                modifier.IsKind(SyntaxKind.InternalKeyword) ||
                modifier.IsKind(SyntaxKind.PrivateKeyword));
        }

        internal static string Describe(SyntaxTree tree, SyntaxNode node, string reason)
            => $"Violated convention in {tree.FilePath}:{GetLineNumber(node)}: {reason}";

        internal static int GetLineNumber(SyntaxNode node)
            => node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
    }
}
