# Screen

**Type:** `NuciXNA.Gui.Screens.Screen`
**Implements:** `NuciXNA.Gui.Screens.IScreen`, `System.IDisposable`
**Source:** [`NuciXNA.Gui/Screens/Screen.cs`](../../NuciXNA.Gui/Screens/Screen.cs)

## Purpose And Boundary

`Screen` is the template lifecycle for a host screen. It captures a clear colour, subscribes screen-level input, invokes derived hooks, and drives the global `GuiManager`. It does not own a public control collection; derived screens register roots during `DoLoadContent()`.

## Public Properties

| Property | Type | Semantics |
|----------|------|-----------|
| `Id` | `string` | Initial GUID string; publicly replaceable |
| `BackgroundColour` | `Colour` | Explicit value or `GuiManager.DefaultBackgroundColour`; constructor sets black explicitly |
| `ForegroundColour` | `Colour` | Explicit value or `GuiManager.DefaultForegroundColour` |
| `IsContentLoaded` | `bool` | True after successful load until unload |
| `IsDisposed` | `bool` | True after successful explicit/finaliser disposal |

## Public Events

| Category | Events |
|----------|--------|
| Properties | `ForegroundColourChanged`, `BackgroundColourChanged` |
| Construction/lifecycle | `Created`, `ContentLoading`, `ContentLoaded`, `ContentUnloading`, `ContentUnloaded`, `Updating`, `Updated`, `Drawing`, `Drawn`, `Disposing`, `Disposed` |
| Input forwarding | `KeyPressed`, `MouseButtonPressed`, `MouseMoved`, `GamepadButtonPressed` |

`Created` is raised inside the constructor, before an ordinary external subscriber can attach.

## Lifecycle Methods

| Method | Ordered Behaviour |
|--------|-------------------|
| `LoadContent()` | Reject if loaded; raise `ContentLoading`; capture XNA clear colour; subscribe input; call `DoLoadContent`; call `GuiManager.LoadContent`; set loaded; raise `ContentLoaded` |
| `UnloadContent()` | Reject if not loaded; raise `ContentUnloading`; unsubscribe input; call `DoUnloadContent`; call `GuiManager.UnloadContent`; clear loaded; raise `ContentUnloaded` |
| `Update(GameTime)` | Require loaded; raise `Updating`; call `DoUpdate`; call `GuiManager.Update`; raise `Updated` |
| `Draw(SpriteBatch)` | Require loaded; raise `Drawing`; clear graphics device; call `DoDraw`; call `GuiManager.Draw`; raise `Drawn` |
| `Dispose()` | If not disposed, raise `Disposing`, unload if loaded, set disposed, raise `Disposed`, suppress finalisation |

The finaliser calls public `Dispose()`.

## Protected Extension Hooks

Derived screens must implement:

```csharp
protected abstract void DoLoadContent();
protected abstract void DoUnloadContent();
protected abstract void DoUpdate(GameTime gameTime);
protected abstract void DoDraw(SpriteBatch spriteBatch);
```

Register all root controls in `DoLoadContent`; the base method loads them afterwards. Remove only screen-owned event subscriptions/resources in `DoUnloadContent`; the base method then unloads and clears all manager roots.

## Failure And Edge Semantics

- Double load, unload-before-load, update-before-load, and draw-before-load raise `InvalidOperationException`.
- Hook, input, manager, graphics, and content exceptions propagate; lifecycle flags can remain in their previous state after partial failure.
- Background clear colour is captured at load. Changing `BackgroundColour` while loaded does not update the cached XNA clear colour until a later load.
- `Dispose()` is idempotent through `IsDisposed` and an object lock.

## Tests And Change Impact

[`ScreenTests.cs`](../../NuciXNA.Gui.UnitTests/Screens/ScreenTests.cs) and [`ScreenExtendedTests.cs`](../../NuciXNA.Gui.UnitTests/Screens/ScreenExtendedTests.cs) exercise base properties/lifecycle with `DummyScreen`. There are no render-device, input forwarding, manager interaction, failure rollback, or finaliser tests.

## Related Documentation

- [Interfaces](./Interfaces.md)
- [ScreenManager](./ScreenManager.md)
- [GuiManager](./GuiManager.md)
