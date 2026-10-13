# GuiText

**Type:** `NuciXNA.Gui.Controls.GuiText`
**Base:** `GuiControl`
**Source:** [`NuciXNA.Gui/Controls/GuiText.cs`](../../../NuciXNA.Gui/Controls/GuiText.cs)

## Purpose And Composition

`GuiText` owns a `TextSprite` and registers one child `GuiImage` background. It propagates effective control state into both on each update.

## Public Members

| Member | Type | Semantics |
|--------|------|-----------|
| `Text` | `string` | Text sprite content; raises `TextChanged` on every assignment |
| `OutlineColour` | `Colour` | Defaults to black; raises `OutlineColourChanged` |
| `FontOutline` | `FontOutline` | Raises `FontOutlineChanged` |
| `HorizontalAlignment` | `Alignment` | Defaults to middle; raises change event |
| `VerticalAlignment` | `Alignment` | Defaults to middle; raises change event |
| `Margins` | `int` | Defaults to `0`; raises `MarginsChanged` |
| `FadeEffect` | `FadeEffect` | Assigned to `TextSprite.OpacityEffect` at load |
| `AreEffectsActive` | `bool` | Controls text/background effect activity |

All base members/events are inherited.

## Lifecycle And Rendering

`DoLoadContent()` creates a tiled background `GuiImage` using `ScreenManager/FillImage`, creates a `TextSprite`, registers the background child, propagates properties, and loads the text sprite. Base `GuiControl.LoadContent()` then loads the child image.

`DoUpdate()` propagates properties and updates the text sprite. `DoDraw()` draws the text sprite. Base child traversal then draws the background image after the text. A non-transparent background can therefore cover the text under the current draw order.

Property propagation uses:
- Background size: `Size + Margins * 2` in both dimensions.
- Text location: `ScreenLocation + (Margins, Margins)`.
- Text sprite size: `Size - Margins * 2`.
- Effective `FontName`, colours, opacity, alignments, outline, text, and effect state.

No validation prevents negative resulting text dimensions or negative margins.

## Failure And Edge Semantics

- Null `FadeEffect` is accepted and assigned to the sprite.
- Content/font/background failures propagate from NuciXNA.Graphics.
- Repeated property assignments still raise change events.
- Changing `FadeEffect` after load does not reassign `TextSprite.OpacityEffect`; only other propagated fields refresh.

## Tests And Change Impact

No dedicated `GuiText` test file exists; behaviour is exercised indirectly by menu-control tests. Add focused tests for alignment/margins, background draw order, effect replacement, negative dimensions, and lifecycle cleanup before changing composition.

## Related Documentation

- [GuiControl](../GuiControl.md)
- [GuiImage](./GuiImage.md)
- [Presentation Components](../../components/presentation.md)
