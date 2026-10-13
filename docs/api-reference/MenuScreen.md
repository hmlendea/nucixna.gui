# MenuScreen

**Type:** `NuciXNA.Gui.Screens.MenuScreen`
**Base:** `NuciXNA.Gui.Screens.Screen`
**Source:** [`NuciXNA.Gui/Screens/MenuScreen.cs`](../../NuciXNA.Gui/Screens/MenuScreen.cs)

## Purpose

`MenuScreen` registers a collection of root `GuiMenuItem` controls, centres and spaces them along one axis, tracks a selected index, and uses screen-level mouse/keyboard/gamepad events to move root focus.

## Public Members

| Member | Type | Default Or Behaviour |
|--------|------|----------------------|
| `MenuScreen()` | Constructor | New GUID ID, selected index `0`, vertical axis, spacing `32`, empty items list |
| `Axis` | `MenuScreenAxis` | `Vertical`; `Horizontal` is the other enum value |
| `Spacing` | `int` | `32` |
| `Items` | `List<GuiMenuItem>` | Publicly replaceable list; initially empty |
| `SelectedItemIndex` | `int` | Public getter/private setter; initially `0` |

All `Screen` members/events are inherited.

## Lifecycle Hooks

- `DoLoadContent()` registers all current `Items` with `GuiManager`, aligns them, and subscribes private handlers to the screen's `MouseMoved`, `KeyPressed`, and `GamepadButtonPressed` events.
- Base `Screen.LoadContent()` subsequently calls `GuiManager.LoadContent()`, which loads the registered items.
- `DoUpdate()` realigns every item, normalises `SelectedItemIndex`, and focuses the newly normalised item only when the index changed.
- `DoUnloadContent()` removes menu handlers; base screen unload then unloads/clears roots.
- `DoDraw()` is empty; controls render through `GuiManager`.

## Layout

`AlignMenuItems()` sums item sizes plus a spacing-sized `Size2D`, derives a starting point from `(ScreenManager.Size - dimensions) / 2`, then:
- Vertical axis: centres each item horizontally and advances both dimensions by item size plus `Spacing / 2`.
- Horizontal axis: centres each item vertically and uses the same dimension advance.

Layout is recalculated on load and every update. Item `Location` change events can therefore fire repeatedly even when assigned coordinates are unchanged.

## Navigation

| Axis | Back | Forward |
|------|------|---------|
| Vertical | `W`, Up, D-pad Up, Left Thumbstick Up | `S`, Down, D-pad Down, Left Thumbstick Down |
| Horizontal | `A`, Left, D-pad Left, Left Thumbstick Left | `D`, Right, D-pad Right, Left Thumbstick Right |

Navigation wraps at list ends and skips items whose `IsSelectable` is false. Mouse movement sets the selected index to a selectable item whose `DisplayRectangle` contains the pointer; focus changes on the subsequent update only if normalisation changes the index, while directional handlers focus immediately.

## Failure And Edge Semantics

- `DoLoadContent()` forwards duplicate/null item-ID errors from manager registration.
- Direction input with an empty list can normalise to `-1` and index `Items[-1]` when focusing.
- If all items are non-selectable, normalisation returns a safe wrapped index without searching indefinitely.
- Replacing `Items` while loaded does not register/load the new list automatically; update only aligns and navigates the current list.
- Horizontal menu direction keys overlap list-selector left/right keys; screen and focused-control event subscribers can both react to one external event.

## Tests And Change Impact

No focused MenuScreen tests exist. Add coverage for empty/all-non-selectable lists, wrapping direction, mouse selection/focus timing, axis layout, resize realignment, list replacement, and selector key interaction before changing navigation/layout.

## Related Documentation

- [Screen](./Screen.md)
- [GuiMenuItem](./Controls/GuiMenuItem.md)
- [Input Handling](../flows/input-handling.md)
