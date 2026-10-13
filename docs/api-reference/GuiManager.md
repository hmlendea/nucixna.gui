# GuiManager

**Type:** `NuciXNA.Gui.GuiManager`
**Source:** [`NuciXNA.Gui/GuiManager.cs`](../../NuciXNA.Gui/GuiManager.cs)

## Purpose And Boundary

`GuiManager` stores root controls by ID, coordinates their lifecycle/update/draw calls, arbitrates polled mouse clicks and root focus, and supplies global defaults. It does not expose its registry, own child controls directly, manage screens, update NuciXNA.Input, or start/end `SpriteBatch`.

## Construction And Defaults

`Instance` is lazily initialised under `System.Threading.Lock`. The constructor is public, so additional instances are possible, but production controls and screens use the static `Instance`.

| Property | Type | Initial Value | Consumer |
|----------|------|---------------|----------|
| `Instance` | `GuiManager` | Lazy singleton | Internal and host callers |
| `DefaultBackgroundColour` | `Colour` | `Colour.Transparent` | `GuiControl`, `Screen` fallback |
| `DefaultForegroundColour` | `Colour` | `Colour.Black` | `GuiControl`, `Screen` fallback |
| `DefaultFontName` | `string` | `"DefaultFont"` | `GuiControl` fallback |
| `DefaultTextureLayout` | `TextureLayout` | `TextureLayout.Stretch` | `GuiImage` fallback |
| `DefaultMargins` | `int` | `8` | No production consumer currently |

## Public Methods

| Signature | Behaviour |
|-----------|-----------|
| `GuiManager()` | Creates an empty private control dictionary and initialises defaults |
| `void LoadContent()` | Calls `LoadContent()` on every registered root |
| `virtual void UnloadContent()` | Calls `UnloadContent()` on every root, then clears the dictionary; does not explicitly dispose roots |
| `virtual void Update(GameTime gameTime)` | Removes disposed roots, polls input in reverse snapshot order, resets the handled flag, then updates loaded/enabled roots forwards |
| `virtual void Draw(SpriteBatch spriteBatch)` | Draws roots that are loaded and visible; root enabled state is not checked |
| `void RegisterControl(GuiControl control)` | Adds one root with `Dictionary.Add(control.Id, control)` |
| `void RegisterControls(params GuiControl[] controls)` | Delegates to enumerable registration |
| `void RegisterControls(IEnumerable<GuiControl> controls)` | Adds each root by ID |
| `void FocusControl(GuiControl control)` | Delegates using `control.Id` |
| `void FocusControl(string id)` | Unfocuses focused roots, then focuses a root whose ID matches |

## Update Order

1. Collect IDs of roots whose `IsDisposed` is true, then remove them.
2. Snapshot `guiControls.Values`.
3. Traverse the snapshot from last to first.
4. Stop if `InputManager.Instance.MouseButtonInputHandled` is true; otherwise call `HandleInput()` on enabled roots.
5. Reset `MouseButtonInputHandled` to false.
6. Traverse forwards and call `Update(gameTime)` on loaded, enabled roots.

The dictionary enumeration used for the snapshot is not exposed as a public ordering contract.

## Failure And Edge Semantics

- Null control or null ID fails through dereference/dictionary exceptions.
- Duplicate ID raises the exception from `Dictionary.Add`.
- `LoadContent()` or `UnloadContent()` can partially process roots before one root throws; no rollback exists.
- `FocusControl(GuiControl)` dereferences a null argument.
- Focus search is root-only; descendants are not indexed.
- Concurrent registry/lifecycle access is unsupported despite locked singleton construction.

## Tests And Change Impact

No focused manager tests exist. Changes should add coverage for duplicate IDs, disposed-root removal, reverse click traversal, handled-flag reset, root-only focus, loaded/enabled/visible filtering, and partial lifecycle failure. Input order changes affect every control; unload changes affect screen transitions and shutdown.

## Related Documentation

- [GuiControl](./GuiControl.md)
- [Screen](./Screen.md)
- [Input Handling](../flows/input-handling.md)
- [Concurrency and Scheduling](../concurrency-and-scheduling.md)
