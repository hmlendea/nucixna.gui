# Cursor

**Type:** `NuciXNA.Gui.Cursor`
**Source:** [`NuciXNA.Gui/Cursor.cs`](../../NuciXNA.Gui/Cursor.cs)

## Purpose And Ownership

`Cursor` is a sealed, host-constructed cursor wrapper over one or two `TextureSprite` instances. It is not a `GuiControl`, is not registered with `GuiManager`, and has no singleton. The host owns lifecycle and draw order.

## Public Properties

| Property | Type | Default And Semantics |
|----------|------|-----------------------|
| `Location` | `Point2D` | Updated by external mouse-move events; publicly settable |
| `LocationOffset` | `Point2D` | Added to sprite location |
| `State` | `ButtonState` | Returns `Released` when backing value is null |
| `Frames` | `int` | `1`; not consumed internally |
| `SpriteSize` | `Size2D` | Empty; non-empty values override sprite size |
| `Scale` | `Scale2D` | `Scale2D.One` |
| `ContentFile` | `string` | Empty/null selects paired mode; non-empty selects one custom sprite |

Changing `ContentFile` to a different value while loaded immediately unloads the idle sprite, creates/loads a new one, and reapplies properties. The click sprite from a prior paired load is not recreated in custom mode.

## Public Methods

| Method | Behaviour |
|--------|-----------|
| `Cursor()` | Sets `Frames = 1` and unit scale |
| `LoadContent()` | Loads paired or custom sprite(s), sets properties, and subscribes mouse events |
| `UnloadContent()` | Marks unloaded, unloads sprite(s), and unsubscribes the events appropriate to current `ContentFile` mode |
| `Update(GameTime)` | Propagates location/size/scale and updates sprite(s) |
| `Draw(SpriteBatch)` | Draws click sprite when present and `State == Pressed`; otherwise idle sprite |

## Content Modes

| Mode | Assets | Input Subscriptions |
|------|--------|---------------------|
| Paired (`ContentFile` empty) | `Cursors/idle`, `Cursors/click` | Mouse moved, button pressed, button released |
| Custom (`ContentFile` non-empty) | The supplied identifier | Mouse moved only |

Only left-button events alter `State`. Mouse movement assigns `Location = e.Location`.

## Failure And Edge Semantics

- The class has no explicit loaded-state guards. Calling update/draw/unload before successful load dereferences uninitialised sprites.
- Calling `LoadContent()` repeatedly can duplicate subscriptions and replace sprite references without first unloading previous content.
- Changing `ContentFile` while loaded changes mode-specific assets but does not adjust button-event subscriptions established by the original load. For example, switching from paired to custom leaves paired button subscriptions until unload logic evaluates the new value and may not remove them.
- `UnloadContent()` chooses button unsubscription based on the current `ContentFile`, not the mode originally loaded.
- Graphics/content exceptions propagate.

## Tests And Change Impact

No cursor tests exist. Changes need mode-switch, repeated-load, unload, offset/scale/size propagation, left-button state, non-left-button behaviour, and draw-choice coverage. Subscription-mode bugs can leak the cursor through the global input manager.

## Related Documentation

- [Host And Composition](../components/host-and-composition.md)
- [Input Handling](../flows/input-handling.md)
- [Configuration](../configuration.md)
