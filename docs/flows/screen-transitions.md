# Screen Transitions

This document describes the screen transition system in NuciXNA.Gui.

## Overview

`NuciXNA.Gui.Screens.ScreenManager` keeps the current screen active while a black overlay becomes opaque. At full opacity it disposes the old screen, loads the pending screen, and removes the overlay. It does not cross-fade two loaded screens.

## Transition Trigger

```csharp
ScreenManager.Instance.ChangeScreens(typeof(GameScreen));

ScreenManager.Instance.ChangeScreens<GameScreen>();

// Constructor arguments for the pending screen
ScreenManager.Instance.ChangeScreens(typeof(GameScreen), levelId);
```

`GuiMenuLink` invokes the non-generic overload with its `TargetScreen` and `Parameters` properties. A request made while `Transitioning` is true is ignored.

## Transition Implementation

### Fade Effect

The transition uses `NuciXNA.Graphics.SpriteEffects.FadeEffect` as the `OpacityEffect` of a `TextureSprite`:

```csharp
// Internal ScreenManager fields
private TextureSprite transitionSprite;
private IScreen currentScreen;
private IScreen newScreen;
private TextureSprite transitionSprite;
```

### Transition Sprite

- **Content file:** `ScreenManager/FillImage`.
- **Tint:** `Colour.Black`.
- **Layout:** `TextureLayout.Tile`.
- **Effect:** `FadeEffect` with `Speed = 3`, multiplier range `0.0f` to `2.0f`, and initial multiplier `0.0f`.
- **Scale:** Refreshed from `GraphicsManager.Instance.BackBufferSize` while no transition is active.
- **Configuration:** These values are hard-coded; there is no duration or transition-effect property.

### FadeEffect Behavior

When a change is accepted, the manager sets the multiplier to `0.0f`, sets `IsIncreasing = true`, activates the effect and sprite, and sets `Transitioning = true`. `Transition` waits until `transitionSprite.ClientOpacity >= 1.0f`; the repository does not implement a fade-in phase for the new screen.

## Transition Sequence

```mermaid
sequenceDiagram
    participant App
    participant SM as ScreenManager
    participant Old as Current Screen
    participant New as Next Screen
    participant GM as GuiManager
    participant FX as FadeEffect

    App->>SM: ChangeScreens(NewScreenType, parameters)
    SM->>New: Activator.CreateInstance(type, parameters)
    SM->>FX: multiplier = 0; increasing; activate
    SM->>SM: Transitioning = true

    loop Each Frame During Transition
        App->>SM: Update(gameTime)
        SM->>Old: Update(gameTime)
        SM->>FX: Update(gameTime)
        App->>SM: Draw(spriteBatch)
        SM->>Old: Draw(spriteBatch)
        SM->>FX: Draw black overlay
    end

    Old->>Old: Dispose()
    Old->>GM: Unload all roots and clear registry
    SM->>SM: currentScreen = newScreen
    SM->>New: LoadContent()
    New->>GM: Register and load new roots
    SM->>FX: IsActive = false
    SM->>SM: Transitioning = false
```

## Transition Phases

1. **Request:** Construct `newScreen`; do not load it.
2. **Cover:** Continue the old screen's update/draw while the black sprite opacity increases.
3. **Swap:** At opaque, dispose the old screen, assign `newScreen`, and synchronously load it.
4. **Reveal:** Deactivate the transition sprite immediately. There is no animated fade from black in this implementation.

## Transition Draw Order

During transition, `ScreenManager.Draw()` renders in this order:

```csharp
public void Draw(SpriteBatch spriteBatch)
{
    currentScreen.Draw(spriteBatch);

    if (Transitioning)
    {
        transitionSprite.Draw(spriteBatch);
    }
}
```

The active screen always draws first. During a transition, the black overlay draws second. The pending screen never draws before the swap.

## Configuration

### Custom Transition Content

