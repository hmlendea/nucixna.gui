# Presentation Components

This document describes control composition and rendering responsibilities. Exact public members are catalogued under [API Reference](../api-reference/INDEX.md).

## Control Hierarchy

```
GuiControl (abstract)
├── GuiText
├── GuiImage
├── GuiMenuItem
│   ├── GuiMenuText
│   ├── GuiMenuLink
│   ├── GuiMenuToggle
│   └── GuiMenuListSelector
└── GuiTooltip
```

## GuiControl (Abstract Base)

**Source:** [`NuciXNA.Gui.Controls.GuiControl`](../../NuciXNA.Gui/Controls/GuiControl.cs)

### Responsibilities
- Retained location, size, appearance, enabled/visible/focus state, and lifecycle state.
- Protected child registration and recursive child lifecycle.
- Effective property inheritance through `Parent`.
- Subscription to NuciXNA.Input while loaded.
- Template hooks for concrete loading, unloading, updating, and drawing.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Initially a GUID string; publicly replaceable and used as root dictionary key |
| `Parent` | `GuiControl` | Publicly settable parent; source contains a TODO to restrict it |
| `Location` | `Point2D` | Position relative to parent |
| `ScreenLocation` | `Point2D` | Recursive sum of local and ancestor locations |
| `Size` | `Size2D` | Explicit size, otherwise parent size, otherwise empty |
| `ClientRectangle` | `Rectangle2D` | Rectangle at local `Location` |
| `DisplayRectangle` | `Rectangle2D` | Rectangle at `ScreenLocation` |
| `Opacity` | `float` | Opacity [0.0, 1.0], inherits from parent |
| `BackgroundColour` | `Colour` | Explicit, parent, then manager default |
| `ForegroundColour` | `Colour` | Explicit, parent, then manager default |
| `FontName` | `string` | Explicit, parent, then manager default |
| `IsEnabled` | `bool` | Local state gated by parent state; changed via methods |
| `IsVisible` | `bool` | Local state gated by parent state; changed via methods |
| `IsHovered`, `IsFocused` | `bool` | Input state |
| `IsContentLoaded`, `IsDisposed` | `bool` | Lifecycle state |

### Key Events

| Event | Args | Description |
|-------|------|-------------|
| `ContentLoading` | `EventArgs` | Before content loads |
| `ContentLoaded` | `EventArgs` | After content loads |
| `ContentUnloading` | `EventArgs` | Before content unloads |
| `ContentUnloaded` | `EventArgs` | After content unloads |
| `Disposing` | `EventArgs` | Before disposal |
| `Disposed` | `EventArgs` | After disposal |
| Property change events | `PropertyChangedEventArgs` | Foreground/background, opacity, font, location, and size assignments |
| `Shown`, `Hidden`, `Enabled`, `Disabled` | `EventArgs` | Explicit state method completed |
| `Focused`, `Unfocused` | `EventArgs` | Explicit focus method completed |
| `Updating`, `Updated`, `Drawing`, `Drawn` | `EventArgs` | Lifecycle call brackets |
| `MouseEntered`, `MouseLeft`, `MouseMoved` | `MouseEventArgs` | Pointer movement filtered by bounds |
| `MouseButtonPressed` | `MouseButtonEventArgs` | Mouse button pressed |
| `Clicked` | `MouseButtonEventArgs` | Left click from direct or polled path |
| Key events | `KeyboardKeyEventArgs` | Held, pressed, and released while focused |
| Gamepad events | `GamepadButtonEventArgs` | Held, pressed, and released while focused |

### Lifecycle Methods

```csharp
public void LoadContent()           // Calls DoLoadContent(), fires ContentLoading/ContentLoaded
public void UnloadContent()         // Calls DoUnloadContent(), fires ContentUnloading/ContentUnloaded
public void Update(GameTime)        // Cleans children, calls DoUpdate(), updates enabled children
public void Draw(SpriteBatch)       // Calls DoDraw(), draws children
public void Dispose()               // Calls Dispose(true), fires Disposing/Disposed
protected abstract void DoLoadContent()
protected abstract void DoUnloadContent()
protected abstract void DoUpdate(GameTime)
protected abstract void DoDraw(SpriteBatch)
public void Enable() / Disable()
public void Show() / Hide()
public void Focus() / Unfocus()
public void HandleInput()
```

### Property Inheritance

