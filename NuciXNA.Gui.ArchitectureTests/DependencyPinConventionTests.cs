using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using NUnit.Framework;

namespace NuciXNA.Gui.ArchitectureTests
{
    /// <summary>
    /// Dependency-pinning and reference-direction checks, plus non-vacuity guards for the suite.
    /// </summary>
    [TestFixture]
    public sealed class DependencyPinConventionTests
    {
        private static readonly string[] ProjectPaths =
        [
            "NuciXNA.Gui/NuciXNA.Gui.csproj",
            "NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj",
            "NuciXNA.Gui.ArchitectureTests/NuciXNA.Gui.ArchitectureTests.csproj"
        ];

        private static readonly Regex ExactVersionPattern = new(@"^\d+\.\d+\.\d+([-+][0-9A-Za-z.\-]+)?$", RegexOptions.Compiled);

        [TestCase("NuciXNA.Gui/NuciXNA.Gui.csproj")]
        [TestCase("NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj")]
        [TestCase("NuciXNA.Gui.ArchitectureTests/NuciXNA.Gui.ArchitectureTests.csproj")]
        public void GivenProjects_WhenCheckingPackages_ThenEveryVersionIsPinned(string projectPath)
        {
            string[] unpinned = ReadPackageVersions(LoadProject(projectPath))
                .Where(pair => !ExactVersionPattern.IsMatch(pair.Value))
                .Select(pair => $"{pair.Key}={pair.Value}")
                .ToArray();

            Assert.That(
                unpinned,
                Is.Empty,
                $"{projectPath}: every package reference must declare an exact version ({string.Join(", ", unpinned)}).");
        }

        [TestCase("NuciXNA.Gui/NuciXNA.Gui.csproj")]
        [TestCase("NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj")]
        [TestCase("NuciXNA.Gui.ArchitectureTests/NuciXNA.Gui.ArchitectureTests.csproj")]
        public void GivenProjects_WhenCheckingPackages_ThenNoFloatingOrWildcardRangeIsUsed(string projectPath)
        {
            string[] floating = ReadPackageVersions(LoadProject(projectPath))
                .Select(pair => pair.Value)
                .Where(version => version.Contains('*', StringComparison.Ordinal)
                    || version.Contains('[', StringComparison.Ordinal)
                    || version.StartsWith("^", StringComparison.Ordinal)
                    || version.StartsWith("~", StringComparison.Ordinal))
                .ToArray();

            Assert.That(floating, Is.Empty, $"{projectPath}: floating version ranges are prohibited.");
        }

        [Test]
        public void GivenProjects_WhenCheckingReferences_ThenOnlyProjectReferencesAreUsedAcrossProjects()
        {
            List<string> violations = [];

            foreach (string projectPath in ProjectPaths)
            {
                XDocument project = LoadProject(projectPath);

                foreach (XElement reference in project.Descendants("ProjectReference"))
                {
                    string include = GetAttribute(reference, "Include").Replace('\\', '/');

                    if (!include.EndsWith(".csproj", StringComparison.Ordinal)
                        || !include.StartsWith("..", StringComparison.Ordinal))
                    {
                        violations.Add($"{projectPath}: '{include}' must be a sibling-relative .csproj reference.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public void GivenSolution_WhenCheckingProjects_ThenEveryDeclaredProjectIsKnown()
        {
            XDocument solution = XDocument.Parse(ArchitectureResources.ReadResource(
                ArchitectureResources.ProjectFilePrefix,
                "NuciXNA.GUI.slnx"));

            string[] declared = solution.Descendants()
                .Where(element => string.Equals(element.Name.LocalName, "Project", StringComparison.Ordinal))
                .Select(element => GetAttribute(element, "Path").Replace('\\', '/'))
                .ToArray();

            Assert.That(declared, Is.Not.Empty, "Vacuous-selector guard: the solution declares no projects.");

            string[] missing = declared
                .Where(path => !ProjectPaths.Contains(path, StringComparer.Ordinal))
                .ToArray();

            Assert.That(missing, Is.Empty, $"Violated convention: the solution declares unknown projects ({string.Join(", ", missing)}).");
        }

        [Test]
        public void GivenArchitectureAssemblyInventory_WhenInspected_ThenTheProductionAssemblyIsLoaded()
        {
            string[] modelled = ProductionArchitecture.Model.Assemblies
                .Select(assembly => assembly.Name)
                .ToArray();

            Assert.That(modelled, Is.Not.Empty, "Vacuous-selector guard: no assembly was modelled.");
            Assert.That(
                modelled,
                Does.Contain(ProductionArchitecture.ProductionAssembly.GetName().Name!),
                "Violated boundary: the production assembly must be part of the modelled architecture.");
        }

        [Test]
        public void GivenArchitectureTypes_WhenSelected_ThenTheTypeProviderIsNotEmpty()
        {
            int productionTypeCount = ProductionArchitecture.ProductionTypes
                .GetObjects(ProductionArchitecture.Model)
                .Count();

            Assert.That(productionTypeCount, Is.GreaterThan(0), "Vacuous-selector guard: the production type provider selected nothing.");
        }

        [Test]
        public void GivenEmbeddedSources_WhenEnumerated_ThenEveryPrefixContainsFiles()
        {
            foreach (string prefix in ArchitectureResources.SourcePrefixes)
            {
                IReadOnlyList<string> paths = ArchitectureResources.GetResourcePaths(prefix);

                Assert.That(paths, Is.Not.Empty, $"Vacuous-selector guard: no embedded sources were found under '{prefix}'.");
            }
        }

        [Test]
        public void GivenProjectFiles_WhenEnumerated_ThenEveryEmbeddedProjectIsAvailable()
        {
            IReadOnlyList<string> projectFiles = ArchitectureResources.GetResourcePaths(ArchitectureResources.ProjectFilePrefix);

            Assert.That(projectFiles, Is.Not.Empty, "Vacuous-selector guard: no embedded project files were found.");
            Assert.That(
                projectFiles,
                Is.SupersetOf(new[] { "NuciXNA.GUI.slnx" }),
                "Violated convention: the embedded solution file must be available.");
            Assert.That(
                projectFiles,
                Is.SupersetOf(ProjectPaths),
                "Violated convention: every project file must be embedded for the convention checks.");
        }

        private static IReadOnlyList<KeyValuePair<string, string>> ReadPackageVersions(XDocument project)
            => project.Descendants("PackageReference")
                .Select(reference => new KeyValuePair<string, string>(
                    GetAttribute(reference, "Include"),
                    GetAttribute(reference, "Version")))
                .ToArray();

        private static string GetAttribute(XElement element, string attributeName)
        {
            XAttribute? attribute = element.Attribute(attributeName);

            if (attribute is null)
            {
                return string.Empty;
            }

            return attribute.Value;
        }

        private static XDocument LoadProject(string relativePath)
            => XDocument.Parse(ArchitectureResources.ReadResource(
                ArchitectureResources.ProjectFilePrefix,
                relativePath));
    }
}