The transition uses the fixed content path `ScreenManager/FillImage`. A consuming content project must make that asset resolvable by NuciXNA.Graphics. The sprite is tinted black and tiled, so source colour and full-screen dimensions are not public transition settings.

Changing the path, tint, effect speed, multiplier range, layout, or swap threshold requires a source change to [`ScreenManager.cs`](../../NuciXNA.Gui/Screens/ScreenManager.cs).

## Transition State

```csharp
public bool Transitioning { get; private set; }
```

### Checking Transition State

```csharp
if (ScreenManager.Instance.Transitioning)
{
    // The old screen remains current until the opaque swap point.
}
```

## Input During Transition

The old screen remains loaded and its normal `Update` executes before transition progress. Its `DoUpdate`, screen input subscriptions, and `GuiManager.Update` therefore remain active. The pending screen is unloaded and receives no input or update.

`Screen.Update` is non-virtual, so a screen cannot override it to suppress work. Transition-sensitive logic belongs in `DoUpdate` or in event handlers, guarded by `ScreenManager.Instance.Transitioning` when appropriate.

## Screen Lifecycle During Transition

### Old Screen
1. Was loaded before the request.
2. Continues to update and draw under the overlay.
3. At opaque, `Dispose()` invokes `UnloadContent()` because it is loaded.
4. Screen unload invokes its hook, then unloads and clears all globally registered roots.

### New Screen
1. Is constructed immediately by `Activator.CreateInstance(screenType, parameters)`.
2. Remains unloaded while the overlay becomes opaque.
3. Becomes `currentScreen` after the old screen is disposed.
4. `LoadContent()` then runs synchronously; its screen hook registers roots before `GuiManager.LoadContent()` loads them.
5. Receives update/draw calls on subsequent host iterations.

## Error Handling

### Transition Failures

| Failure Point | Behavior |
|---------------|----------|
| Pending constructor or cast throws in `ChangeScreens` | Exception propagates before transition activation; old screen remains current |
| Transition asset load throws in manager `LoadContent` | Exception propagates; manager has no rollback |
| Old screen disposal/unload throws at swap | Exception propagates before pending assignment/loading completes |
| Pending `LoadContent()` throws at swap | Exception propagates after old screen disposal and pending assignment; `Transitioning` remains true because cleanup is not in `finally` |

### Recovery

```csharp
try
{
    ScreenManager.Instance.ChangeScreens<GameScreen>();
}
catch (Exception ex)
{
    // Apply host-owned reporting and recovery policy.
}
```

The library has no logger, error screen, retry, fallback screen, or transition rollback.

## Custom Transitions

`ScreenManager` exposes no transition strategy interface or protected extension hook. Replacing the effect coherently requires either:
- a deliberate source change to the manager and focused transition tests; or
- a host-owned screen coordinator that does not rely on `ScreenManager.Instance`.

Changing this process must preserve screen/control lifecycle ordering or intentionally document a breaking change.

## Best Practices

1. Ensure target types implement `IScreen` and have a constructor matching the supplied parameters.
2. Treat a request during `Transitioning` as deliberately dropped by the current API.
3. Keep old-screen update logic valid until the opaque swap point.
4. Account for pending content loading occurring synchronously at the swap, not at request time.
5. Add tests before changing effect thresholds, disposal order, loading order, or request arbitration; no transition tests currently exist.

## Common Issues

| Issue | Cause | Solution |
|-------|-------|----------|
| Change request has no effect | `Transitioning` was already true | Queue the desired change in host code or wait until false |
| Reflection exception at request time | Target type or constructor does not match | Supply an `IScreen` implementation and matching constructor parameters |
| Missing transition graphic | `ScreenManager/FillImage` is not resolvable | Add the conventional asset through the host content pipeline |
| Old screen still reacts during cover | Current implementation keeps updating it | Guard old-screen hooks/handlers with `Transitioning` if host semantics require it |
| Manager remains transitioning after pending load failure | Swap has no rollback or `finally` recovery | Define and test a library recovery policy before relying on automatic recovery |