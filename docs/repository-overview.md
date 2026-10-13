# Repository Overview

## Purpose

NuciXNA.Gui is a .NET class library that provides GUI management and lightweight widgets for the NuciXNA stack on top of MonoGame/XNA. It enables game developers to create interactive user interfaces (menus, buttons, text, images, tooltips) that integrate with the MonoGame game loop.

## Scope

The library provides:
- A singleton `GuiManager` for registering, updating, and drawing GUI controls
- Reusable controls: `GuiText`, `GuiImage`, `GuiMenuItem`, `GuiMenuText`, `GuiMenuLink`, `GuiMenuToggle`, `GuiMenuListSelector`, `GuiTooltip`
- Screen abstractions (`Screen`, `MenuScreen`) that drive the global control lifecycle.
- A transition-enabled `ScreenManager` that constructs pending screens and swaps them beneath an opaque fade overlay.
- Parent-inherited colours, font, opacity, size, enabled state, and visibility.
- A host-owned `Cursor` with conventional idle/click assets or one custom sprite.

The library does **not** provide:
- Network connectivity or external service integrations
- Persistent storage or database access
- Audio playback or sound effects
- Game logic beyond menu navigation and screen transitions.
- Persistence, logging, localisation, HTTP APIs, or deployment infrastructure.

## Entry Points

### Primary Entry Points

1. **`NuciXNA.Gui.Screens.ScreenManager.Instance`** - Host entry point for active-screen lifecycle and transitions.
2. **`NuciXNA.Gui.Screens.Screen`** - Base class whose lifecycle delegates global root-control work to `GuiManager`.
3. **`NuciXNA.Gui.GuiManager.Instance`** - Root-control registry, input arbitration, update, and draw coordinator.
4. **`NuciXNA.Gui.Controls.GuiControl`** - Base class for retained control state, child ownership, input events, and lifecycle hooks.
5. **`NuciXNA.Gui.Screens.MenuScreen`** - Menu layout and axis-aware focus navigation.
6. **`NuciXNA.Gui.Cursor`** - Independently constructed cursor lifecycle.

### Consuming Application Integration

The consuming application integrates via:
- Setting `ScreenManager.Instance.StartingScreenType` to specify the initial screen
- Calling `ScreenManager.Instance.LoadContent()` in `Game.LoadContent()`
- Calling `ScreenManager.Instance.Update(GameTime)` in `Game.Update()`
- Calling `ScreenManager.Instance.Draw(SpriteBatch)` in `Game.Draw()`
- Calling `ScreenManager.Instance.UnloadContent()` in `Game.UnloadContent()`

## Repository Structure

```
NuciXNA.Gui/
├── Controls/
│   ├── GuiControl.cs          # Abstract base class for all controls
│   ├── IGuiControl.cs         # Control interface
│   ├── GuiText.cs             # Text display control
│   ├── GuiImage.cs            # Image display control
│   ├── GuiMenuItem.cs         # Base menu item control
│   ├── GuiMenuText.cs         # Text menu item
│   ├── GuiMenuLink.cs         # Screen navigation link
│   ├── GuiMenuToggle.cs       # Toggle menu item
│   ├── GuiMenuListSelector.cs # List selector menu item
│   ├── GuiTooltip.cs          # Tooltip control
│   └── ListSelectionEventArgs.cs
├── Screens/
│   ├── IScreen.cs             # Screen interface
│   ├── Screen.cs              # Abstract base class for screens
│   ├── MenuScreen.cs          # Menu screen implementation
│   ├── MenuScreenAxis.cs      # Enum for menu orientation
│   └── ScreenManager.cs       # Screen lifecycle and transitions
├── Cursor.cs                  # Custom cursor implementation
├── GuiManager.cs              # Singleton control manager
└── NuciXNA.Gui.csproj

NuciXNA.Gui.UnitTests/
├── Controls/                  # Unit tests for controls
├── Screens/                   # Unit tests for screens
├── Helpers/                   # Test helpers (DummyControl, DummyScreen)
└── NuciXNA.Gui.UnitTests.csproj
```

## Target Framework

- .NET 10.0 (`net10.0`)
- MonoGame.Framework.DesktopGL 3.8.4
- NuciXNA.Graphics (sprite rendering utilities)
- NuciXNA.Input (input event abstractions)
- NuciXNA.Primitives (common value types: Point2D, Size2D, Colour)

## License

GPL-3.0-or-later. See [LICENSE](../LICENSE).