Properties inherit from parent if not explicitly set:
- `Opacity` -> parent opacity -> `1.0f`.
- `BackgroundColour` -> parent -> `GuiManager.DefaultBackgroundColour`.
- `ForegroundColour` -> parent -> `GuiManager.DefaultForegroundColour`.
- `FontName` -> parent -> `GuiManager.DefaultFontName`.
- Unset `Size` -> parent size -> `Size2D.Empty`.
- Parent disabled/hidden state forces a child effectively disabled/hidden without changing its local flag.

---

## GuiText

**Source:** [`NuciXNA.Gui.Controls.GuiText`](../../NuciXNA.Gui/Controls/GuiText.cs)

### Purpose
Composes a `TextSprite` with a child `GuiImage` that uses `ScreenManager/FillImage` for the background.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `Text` | `string` | Display text |
| `OutlineColour` | `Colour` | Text outline colour; defaults to black |
| `FontOutline` | `FontOutline` | Outline style |
| `HorizontalAlignment` | `Alignment` | Defaults to `Alignment.Middle` |
| `VerticalAlignment` | `Alignment` | Defaults to `Alignment.Middle` |
| `Margins` | `int` | Passed into background/text size calculations; defaults to `0` |
| `FadeEffect` | `FadeEffect` | Assigned to the text sprite's opacity effect |
| `AreEffectsActive` | `bool` | Activates text/background effects |

### Rendering
- `DoDraw` draws the `TextSprite`; normal child traversal then draws the background `GuiImage` because it is registered as a child.
- Every update propagates text, font, outline, colour, opacity, alignment, location, size, and effect state.
- A non-transparent background therefore draws after the text under the current parent-before-children draw contract.

---

## GuiImage

**Source:** [`NuciXNA.Gui.Controls.GuiImage`](../../NuciXNA.Gui/Controls/GuiImage.cs)

### Purpose
Wraps `NuciXNA.Graphics.Drawing.TextureSprite` and propagates content, mask, source rectangle, layout, tint, opacity, rotation, sampler state, and sprite effects.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `ContentFile` | `string` | NuciXNA.Graphics content identifier; empty when unset |
| `MaskFile` | `string` | Alpha-mask content identifier; empty when unset |
| `SourceRectangle` | `Rectangle2D` | Source region; initial empty value becomes loaded sprite size |
| `TextureLayout` | `TextureLayout` | Explicit value or `GuiManager.DefaultTextureLayout` |
| `TintColour` | `Colour` | Colour tint (default White) |
| Effect properties | NuciXNA.Graphics effects | Opacity, rotation, sprite sheet, movement, and scale effects |
| `Rotation` | `float` | Base sprite rotation |
| `SamplerState` | `SamplerState` | Optional override; graphics default when null |
| `AreEffectsActive` | `bool` | Sets `TextureSprite.IsActive` |

### Rendering
- Loading creates and loads one `TextureSprite`; unloading unloads it.
- If control `Size` is empty, propagation assigns `SourceRectangle.Size`.
- A non-empty source rectangle determines a scale that maps it to `Size`.
- Update and draw refresh the sprite's absolute `ScreenLocation`.

---

## GuiMenuItem

**Source:** [`NuciXNA.Gui.Controls.GuiMenuItem`](../../NuciXNA.Gui/Controls/GuiMenuItem.cs)

### Purpose
Concrete interactive menu item and base for specialised menu controls. It composes a child `GuiText` with `MenuFont` and a fade effect.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `Text` | `string` | Label propagated to the child text control |
| `SelectedTextColour` | `Colour` | Focused label colour; defaults to gold |
| `IsSelectable` | `bool` | Used by `MenuScreen` navigation; defaults to true |

### Events

| Event | Args | Description |
|-------|------|-------------|
| Property change events | `PropertyChangedEventArgs` | `Text`, `SelectedTextColour`, and `IsSelectable` assignments |
| `Triggered` | `EventArgs` | Mouse `Clicked`, focused Enter/E, or focused gamepad A |

### Behavior
- Focus, not a separate selection property, controls selected colour and fade activation.
- Pointer entry calls `GuiManager.FocusControl(this)`.
- `IsSelectable` affects `MenuScreen` directional/mouse navigation, but base click handling itself does not check it.

---

## GuiMenuText

**Source:** [`NuciXNA.Gui.Controls.GuiMenuText`](../../NuciXNA.Gui/Controls/GuiMenuText.cs)

### Purpose
Label-style menu item whose overridden `IsSelectable` getter always returns false.

### Key Properties
- It inherits `GuiMenuItem` activation/input behaviour; only menu navigation selection is disabled.

---

