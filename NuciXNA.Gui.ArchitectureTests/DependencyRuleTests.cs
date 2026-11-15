using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

using NUnit.Framework;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NuciXNA.Gui.ArchitectureTests
{
    /// <summary>
    /// Dependency, boundary, and public-surface checks over the loaded production architecture.
    /// </summary>
    [TestFixture]
    public sealed class DependencyRuleTests
    {
        private static readonly string[] ExpectedControlTypes =
        [
            "NuciXNA.Gui.Controls.GuiControl",
            "NuciXNA.Gui.Controls.GuiImage",
            "NuciXNA.Gui.Controls.GuiMenuItem",
            "NuciXNA.Gui.Controls.GuiMenuLink",
            "NuciXNA.Gui.Controls.GuiMenuListSelector",
            "NuciXNA.Gui.Controls.GuiMenuText",
            "NuciXNA.Gui.Controls.GuiMenuToggle",
            "NuciXNA.Gui.Controls.GuiText",
            "NuciXNA.Gui.Controls.GuiTooltip",
            "NuciXNA.Gui.Controls.IGuiControl",
            "NuciXNA.Gui.Controls.ListSelectionEventArgs"
        ];

        private static readonly string[] ExpectedScreenTypes =
        [
            "NuciXNA.Gui.Screens.IScreen",
            "NuciXNA.Gui.Screens.MenuScreen",
            "NuciXNA.Gui.Screens.MenuScreenAxis",
            "NuciXNA.Gui.Screens.Screen",
            "NuciXNA.Gui.Screens.ScreenManager"
        ];

        private static readonly string[] ExpectedRootTypes =
        [
            "NuciXNA.Gui.Cursor",
            "NuciXNA.Gui.GuiManager"
        ];

        private static readonly string[] AllowedAssemblyReferences =
        [
            "MonoGame.Framework",
            "NuciXNA.Graphics",
            "NuciXNA.Input",
            "NuciXNA.Primitives"
        ];

        private static readonly string[] ProhibitedNamespacePatterns =
        [
            @"Microsoft\.EntityFrameworkCore(\..*)?",
            @"Amazon\.DynamoDBv2(\..*)?",
            @"Dapper(\..*)?",
            @"Newtonsoft\.Json(\..*)?",
            @"System\.Net\.Http(\..*)?",
            @"NUnit(\..*)?",
            @"Moq(\..*)?",
            @"ArchUnitNET(\..*)?"
        ];

        [Test]
        public void GivenProductionArchitecture_WhenLoaded_ThenOnlyTheProductionAssemblyIsModelled()
        {
            string[] modelledAssemblies = ProductionArchitecture.Model.Assemblies
                .Select(assembly => assembly.Name)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            Assert.That(
                modelledAssemblies,
                Is.EquivalentTo([ProductionArchitecture.ProductionAssembly.GetName().Name]),
                "Violated boundary: only the production assembly must be modelled.");
        }

        [Test]
        public void GivenProductionAssembly_WhenInspectingExportedTypes_ThenEveryDocumentedTypeIsPresent()
        {
            string[] exportedTypes = GetExportedTypeNames();

            Assert.That(
                exportedTypes,
                Is.SupersetOf(ExpectedRootTypes),
                "Violated boundary: a documented root type is missing from the public surface.");
            Assert.That(
                exportedTypes,
                Is.SupersetOf(ExpectedControlTypes),
                "Violated boundary: a documented control type is missing from the public surface.");
            Assert.That(
                exportedTypes,
                Is.SupersetOf(ExpectedScreenTypes),
                "Violated boundary: a documented screen type is missing from the public surface.");
        }

        [Test]
        public void GivenProductionTypes_WhenCheckingNamespacePlacement_ThenAllResideInDocumentedNamespaces()
        {
            string[] unexpectedTypes = GetProductionTypeNames()
                .Where(name => !name.StartsWith("NuciXNA.Gui.", StringComparison.Ordinal)
                    && !string.Equals(name, "NuciXNA.Gui", StringComparison.Ordinal))
                .ToArray();

            Assert.That(
                unexpectedTypes,
                Is.Empty,
                "Violated boundary: every production type must reside under NuciXNA.Gui.");
        }

        [Test]
        public void GivenProductionTypes_WhenCheckingExternalDependencies_ThenNoProhibitedFrameworkIsUsed()
        {
            AssertProductionTypesSelected();

            foreach (string namespacePattern in ProhibitedNamespacePatterns)
            {
                IArchRule rule = Types()
                    .That()
                    .Are(ProductionArchitecture.ProductionTypes)
                    .Should()
                    .NotDependOnAnyTypesThat()
                    .ResideInNamespaceMatching(namespacePattern)
                    .Because("the GUI library must stay free of persistence, web, serialisation, and test dependencies");

                AssertNoViolations(rule);
            }
        }

        [Test]
        public void GivenGuiManagerTypes_WhenCheckingDependencies_ThenScreenManagerIsNotReferenced()
        {
            IArchRule rule = Types()
                .That()
                .HaveNameStartingWith("GuiManager")
                .Should()
                .NotDependOnAnyTypesThat()
                .ResideInNamespace("NuciXNA.Gui.Screens")
                .Because("GuiManager must not depend on ScreenManager");

            AssertNoViolations(rule);
        }

        [Test]
        public void GivenControlTypes_WhenInspectingDeclarations_ThenEachIsAControlImplementingTheContract()
        {
            Type[] controlTypes = GetProductionTypesInNamespace("NuciXNA.Gui.Controls")
                .Where(type => typeof(Controls.GuiControl).IsAssignableFrom(type) && !type.IsAbstract)
                .ToArray();

            Assert.That(controlTypes, Is.Not.Empty, "Vacuous-selector guard: no concrete control types were found.");

            foreach (Type controlType in controlTypes)
            {
                Assert.That(
                    typeof(Controls.IGuiControl).IsAssignableFrom(controlType),
                    Is.True,
                    $"Violated boundary: {controlType.FullName} must implement IGuiControl.");
            }
        }

        [Test]
        public void GivenScreenTypes_WhenInspectingDeclarations_ThenEachScreenImplementsTheContract()
        {
            Type[] screenTypes = GetProductionTypesInNamespace("NuciXNA.Gui.Screens")
                .Where(type => typeof(Screens.Screen).IsAssignableFrom(type) && !type.IsAbstract)
                .ToArray();

            Assert.That(screenTypes, Is.Not.Empty, "Vacuous-selector guard: no concrete screen types were found.");

            foreach (Type screenType in screenTypes)
            {
                Assert.That(
                    typeof(Screens.IScreen).IsAssignableFrom(screenType),
                    Is.True,
                    $"Violated boundary: {screenType.FullName} must implement IScreen.");
            }
        }

        [Test]
        public void GivenManagerTypes_WhenInspectingDeclarations_ThenBothExposeProcessWideInstances()
        {
            PropertyInfo? guiManagerInstance = typeof(GuiManager).GetProperty(
                "Instance",
                BindingFlags.Public | BindingFlags.Static);
            PropertyInfo? screenManagerInstance = typeof(Screens.ScreenManager).GetProperty(
                "Instance",
                BindingFlags.Public | BindingFlags.Static);

            Assert.That(
                guiManagerInstance,
                Is.Not.Null,
                "Violated boundary: GuiManager must expose a static Instance property.");
            Assert.That(
                guiManagerInstance!.PropertyType,
                Is.EqualTo(typeof(GuiManager)),
                "Violated boundary: GuiManager.Instance must return GuiManager.");
            Assert.That(
                screenManagerInstance,
                Is.Not.Null,
                "Violated boundary: ScreenManager must expose a static Instance property.");
            Assert.That(
                screenManagerInstance!.PropertyType,
                Is.EqualTo(typeof(Screens.ScreenManager)),
                "Violated boundary: ScreenManager.Instance must return ScreenManager.");
        }

        [Test]
        public void GivenCursorType_WhenInspectingDeclarations_ThenItIsAnOwnedNonSingletonType()
        {
            Assert.That(
                typeof(Cursor).IsSealed,
                Is.True,
                "Violated boundary: Cursor must remain a sealed class.");

            Assert.That(
                typeof(Cursor).GetProperty("Instance", BindingFlags.Public | BindingFlags.Static),
                Is.Null,
                "Violated boundary: Cursor must not become a singleton; the host owns each instance.");
        }

        [Test]
        public void GivenProductionAssembly_WhenCheckingFriendAssemblies_ThenInternalsAreNotExposed()
        {
            InternalsVisibleToAttribute[] friendAssemblies = ProductionArchitecture.ProductionAssembly
                .GetCustomAttributes<InternalsVisibleToAttribute>()
                .ToArray();

            Assert.That(
                friendAssemblies,
                Is.Empty,
                "Violated boundary: production code must not declare InternalsVisibleTo.");
        }

        [Test]
        public void GivenProductionAssembly_WhenInspectingReferences_ThenOnlyDocumentedAssembliesAreReferenced()
        {
            string[] unexpectedReferences = GetNonFrameworkReferences()
                .Where(reference => !AllowedAssemblyReferences.Contains(reference, StringComparer.Ordinal))
                .ToArray();

            Assert.That(
                GetNonFrameworkReferences(),
                Is.Not.Empty,
                "Violated boundary: the assembly references must be inspectable.");
            Assert.That(
                unexpectedReferences,
                Is.Empty,
                "Violated boundary: the production assembly references an undocumented assembly.");
        }

        [Test]
        public void GivenProductionAssembly_WhenInspectingReferences_ThenNoTestFrameworkIsReferenced()
        {
            string[] references = GetNonFrameworkReferences()
                .Where(reference => reference.Contains("NUnit", StringComparison.OrdinalIgnoreCase)
                    || reference.Contains("Moq", StringComparison.OrdinalIgnoreCase)
                    || reference.Contains("ArchUnit", StringComparison.OrdinalIgnoreCase)
                    || reference.Contains("Test", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            Assert.That(
                references,
                Is.Empty,
                "Violated boundary: the production assembly must not reference test frameworks.");
        }

        private static void AssertProductionTypesSelected()
            => Assert.That(
                ProductionArchitecture.ProductionTypes.GetObjects(ProductionArchitecture.Model),
                Is.Not.Empty,
                "Vacuous-selector guard: no production types were selected.");

        private static void AssertNoViolations(IArchRule archRule)
        {
            EvaluationResult[] failures = archRule
                .Evaluate(ProductionArchitecture.Model)
                .Where(result => !result.Passed)
                .ToArray();

            Assert.That(
                failures,
                Is.Empty,
                "Violated boundary: " + archRule.Description);
        }

        private static string[] GetNonFrameworkReferences()
            => ProductionArchitecture.ProductionAssembly
                .GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .OfType<string>()
                .Where(name => !name.StartsWith("System", StringComparison.Ordinal)
                    && !string.Equals(name, "netstandard", StringComparison.Ordinal)
                    && !string.Equals(name, "mscorlib", StringComparison.Ordinal))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

        private static string[] GetExportedTypeNames()
            => ProductionArchitecture.ProductionAssembly
                .GetExportedTypes()
                .Select(type => type.FullName)
                .OfType<string>()
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

        private static IEnumerable<Type> GetProductionTypesInNamespace(string @namespace)
            => ProductionArchitecture.ProductionAssembly
                .GetTypes()
                .Where(type => !IsCompilerGenerated(type))
                .Where(type => string.Equals(type.Namespace, @namespace, StringComparison.Ordinal));

        private static IEnumerable<string> GetProductionTypeNames()
            => ProductionArchitecture.ProductionAssembly
                .GetTypes()
                .Where(type => !IsCompilerGenerated(type))
                .Select(type => type.FullName)
                .OfType<string>()
                .OrderBy(name => name, StringComparer.Ordinal);

        private static bool IsCompilerGenerated(Type type)
            => type.Name.StartsWith('<') || type.GetCustomAttribute<CompilerGeneratedAttribute>() is not null;
    }
}
