# GuiImage

**Type:** `NuciXNA.Gui.Controls.GuiImage`
**Base:** `GuiControl`
**Source:** [`NuciXNA.Gui/Controls/GuiImage.cs`](../../../NuciXNA.Gui/Controls/GuiImage.cs)

## Purpose

`GuiImage` adapts retained control properties to one `NuciXNA.Graphics.Drawing.TextureSprite`.

## Public Properties

| Property | Type | Default And Semantics |
|----------|------|-----------------------|
| `ContentFile` | `string` | Empty when unset; raises `ContentFileChanged` |
| `MaskFile` | `string` | Empty when unset; raises `MaskFileChanged` |
| `TintColour` | `Colour` | White when unset; raises `TintColourChanged` |
| `SourceRectangle` | `Rectangle2D` | Empty when unset; raises `SourceRectangleChanged` |
| `TextureLayout` | `TextureLayout` | Explicit value or `GuiManager.DefaultTextureLayout`; raises change event |
| `OpacityEffect` | `OpacityEffect` | Propagated to sprite |
| `RotationEffect` | `RotationEffect` | Propagated to sprite |
| `SpriteSheetEffect` | `SpriteSheetEffect` | Propagated to sprite |
| `MovementEffect` | `MovementEffect` | Propagated to sprite |
| `ScaleEffect` | `ScaleEffect` | Propagated to sprite |
| `Rotation` | `float` | Propagated to sprite |
| `SamplerState` | `SamplerState` | Optional override; sprite/graphics default when null |
| `AreEffectsActive` | `bool` | Propagated to `TextureSprite.IsActive` |

All `GuiControl` properties/events are inherited.

## Lifecycle And Property Propagation

`DoLoadContent()` creates a `TextureSprite`, propagates current values, loads it, and if `SourceRectangle` is empty assigns a rectangle matching the loaded sprite size.

`DoUpdate()` propagates all values, updates the sprite, then refreshes absolute location. `DoDraw()` refreshes location/tint and draws. `DoUnloadContent()` unloads the sprite.

When control `Size == Size2D.Empty`, propagation assigns `SourceRectangle.Size`. When the sprite source rectangle is non-empty, scale is calculated as control width/source width and control height/source height.

## Failure And Edge Semantics

- Content and mask identifiers are not validated locally; dependency exceptions propagate.
- Effect objects and sampler state can be null.
- Assigning any property with a change event raises it even when the value is equal.
- Zero source dimensions can cause invalid scale division if represented as non-empty by the primitive type.
- The class does not expose the owned `TextureSprite` or loaded texture.

## Tests And Change Impact

No dedicated GuiImage tests exist. Changes need coverage for empty-size adoption, source scaling, content/mask propagation, sampler/effect propagation, absolute location, lifecycle, and graphics failures.

## Related Documentation

- [GuiControl](../GuiControl.md)
- [GuiText](./GuiText.md)
- [Dependencies](../../dependencies.md)