## GuiMenuLink

**Source:** [`NuciXNA.Gui.Controls.GuiMenuLink`](../../NuciXNA.Gui/Controls/GuiMenuLink.cs)

### Purpose
Menu item that navigates to a target screen when activated.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `TargetScreen` | `Type` | Type passed to `ScreenManager.ChangeScreens` |
| `Parameters` | `object[]` | Constructor arguments passed to the target screen |

### Behavior
- While loaded, its `Triggered` handler calls `ScreenManager.Instance.ChangeScreens(TargetScreen, Parameters)`.
- It performs no validation itself; reflection and cast failures propagate from `ScreenManager`.

---

## GuiMenuToggle

**Source:** [`NuciXNA.Gui.Controls.GuiMenuToggle`](../../NuciXNA.Gui/Controls/GuiMenuToggle.cs)

### Purpose
Boolean toggle menu item with on/off states.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `IsOn` | `bool` | Toggle state |
| `SwitchOn()` | Method | Calls `SetState(true)` |
| `SwitchOff()` | Method | Calls `SetState(false)` |
| `SwitchState()` | Method | Inverts state |
| `SetState(bool)` | Method | Changes state only when different |

### Events

| Event | Args | Description |
|-------|------|-------------|
| `StateChanged` | `EventArgs` | Toggle state changed |

### Behavior
- `Triggered` calls `SwitchState()`.
- Update appends the fixed text ` : On` or ` : Off` to the child label.
- A genuine state change raises `StateChanged`; assigning the existing state is a no-op.

---

## GuiMenuListSelector

**Source:** [`NuciXNA.Gui.Controls.GuiMenuListSelector`](../../NuciXNA.Gui/Controls/GuiMenuListSelector.cs)

### Purpose
Cyclic list selector with multiple options.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `Keys`, `Values` | `IEnumerable<string>` | Current dictionary views |
| `SelectedIndex` | `int` | `-1` when empty; otherwise selected insertion-order index |
| `SelectedKey`, `SelectedValue` | `string` | Current pair; null before selection |
| `ItemsCount` | `int` | Current count |
| Selection methods | Methods | Select by index, key, or value; next/previous; tolerant `TrySelectItem` overloads |
| `SetItems` overloads | Methods | Copy strings, pairs, enumerables, or dictionary into internal storage |
| `GetItems()` | Method | Returns a defensive dictionary copy |

### Events

| Event | Args | Description |
|-------|------|-------------|
| `ItemsChanged` | `EventArgs` | Item collection copy replaced |
| `SelectedItemChanged` | `ListSelectionEventArgs` instance via `EventHandler` | Selection changed |

### Behavior
- Non-empty `SetItems` raises `ItemsChanged`, resets selection, then selects index `0` and raises `SelectedItemChanged`.
- Next/previous wraps; a one-item collection is a no-op. On an empty collection, next throws for index `0`, while previous can return early because `-1` equals the current index.
- String-only inputs receive generated GUID keys.
- Focused left/right keyboard and gamepad directions cycle; pressed left/right mouse buttons inside cycle forwards/backwards.
- Update renders `Text + " : " + SelectedValue` when items exist.

---

## GuiTooltip

**Source:** [`NuciXNA.Gui.Controls.GuiTooltip`](../../NuciXNA.Gui/Controls/GuiTooltip.cs)

### Purpose
Contextual tooltip with border, background, and automatic screen-boundary clamping.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `Text` | `string` | Tooltip text |
| `BorderSize` | `int` | Border inset; defaults to `1` |

### Behavior
- Starts hidden and uses `MenuFont`.
- Composes a border image, inset background image, and text child.
- Every update adjusts `Location` to keep `DisplayRectangle` inside `ScreenManager.Size`.
- Child colours derive from tooltip foreground/background; the text child uses transparent background and margin `2`.

---

## Cursor

**Source:** [`NuciXNA.Gui.Cursor`](../../NuciXNA.Gui/Cursor.cs)

### Purpose
Host-owned cursor wrapper. It is neither a control nor a singleton.

### Key Properties

| Property | Type | Description |
|----------|------|-------------|
| `Location` | `Point2D` | Updated from external mouse-move events |
| `LocationOffset` | `Point2D` | Added to sprite location |
| `State` | `ButtonState` | Released fallback when unset |
| `Frames` | `int` | Public metadata; defaults to `1` and is not consumed internally |
| `SpriteSize` | `Size2D` | Optional sprite size override |
| `Scale` | `Scale2D` | Defaults to one |
| `ContentFile` | `string` | Custom single sprite; empty uses paired conventional sprites |

