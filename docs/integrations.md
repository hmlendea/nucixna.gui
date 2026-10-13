# Integrations

## Integration Summary

| Integration | Direction | Data/Commands | Owner | Failure Behaviour |
|-------------|-----------|---------------|-------|-------------------|
| MonoGame host | Host -> library | `GameTime`, `SpriteBatch`, lifecycle cadence | Consuming application | Invalid graphics/thread/batch state propagates |
| NuciXNA.Graphics | Library -> package | Content identifiers, geometry, colours, sprites/effects | Graphics package and host content setup | Content/device/effect exceptions propagate |
| NuciXNA.Input | Package -> library and library queries | Input events, mouse location/click, handled flag | Input package; host advances lifecycle | Event/query failures propagate; no provider fallback |
| NuciXNA.Primitives | Shared types | Points, sizes, rectangles, colours, scale | Primitive package | Type/semantic compatibility required |
| Host screen types | Library reflection -> host code | `Type` and constructor arguments | Consuming application | Reflection/cast/constructor exceptions propagate |
| Host event handlers | Library -> host callbacks | Synchronous control/screen events | Consuming application | Subscriber exceptions propagate |

There are no remote services, HTTP endpoints, message brokers, databases, browser APIs, or operating-system service integrations.

## MonoGame Host Contract

The host:
- Creates `SpriteBatch` and owns `Begin`/`End` plus render-state settings.
- Calls parameterless load/unload methods and supplies `GameTime`/`SpriteBatch` for update/draw.
- Invokes calls on a thread compatible with MonoGame graphics operations.
- Chooses update/draw cadence and cursor layering.
- Provides a `GraphicsManager`/graphics-device environment through the NuciXNA stack.

`Screen.Draw` clears the graphics device itself using the background colour captured during screen load.

## NuciXNA.Graphics Contract

### Used Types

- `GraphicsManager`: back-buffer size and graphics device.
- `TextureSprite`: images, cursor sprites, transition overlay.
- `TextSprite`: rendered text.
- `TextureLayout`: image/overlay layout.
- `FadeEffect` and other sprite effects.

### Conventional Content Identifiers

| Identifier | Consumer |
|------------|----------|
| `ScreenManager/FillImage` | Transition overlay, `GuiText` background, `GuiTooltip` border/background |
| `Cursors/idle` | Cursor paired mode |
| `Cursors/click` | Cursor paired mode |
| `DefaultFont` | `GuiManager` default font |
| `MenuFont` | Menu item and tooltip text |

Custom `GuiImage.ContentFile`, `MaskFile`, `Cursor.ContentFile`, and effective `FontName` values add host-defined identifiers.

## NuciXNA.Input Contract

Loaded screens/controls/cursors subscribe directly to `InputManager.Instance`. `GuiManager.Update` additionally reads `MouseLocation`, `MouseButtonInputHandled`, and `IsLeftMouseButtonClicked()`.

The repository does not initialise or advance the input manager. The host must follow NuciXNA.Input's lifecycle and ensure events/current state are coherent before NuciXNA.Gui work.

The shared handled flag crosses package boundaries. Changing its reset or event semantics can alter overlapping-control click behaviour.

## Reflection Contract

`ScreenManager` constructs screens through `Activator.CreateInstance`:
- Starting screen: no constructor arguments.
- Changed screen: supplied `object[]` arguments.
- Reflected result is cast to concrete `Screen` in the transition path even though public generic constraints mention `IScreen`.

Constructor signatures and side effects therefore form a runtime integration contract. Pending constructors run at request time; `LoadContent()` runs later at the opaque swap.

## Event Callback Contract

All events are synchronous. Subscriber mutations affect subsequent code/subscribers in the same call. Subscriber exceptions abort the originating input/lifecycle operation. The library provides no dispatcher, isolation, queue, correlation context, or error boundary.

## Adding An Integration

Adding a service/provider requires updates to:
- Project manifest and dependency/security review.
- Architecture and dependency direction.
- Configuration and secret handling.
- Error translation/retry/degradation semantics.
- Logging/privacy/data retention.
- Focused unit/integration tests.
- Root [Security Policy](../SECURITY.md) and [Privacy Policy](../PRIVACY.md) when data or trust boundaries change.
