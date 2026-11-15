# Testing

## Test Stack

[`NuciXNA.Gui.UnitTests`](../NuciXNA.Gui.UnitTests/) targets `net10.0` and uses:
- NUnit `5.0.0`.
- NUnit3TestAdapter `6.3.1`.
- Microsoft.NET.Test.Sdk `18.11.0`.
- Direct reference to the library project.
- `DummyControl` and `DummyScreen` test doubles.

[`NuciXNA.Gui.ArchitectureTests`](../NuciXNA.Gui.ArchitectureTests/) targets `net10.0` and uses:
- NUnit `5.0.0`, NUnit3TestAdapter `6.3.1`, and Microsoft.NET.Test.Sdk `18.11.0`.
- ArchUnitNET `0.13.5` for compiled dependency and public-surface rules.
- Microsoft.CodeAnalysis.CSharp `5.9.0` for source-file and test-source convention rules.
- Embedded copies of the production, unit-test, and architecture-test sources, plus project and solution files, so rules never depend on the local file system.

## Current Validation

On 2026-11-15, `dotnet test` across the solution discovered and passed 457 unit tests and 194 architecture tests with no failures or skips. This is a point-in-time result, not a permanent coverage guarantee.

## Commands

```bash
# Complete solution
dotnet test NuciXNA.GUI.slnx

# Test project only
dotnet test NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj

# Release configuration after an explicit build
dotnet build NuciXNA.GUI.slnx --configuration Release
dotnet test NuciXNA.GUI.slnx --configuration Release --no-build
```

The repository has no configured coverage collector/report, snapshot test framework, benchmark project, integration-test project, or end-to-end host.

## Test Organisation

### Base Controls

- [`ControlTests.cs`](../NuciXNA.Gui.UnitTests/Controls/ControlTests.cs)
- [`GuiControlLifecycleTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiControlLifecycleTests.cs)
- [`GuiControlPropertyTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiControlPropertyTests.cs)

These use [`DummyControl.cs`](../NuciXNA.Gui.UnitTests/Helpers/DummyControl.cs) for base lifecycle, property, event, inheritance, and disposal behaviour.

### Menu Controls

- [`GuiMenuItemTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuItemTests.cs) and [`GuiMenuItemExtendedTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuItemExtendedTests.cs)
- [`GuiMenuLinkTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuLinkTests.cs) and [`GuiMenuLinkExtendedTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuLinkExtendedTests.cs)
- [`GuiMenuTextTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuTextTests.cs)
- [`GuiMenuToggleTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuToggleTests.cs) and [`GuiMenuToggleExtendedTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuToggleExtendedTests.cs)
- [`GuiMenuListSelectorTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuListSelectorTests.cs) and [`GuiMenuListSelectorExtendedTests.cs`](../NuciXNA.Gui.UnitTests/Controls/GuiMenuListSelectorExtendedTests.cs)
- [`ListSelectionEventArgsTests.cs`](../NuciXNA.Gui.UnitTests/Controls/ListSelectionEventArgsTests.cs)

### Screens

- [`ScreenTests.cs`](../NuciXNA.Gui.UnitTests/Screens/ScreenTests.cs)
- [`ScreenExtendedTests.cs`](../NuciXNA.Gui.UnitTests/Screens/ScreenExtendedTests.cs)
- [`DummyScreen.cs`](../NuciXNA.Gui.UnitTests/Helpers/DummyScreen.cs)

### Unit Tests

The unit-test project covers control and screen behaviour:

```text
NuciXNA.Gui.UnitTests/
├── Controls/
│   ├── ControlTests.cs
│   ├── GuiControlLifecycleTests.cs
│   ├── GuiControlPropertyTests.cs
│   ├── GuiMenuItemTests.cs
│   ├── GuiMenuItemExtendedTests.cs
│   ├── GuiMenuLinkTests.cs
│   ├── GuiMenuLinkExtendedTests.cs
│   ├── GuiMenuListSelectorTests.cs
│   ├── GuiMenuListSelectorExtendedTests.cs
│   ├── GuiMenuTextTests.cs
│   ├── GuiMenuToggleTests.cs
│   ├── GuiMenuToggleExtendedTests.cs
│   └── ListSelectionEventArgsTests.cs
├── Screens/
│   ├── ScreenTests.cs
│   └── ScreenExtendedTests.cs
└── Helpers/
    ├── DummyControl.cs
    └── DummyScreen.cs
```

### Architecture Tests

The architecture-test project enforces documented boundaries and conventions:

```text
NuciXNA.Gui.ArchitectureTests/
├── ArchitectureResources.cs            # Embedded-source access
├── ProductionArchitecture.cs           # Reusable ArchUnitNET model
├── SourceParsing.cs                    # Roslyn parsing helpers
├── DependencyRuleTests.cs              # Assemblies, namespaces, references, public surface
├── DependencyPinConventionTests.cs     # Exact package versions, references, non-vacuity guards
├── ProjectConventionTests.cs           # Project property, package, and reference rules
├── SolutionConventionTests.cs          # SDK-style solution structure and order
├── SourceFileConventionTests.cs        # Files, namespaces, syntax, using directives
├── LayoutAndApiConventionTests.cs      # Namespace/folder mapping, layout, collections, contracts, coalescing
├── TypeDeclarationConventionTests.cs   # Contracts, colocation, XML documentation
├── TypeSurfaceConventionTests.cs       # Enumerations, interface docs, static state, suite self-checks
├── MemberOrganisationTests.cs          # Accessibility, field/member order, formatting
├── NamingConventionTests.cs            # Type, method, field, and fixture naming
├── FrameworkUsageConventionTests.cs    # MonoGame/NuciXNA wrappers, async, dummies, assertions, lifecycle ownership
└── TestSourceConventionTests.cs        # Fixture attributes, Given/When/Then names, reflection, case counts
```