### Lifecycle
- `LoadContent()` loads one custom sprite or the conventional pair and subscribes input events.
- `Update(GameTime)` propagates properties and updates loaded sprite wrappers.
- `Draw(SpriteBatch)` draws the click sprite only when paired mode is active and `State` is pressed.
- `UnloadContent()` unloads sprite wrappers and unsubscribes matching events.

### Content Paths
- Paired mode uses `Cursors/idle` and `Cursors/click`.
- A non-empty `ContentFile` at load selects one sprite. Changing mode while loaded reloads only the idle sprite; existing click sprite/event subscriptions are not reconciled.

---

## Rendering Pipeline

All controls render through `GuiManager.Draw(SpriteBatch)`:

```csharp
public void Draw(SpriteBatch spriteBatch)
{
    foreach (GuiControl control in guiControls.Values)
    {
        if (control.IsContentLoaded && control.IsVisible)
        {
            control.Draw(spriteBatch);
        }
    }
}
```

### Draw Order
1. `Screen.Draw` clears, invokes the screen hook, then invokes `GuiManager.Draw`.
2. The manager enumerates its root dictionary; it exposes no public z-order contract.
3. `GuiControl.Draw` invokes `DoDraw` before enabled, visible children in child-list order.
4. `ScreenManager` draws the transition overlay after the current screen while transitioning.
5. Host-owned cursors or overlays draw wherever the host places them.

### Batching
- The host owns `SpriteBatch.Begin` and `End` plus sort, blend, sampler, depth, rasteriser, and effect settings.
- The library performs no cross-control draw sorting or batching policy.
- `GuiImage.SamplerState` can override the graphics wrapper's default for an image.

---

## Input Handling

Input flows through `GuiManager.Update()`:

```csharp
public void Update(GameTime gameTime)
{
    RemoveDisposedControls();
    // Snapshot roots; poll clicks in reverse; reset handled flag;
    // update loaded and enabled roots forwards.
}
```

### Hit Testing
- `ScreenLocation` recursively adds ancestor locations; `DisplayRectangle` combines it with effective size.
- Loaded controls also subscribe directly to input-manager events.
- `InputManager.MouseButtonInputHandled` arbitrates manager polling but does not suppress every direct event subscriber.

### Focus/Selection
- `GuiManager.FocusControl` operates on registered roots only.
- `MenuScreen.SelectedItemIndex` is separate navigation state; it focuses the selected root item.
- `GuiMenuItem` uses `IsFocused` for selected text colour/effects.
- See [Input Handling](../flows/input-handling.md) for both input paths.

---

## Styling System

### Default Values (GuiManager.Instance)

| Property | Default |
|----------|---------|
| `DefaultBackgroundColour` | `Colour.Transparent` |
| `DefaultForegroundColour` | `Colour.Black` |
| `DefaultFontName` | `"DefaultFont"` |
| `DefaultTextureLayout` | `TextureLayout.Stretch` |
| `DefaultMargins` | `8`; currently not consumed by a production control |

### Inheritance Chain
```
GuiManager colour/font defaults
    -> Root control when unset
        -> Child effective value when unset
            -> Descendant effective value when unset
```

Opacity instead falls back to `1.0f`; size falls back to the parent then empty; texture layout is a `GuiImage`/manager rule rather than a base-control inherited property.

### Override Precedence
1. Explicit backing value on the control.
2. Parent's effective value when that property supports inheritance.
3. Applicable manager or hard-coded fallback.

---

## Extensibility

### Creating Custom Controls
1. Inherit from `GuiControl` (or `GuiMenuItem` for interactive)
2. Implement abstract `Do*` methods
3. In `DoLoadContent`, create descendants and add them through protected `RegisterChild`/`RegisterChildren`; the base loader loads children afterwards.
4. Register only roots with `GuiManager.Instance.RegisterControl` or `RegisterControls` before `Screen.LoadContent` reaches `GuiManager.LoadContent`.
5. If extending a concrete control, call its base hook when relying on its child creation or event registration.
6. Add lifecycle, inheritance, input, and disposal tests proportionate to the new behaviour.

### Example Minimal Control
```csharp
public class CustomControl : GuiControl
{
    protected override void DoLoadContent() { }
    protected override void DoUnloadContent() { }
    protected override void DoUpdate(GameTime gameTime) { }
    protected override void DoDraw(SpriteBatch spriteBatch) { }
}
```