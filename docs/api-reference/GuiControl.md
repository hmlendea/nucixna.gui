# GuiControl

**Type:** `NuciXNA.Gui.Controls.GuiControl`
**Implements:** `IGuiControl`, `IComponent`, `IDisposable`
**Source:** [`NuciXNA.Gui/Controls/GuiControl.cs`](../../NuciXNA.Gui/Controls/GuiControl.cs)

## Purpose And Boundary

`GuiControl` is the retained control base. It owns local/effective presentation state, a private child list, lifecycle/event ordering, NuciXNA.Input subscriptions, focus/hover state, and template hooks. Only roots belong in `GuiManager`; descendants belong to their parent through protected registration.

## Public Properties

| Property | Type | Semantics |
|----------|------|-----------|
| `Id` | `string` | Initial GUID string; publicly replaceable |
| `Location` | `Point2D` | Local location; empty fallback |
| `ScreenLocation` | `Point2D` | Local location plus recursive parent location |
| `Size` | `Size2D` | Explicit value, then parent size, then empty |
| `ClientRectangle` | `Rectangle2D` | Local location plus effective size |
| `DisplayRectangle` | `Rectangle2D` | Screen location plus effective size |
| `Opacity` | `float` | Explicit/parent/`1.0f`; assignments clamped to `[0,1]` |
| `BackgroundColour` | `Colour` | Explicit/parent/manager default |
| `ForegroundColour` | `Colour` | Explicit/parent/manager default |
| `FontName` | `string` | Explicit/parent/manager default |
| `IsEnabled` | `bool` | Local flag gated by parent; changed through methods |
| `IsVisible` | `bool` | Local flag gated by parent; public changes through methods |
| `IsHovered` | `bool` | Updated by mouse-move transitions |
| `IsFocused` | `bool` | Updated by focus methods/manager |
| `IsContentLoaded` | `bool` | Lifecycle state |
| `IsDisposed` | `bool` | Disposal state |
| `Parent` | `GuiControl` | Publicly settable despite source TODO; assignment alone does not register a child |
| `Site` | `ISite` | `IComponent` site |
| `Container` | `IContainer` | `Site?.Container` |

## Public Events

| Category | Events |
|----------|--------|
| Property assignments | `ForegroundColourChanged`, `BackgroundColourChanged`, `OpacityChanged`, `FontNameChanged`, `LocationChanged`, `SizeChanged` |
| State methods | `Shown`, `Hidden`, `Enabled`, `Disabled`, `Focused`, `Unfocused` |
| Lifecycle | `Created`, `ContentLoading`, `ContentLoaded`, `ContentUnloading`, `ContentUnloaded`, `Updating`, `Updated`, `Drawing`, `Drawn`, `Disposing`, `Disposed` |
| Keyboard | `KeyHeldDown`, `KeyPressed`, `KeyReleased` |
| Gamepad | `GamepadButtonHeldDown`, `GamepadButtonPressed`, `GamepadButtonReleased` |
| Pointer | `Clicked`, `MouseButtonPressed`, `MouseEntered`, `MouseLeft`, `MouseMoved` |

There is no public parent-changed, generic enabled-changed, visible-changed, mouse-button-released, or child-collection event.

## Public Methods

| Method | Behaviour |
|--------|-----------|
| `LoadContent()` | Validate unloaded; raise event; subscribe input; call hook; load children; set loaded/not disposed; raise event |
| `UnloadContent()` | Validate loaded; raise event; clear parent; unsubscribe; call hook; unload and clear children; clear loaded; raise event |
| `Update(GameTime)` | Validate loaded; raise event; prune/fix child parents; call hook; update enabled children; raise event |
| `Draw(SpriteBatch)` | Validate loaded; raise event; call hook; draw enabled/visible children; raise event |
| `Dispose()` | Snapshot children; guarded unload/disposal; suppress finaliser; dispose snapshot children |
| `Enable()` / `Disable()` | Set local flag and raise corresponding event, even if value was already the same |
| `Show()` / `Hide()` | Set local flag and raise corresponding event |
| `Focus()` / `Unfocus()` | Set local flag and raise corresponding event |
| `HandleInput()` | Poll eligible left click, focus/raise `Clicked`, set handled flag, then recurse children until handled |
| `ToString()` | Full type name without a site; otherwise site name plus full type name |

## Protected Extension Surface

- Abstract hooks: `DoLoadContent`, `DoUnloadContent`, `DoUpdate`, `DoDraw`.
- `RegisterChild(IGuiControl)` and two `RegisterChildren` overloads append private children and set `Parent`.
- `GetService(Type)` delegates to `Site`.
- `DesignMode` reads `Site?.DesignMode`.

Create children in `DoLoadContent`; base loading traverses them after the hook. When overriding a concrete control hook, call its base hook if its internal child/event setup is required.

## Input Semantics

Loaded controls subscribe directly to input events. Keyboard/gamepad events require enabled, visible, and focused state. Pointer pressed/moved handlers require enabled/visible state and bounds as applicable. Separately, manager polling calls `HandleInput()` in reverse root order. See [Input Handling](../flows/input-handling.md).

## Failure And Edge Semantics

- Invalid lifecycle order raises `InvalidOperationException`.
- No cycle prevention exists for `Parent`; cycles cause recursive property evaluation.
- Public `Parent` assignment can create parent references absent from the owner's private child list.
- Constructor-raised `Created` cannot normally be observed by external subscribers.
- Mouse hover changes only when a mouse-move event crosses/reaches bounds; moving a control beneath a stationary pointer does not recompute it.
- Object-level disposal locking does not make update/draw/input thread-safe.

## Tests And Change Impact

[`GuiControlLifecycleTests.cs`](../../NuciXNA.Gui.UnitTests/Controls/GuiControlLifecycleTests.cs), [`GuiControlPropertyTests.cs`](../../NuciXNA.Gui.UnitTests/Controls/GuiControlPropertyTests.cs), and [`ControlTests.cs`](../../NuciXNA.Gui.UnitTests/Controls/ControlTests.cs) cover substantial lifecycle/property behaviour through `DummyControl`. Gaps include direct input events, reverse click arbitration, focus across roots/children, parent cycles, render order, and concurrent access.

## Related Documentation

- [Interfaces](./Interfaces.md)
- [GuiManager](./GuiManager.md)
- [Presentation Components](../components/presentation.md)