### Enforced Convention Families

| Family | Examples of enforced rules |
|--------|--------------------------|
| Compiled dependencies | Only the production assembly is modelled; no persistence, web, serialisation, or test-framework dependencies; `GuiManager` must not depend on `Screens`; no `InternalsVisibleTo` |
| Public surface | Documented types and contracts exist; `Cursor` stays sealed and non-singleton; managers expose `Instance` |
| Projects and solution | `net10.0`, explicit `ImplicitUsings`/`LangVersion`/`Nullable`, alphabetical exact-version packages, test flags, SDK-style `.slnx` with documented project order |
| Source structure | One type per file matching its name, block namespaces matching folders, no nested types, no dedicated `.Interfaces`/`.Enums` namespaces |
| Syntax bans | `var`, ternaries, tuples, `init`, `partial`, `const`, `ref`/`out`/optional parameters, `??`, `== null`, `#nullable`, regions, `#pragma warning disable`, `global using`, ignored tests |
| Layout | Four-space indentation, single blank-line density, no blank line before closing delimiters, no `{ }` across lines, adjacency and parameter ordering of overloads, long parameter/argument lists wrapped at 96 characters |
| Naming | PascalCase types/methods, `I`-prefixed interfaces, camelCase private fields, uppercase public/protected members, permitted boolean forms, `EventArgs` suffix |
| Member organisation | Explicit accessibility, fields before members, static-readonly-before-instance field order, expression-bodied single-statement members and computed getters |
| Documentation | XML summaries on public package types, members, and interface contracts |
| Framework usage | `GuiImage`/`GuiText` wrappers instead of raw `Texture2D`/`SpriteFont`, no `async`/`CancellationToken`, `Point2D.Empty`/`Size2D.Zero`, controls driven only by managers |
| Test sources | `[TestFixture]` on fixtures, `Given..._When..._Then...` names, `SetUp` naming, no non-public reflection, at least two `[TestCase]` values, expression-bodied single-assert tests, internal top-level dummies |
| Suite integrity | Embedded sources and project files are present, the ArchUnitNET type provider is non-empty, and the required suite packages are declared |

## Coverage Map

| Area | Current Automated Evidence | Material Gaps |
|------|----------------------------|---------------|
| `GuiControl` lifecycle/properties | Strong unit coverage | Input subscriptions/polling, cycles, graphics draw order, partial failure, concurrency |
| Menu item/link/toggle/selector | Property and behavioural fixtures | End-to-end input manager/focus/transition integration |
| `Screen` | Base lifecycle/properties | Graphics device clear, input forwarding, global manager interaction/failures |
| `GuiManager` | Incidental only | Registry failures, reverse input order, handled reset, focus, draw filters |
| `ScreenManager` | None focused | Reflection, transitions, resize, failure recovery, ignored requests |
| `MenuScreen` | None focused | Empty lists, selectable skipping, layout, mouse/directional focus |
| `Cursor` | None | Modes, subscriptions, reload/unload, draw choice |
| `GuiText`, `GuiImage`, `GuiTooltip` | Mostly indirect or none | Composition, propagation, rendering, bounds/content failures |

## Test Design For Changes

- Derive cases from lifecycle states: fresh, loaded, unloaded, disposed, repeated operation, hook failure.
- Test local and inherited values, including parent disabled/hidden and explicit overrides.
- Test collection boundaries: zero, one, many, duplicate keys/IDs, first/last/wrap, current invalid index.
- Test ordering with event traces, not only final state.
- Test both direct input events and manager-polled click path.
- For transitions, use controllable game time/effect state and assert construction, old disposal, new load, overlay, and failure order.
- Keep graphics-dependent tests deterministic; separate pure state tests from host/device integration.

## Manual Verification

Until host-level automation exists, manually verify:
1. Fonts/content identifiers resolve on supported platforms.
2. Text/image/tooltip composition renders in expected order.
3. Mouse, keyboard, and gamepad focus/activation work with overlapping/nested controls.
4. Screen transitions resize, cover, swap, and recover as intended.
5. Cursor paired/custom modes render and unsubscribe correctly.

## CI Parity

CI installs Microsoft core fonts on Ubuntu before restore/build/test. A local Linux failure involving fonts should first be compared with [the workflow](../.github/workflows/dotnet.yml). The workflow currently runs only against `master` push/pull-request events and does not report coverage. [The security workflow](../.github/workflows/security.yml) additionally reports vulnerable NuGet packages and uploads CodeQL C# analysis to GitHub Code Scanning on pushes, pull requests, and a weekly schedule.
