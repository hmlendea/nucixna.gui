# Interfaces

## IGuiControl

**Type:** `NuciXNA.Gui.Controls.IGuiControl`
**Source:** [`NuciXNA.Gui/Controls/IGuiControl.cs`](../../NuciXNA.Gui/Controls/IGuiControl.cs)

`IGuiControl` declares the complete public control contract used by `GuiControl`:
- Identity/layout: `Id`, `Location`, `ScreenLocation`, `Size`, `ClientRectangle`, `DisplayRectangle`.
- Appearance/state: `Opacity`, colours, font, enabled/visible/hovered/focused/content-loaded/disposed state, and concrete `GuiControl Parent`.
- Component model: `Site` and `Container`.
- Property, state, lifecycle, keyboard, gamepad, and pointer events listed in [GuiControl](./GuiControl.md).
- Lifecycle/state/input methods: load, unload, update, draw, dispose, enable/disable, show/hide, focus/unfocus, `ToString`, and `HandleInput`.

The interface does not inherit `IDisposable`, although it declares `void Dispose()`. Its concrete `Parent` type couples alternative implementations to `GuiControl`, and `GuiControl.RegisterChild` casts `IGuiControl` back to `GuiControl`; a non-`GuiControl` implementation cannot be registered as a child without `InvalidCastException`.

## IScreen

**Type:** `NuciXNA.Gui.Screens.IScreen`
**Source:** [`NuciXNA.Gui/Screens/IScreen.cs`](../../NuciXNA.Gui/Screens/IScreen.cs)

`IScreen : IDisposable` declares:
- `Id`, `BackgroundColour`, `ForegroundColour`, `IsContentLoaded`, and `IsDisposed`.
- Foreground/background property-change events.
- Created, content, update, draw, and disposal lifecycle events.
- `KeyPressed`, `MouseButtonPressed`, `MouseMoved`, and `GamepadButtonPressed` forwarding events.
- `LoadContent()`, `UnloadContent()`, `Update(GameTime)`, and `Draw(SpriteBatch)`.

`ScreenManager` stores current/pending values as `IScreen`, but `ChangeScreens(Type, ...)` casts the reflected object to concrete `Screen`, not merely `IScreen`. Therefore a custom implementation that implements `IScreen` without inheriting `Screen` satisfies the generic constraint yet fails in that non-generic implementation path at runtime.

## Compatibility And Change Impact

These public interfaces expose MonoGame, NuciXNA.Input, and NuciXNA.Primitives types. Changes to signatures, event delegates, concrete `Parent`, or lifecycle state are source/binary compatibility changes. Interface additions are also breaking for third-party implementers.

The production implementation and tests predominantly use `GuiControl`/`Screen` inheritance rather than independent interface implementations. No contract tests exercise non-base implementations.

## Related Documentation

- [GuiControl](./GuiControl.md)
- [Screen](./Screen.md)
- [ScreenManager](./ScreenManager.md)
