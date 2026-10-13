# Menu Navigation

## Visible Behaviour

`MenuScreen` centres its `Items` and lays them out vertically or horizontally. Selectable items can gain focus from pointer movement or direction input. Focus changes the item text colour to `SelectedTextColour` and activates its text fade effect.

## Direction Mapping

| Axis | Previous | Next |
|------|----------|------|
| Vertical | `W`, Up, D-pad Up, Left Thumbstick Up | `S`, Down, D-pad Down, Left Thumbstick Down |
| Horizontal | `A`, Left, D-pad Left, Left Thumbstick Left | `D`, Right, D-pad Right, Left Thumbstick Right |

Direction wraps at either end and skips `IsSelectable == false` items. `GuiMenuText` is always non-selectable.

## Pointer Selection

On mouse movement, the menu searches for the first selectable item whose `DisplayRectangle` contains the new location and stores its index. Pointer entry on the item itself also asks `GuiManager` to focus that root.

Selection index and focus are distinct. Mouse movement changes the index immediately; `DoUpdate` focuses only when index normalisation changes it, while the item's `MouseEntered` handler normally supplies immediate pointer focus.

## Activation

A focused `GuiMenuItem` raises `Triggered` from:
- Left click through inherited click handling.
- Enter or `E`.
- Gamepad A.

Specialised results:

| Item | Result |
|------|--------|
| `GuiMenuLink` | Requests `ScreenManager.ChangeScreens(TargetScreen, Parameters)` |
| `GuiMenuToggle` | Inverts `IsOn`, raises `StateChanged`, displays ` : On` or ` : Off` |
| `GuiMenuListSelector` | Inherited trigger itself has no selector action; left/right-specific input cycles values |
| `GuiMenuText` | Navigation skips it, but inherited trigger/input behaviour still exists if focused/clicked by another path |

## List Selector Navigation

While focused:
- Right/`D`, D-pad Right, or Left Thumbstick Right selects next.
- Left/`A`, D-pad Left, or Left Thumbstick Left selects previous.
- Pressed left mouse button inside selects next; right selects previous.
- Selection wraps for collections larger than one.

A horizontal menu uses the same keyboard/gamepad directions, so the screen can change focus and a selector can change value from one external event. This interaction is not covered by tests.

## Layout And Resize

Layout runs on load and every update using `ScreenManager.Size`, each item size, and `Spacing`. Outside transitions the screen manager refreshes size from the graphics back buffer. During transitions it returns before that refresh, so cached size remains unchanged until transition completion.

## Edge Behaviour

- Empty menu direction input can attempt `Items[-1]`.
- With no selectable items, normalisation returns a wrapped safe index rather than looping forever.
- Replacing `Items` while loaded does not register/load new roots automatically.
- Requests triggered during an active transition are ignored.
- Generated selector string keys are not user-visible unless the host exposes them.

## Verification

Menu-control state has unit tests, but `MenuScreen` navigation/layout and complete input interaction do not. See [Testing](../testing.md) and [Input Handling](../flows/input-handling.md).
