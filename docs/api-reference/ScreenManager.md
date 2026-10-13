# ScreenManager

**Type:** `NuciXNA.Gui.Screens.ScreenManager`
**Source:** [`NuciXNA.Gui/Screens/ScreenManager.cs`](../../NuciXNA.Gui/Screens/ScreenManager.cs)

## Purpose And Boundary

`ScreenManager` owns private current and pending `IScreen` references plus a transition `TextureSprite`. It delegates control work through the current screen; it does not expose either screen, validate target types explicitly, provide a transition strategy, or own host `SpriteBatch.Begin`/`End`.

## Public Properties

| Property | Type | Semantics |
|----------|------|-----------|
| `Instance` | `ScreenManager` | Locked, lazily initialised static instance |
| `StartingScreenType` | `Type` | Reflected without arguments during `LoadContent()` |
| `Size` | `Size2D` | Cached back-buffer size; refreshed during non-transition updates |
| `SpriteBatch` | `SpriteBatch` | Public get/set property; production source does not read it |
| `Transitioning` | `bool` | True from accepted change request until opaque swap completes |

The constructor is public and initialises `Size` from `GraphicsManager.Instance.BackBufferSize`.

## Public Methods

| Signature | Behaviour |
|-----------|-----------|
| `void LoadContent()` | Constructs the starting screen, configures the fixed transition sprite, loads the screen, then loads the sprite |
| `void UnloadContent()` | Disposes the current screen and unloads the transition sprite |
| `void Update(GameTime gameTime)` | Updates the current screen first; then advances/swaps a transition or refreshes size/overlay scale |
| `void Draw(SpriteBatch spriteBatch)` | Draws current screen, then transition overlay when active |
| `void ChangeScreens<TScreen>() where TScreen : IScreen` | Requests a parameterless target |
| `void ChangeScreens<TScreen>(params object[] parameters)` | Requests a generic target with constructor arguments |
| `void ChangeScreens(Type screenType)` | Requests a target type without arguments |
| `void ChangeScreens(Type screenType, params object[] parameters)` | Constructs a pending target and activates transition; returns silently if already transitioning |

## Fixed Transition

`LoadContent()` creates a tiled `TextureSprite` using `ScreenManager/FillImage`, black tint, and a `FadeEffect` with speed `3`, current/minimum multiplier `0.0f`, and maximum `2.0f`.

At request time the pending screen is constructed but not loaded. While overlay opacity is below `1.0f`, only the old screen updates/draws. At opaque, the old screen is disposed, the pending screen is assigned and loaded, the overlay is deactivated, and `Transitioning` becomes false.

## Failure And Edge Semantics

- Null/invalid starting or target types fail through reflection/cast exceptions.
- Constructor mismatch or target-constructor failure occurs before transition activation.
- Requests during an active transition are dropped without an event/result.
- If old-screen disposal or pending-screen loading fails at the swap, no rollback or `finally` reset exists.
- `UnloadContent()` assumes current screen and transition sprite are initialised.
- A null current screen branch exists in `ChangeScreens`, but normal construction plus `LoadContent` supplies one.

## Tests And Change Impact

No focused ScreenManager or transition tests exist. Changes require coverage of reflection arguments, ignored concurrent requests, overlay threshold, old-screen disposal, pending-screen load timing, back-buffer resizing, and failure recovery. Transition changes also affect the global `GuiManager` registry because screen disposal unloads and clears it.

## Related Documentation

- [Screen](./Screen.md)
- [Screen Transitions](../flows/screen-transitions.md)
- [Startup and Rendering](../flows/startup-and-rendering.md)
