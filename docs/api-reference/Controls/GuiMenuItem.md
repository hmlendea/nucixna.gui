# GuiMenuItem

**Type:** `NuciXNA.Gui.Controls.GuiMenuItem`
**Base:** `GuiControl`
**Source:** [`NuciXNA.Gui/Controls/GuiMenuItem.cs`](../../../NuciXNA.Gui/Controls/GuiMenuItem.cs)

## Purpose And Defaults

`GuiMenuItem` is both a usable menu item and the base for links, toggles, selectors, and labels. It composes a child `GuiText` and converts mouse, keyboard, and gamepad activation into one `Triggered` event.

Constructor defaults:

| Member | Value |
|--------|-------|
| `ForegroundColour` | `Colour.White` |
| `SelectedTextColour` | `Colour.Gold` |
| `IsSelectable` | `true` |
| `Size` | `512 x 48` |

## Public Members

| Member | Type | Semantics |
|--------|------|-----------|
| `Text` | `string` | Label source; raises `TextChanged` on assignment |
| `SelectedTextColour` | `Colour` | Focused label colour; raises change event |
| `IsSelectable` | `virtual bool` | MenuScreen navigation eligibility; raises change event |
| `Triggered` | `EventHandler` | Unified activation signal |

All base members/events remain public.

## Composition And Lifecycle

`DoLoadContent()` creates a child `GuiText` with ID suffix `_text`, `MenuFont`, active effects, and a `FadeEffect` with speed `2` and minimum multiplier `0.25f`. It registers the child, propagates properties, and registers private event handlers. When the child finishes loading, its fade effect is activated.

`DoUpdate()` propagates `Text` and `Size`. If focused, it activates effects and uses `SelectedTextColour`; otherwise it deactivates effects and uses normal `ForegroundColour`. `DoDraw()` is empty because the child renders.

Derived classes overriding load/unload/update hooks must call the base implementation to retain this child and event wiring.

## Activation Paths

- `Clicked` raises `Triggered`.
- Focused `KeyboardKeyPressed` with Enter or `E` raises `Triggered`.
- Focused `GamepadButtonPressed` with `Buttons.A` raises `Triggered`.
- `MouseEntered` calls `GuiManager.FocusControl(this)`.

`IsSelectable` affects MenuScreen navigation only; these activation handlers do not inspect it.

## Failure And Edge Semantics

- The source TODO proposes custom trigger event arguments; current `Triggered` carries only `EventArgs.Empty`.
- A source TODO notes unimplemented selection sound on pointer entry.
- Pointer focus works only when the item is a registered root because manager focus search is root-only.
- Trigger event subscribers run synchronously and exceptions propagate through input delivery.

## Tests And Change Impact

[`GuiMenuItemTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuItemTests.cs) and [`GuiMenuItemExtendedTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuItemExtendedTests.cs) cover properties and behaviour. Input-manager integration, focus arbitration, child visual propagation, fade activation, and non-selectable activation remain material gaps.

## Related Documentation

- [GuiControl](../GuiControl.md)
- [MenuScreen](../MenuScreen.md)
- [Input Handling](../../flows/input-handling.md)
