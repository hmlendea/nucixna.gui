# Testing

## Test Stack

[`NuciXNA.Gui.UnitTests`](../NuciXNA.Gui.UnitTests/) targets `net10.0` and uses:
- NUnit `4.6.1`.
- NUnit3TestAdapter `6.2.0`.
- Microsoft.NET.Test.Sdk `18.7.0`.
- Direct reference to the library project.
- `DummyControl` and `DummyScreen` test doubles.

## Current Validation

On 2026-10-13, `dotnet test NuciXNA.GUI.slnx --configuration Release --no-build --verbosity minimal` discovered and passed 409 of 409 NUnit tests with no failures or skips. This is a point-in-time result, not a permanent coverage guarantee.

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

CI installs Microsoft core fonts on Ubuntu before restore/build/test. A local Linux failure involving fonts should first be compared with [the workflow](../.github/workflows/dotnet.yml). The workflow currently runs only against `master` push/pull-request events and does not report coverage.
