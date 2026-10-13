# GuiMenuToggle

**Type:** `NuciXNA.Gui.Controls.GuiMenuToggle`
**Base:** `GuiMenuItem`
**Source:** [`NuciXNA.Gui/Controls/GuiMenuToggle.cs`](../../../NuciXNA.Gui/Controls/GuiMenuToggle.cs)

## Purpose

`GuiMenuToggle` maintains one boolean state, toggles it when the inherited item is triggered, and appends a fixed state label to its child text.

## Public Members

| Member | Type | Semantics |
|--------|------|-----------|
| `IsOn` | `bool` | Public getter/private setter; initially false |
| `StateChanged` | `EventHandler` | Raised only when state genuinely changes |
| `SwitchOn()` | Method | `SetState(true)` |
| `SwitchOff()` | Method | `SetState(false)` |
| `SwitchState()` | Method | Inverts current state |
| `SetState(bool state)` | Method | No-op when equal; otherwise assign and raise event |

All `GuiMenuItem` members/events are inherited.

## Lifecycle And Display

`DoLoadContent()` calls the base hook and then subscribes to `Triggered`. `DoUnloadContent()` calls the base hook and unsubscribes. `DoUpdate()` first executes base label/focus propagation, then sets the child text to:
- `Text + " : On"` when on.
- `Text + " : Off"` when off.

The suffixes are fixed English strings and are not configurable or localised.

## Failure And Edge Semantics

State change handlers execute synchronously and can throw through trigger/input delivery. Repeated `SwitchOn`, `SwitchOff`, or `SetState` with the current value raises no event. The base `Text` property itself remains unchanged; only the child display includes the suffix.

## Tests And Change Impact

[`GuiMenuToggleTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuToggleTests.cs) and [`GuiMenuToggleExtendedTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuToggleExtendedTests.cs) cover state and trigger behaviour. Localisation, child-label rendering, input integration, and subscriber failures are not covered.

## Related Documentation

- [GuiMenuItem](./GuiMenuItem.md)
- [State And Persistence](../../state-and-persistence.md)
