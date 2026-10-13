# GuiTooltip

**Type:** `NuciXNA.Gui.Controls.GuiTooltip`
**Base:** `GuiControl`
**Source:** [`NuciXNA.Gui/Controls/GuiTooltip.cs`](../../../NuciXNA.Gui/Controls/GuiTooltip.cs)

## Purpose And Defaults

`GuiTooltip` is a hidden-by-default composite of border image, inset background image, and text. It adjusts its own location to remain inside `ScreenManager.Size`.

Constructor defaults:

| Member | Value |
|--------|-------|
| `FontName` | `"MenuFont"` |
| `IsVisible` | false |
| `BorderSize` | `1` |

## Public Members

| Member | Type | Semantics |
|--------|------|-----------|
| `Text` | `string` | Raises `TextChanged` on assignment |
| `BorderSize` | `int` | Raises `BorderSizeChanged` on assignment |

All `GuiControl` members/events are inherited. Show/hide uses inherited methods.

## Composition And Layout

`DoLoadContent()` creates and registers, in order:
1. Border `GuiImage` using `ScreenManager/FillImage`, tiled.
2. Background `GuiImage` using the same asset/layout.
3. `GuiText` with margin `2` and transparent background.

Base child loading follows. Update calls `SetChildrenProperties()` before base child updates:
- If any display edge exceeds screen bounds, tooltip `Location` is shifted back inside.
- Border tint uses effective `ForegroundColour`.
- Background tint uses effective `BackgroundColour`.
- Background size is tooltip size minus twice the border size.
- Background local location is border local location plus border size.
- Text content is propagated.

`DoDraw()` is empty; child order renders border, background, then text.

## Failure And Edge Semantics

- Negative or excessive `BorderSize` is not validated and can produce invalid background size/location.
- Tooltip size larger than the screen cannot be made fully contained by simple axis shifts.
- Clamping depends on cached `ScreenManager.Size`, which is not refreshed during transitions.
- Location assignments raise `LocationChanged`, including repeated assignments if clamping logic computes the same value.
- Content/graphics failures propagate.

## Tests And Change Impact

No dedicated GuiTooltip tests exist. Add composition-order, border geometry, each screen edge, oversized tooltip, transition-size caching, visibility, and lifecycle tests before changing layout.

## Related Documentation

- [GuiControl](../GuiControl.md)
- [GuiImage](./GuiImage.md)
- [GuiText](./GuiText.md)
