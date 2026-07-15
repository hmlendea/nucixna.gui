[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucixna.gui)](https://github.com/hmlendea/nucixna.gui/releases/latest)
[![Build Status](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# NuciXNA.Gui

GUI management and lightweight widgets for the NuciXNA stack on top of MonoGame/XNA.

## ✨ Features

- Central `GuiManager` singleton for registering, updating, and drawing controls
- Reusable controls: `GuiText`, `GuiImage`, `GuiMenuItem`, `GuiMenuText`, `GuiMenuLink`, `GuiMenuToggle`, `GuiMenuListSelector`, `GuiTooltip`
- Screen abstractions (`Screen`, `MenuScreen`) with full lifecycle management
- Transition-enabled `ScreenManager` for screen switching
- Parent-inherited default styling: font, colours, opacity, and margins

## 🚀 Usage

### Setting up the screen manager

Configure the starting screen in your `Game` subclass:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Gui.Screens;

public class GameRoot : Game
{
    SpriteBatch spriteBatch;

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        ScreenManager.Instance.StartingScreenType = typeof(MainMenuScreen);
        ScreenManager.Instance.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        ScreenManager.Instance.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        ScreenManager.Instance.Draw(spriteBatch);
        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        ScreenManager.Instance.UnloadContent();
        base.UnloadContent();
    }
}
```

### Implementing a screen

Derive from `Screen` or `MenuScreen` and register controls via `GuiManager`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

public class MainMenuScreen : Screen
{
    GuiText title;

    protected override void DoLoadContent()
    {
        title = new GuiText
        {
            Id = "title",
            Text = "My Game",
            Location = new Point2D(32, 24),
            Size = new Size2D(480, 64)
        };

        GuiManager.Instance.RegisterControls(title);
    }

    protected override void DoUnloadContent() { }

    protected override void DoUpdate(GameTime gameTime) { }

    protected override void DoDraw(SpriteBatch spriteBatch) { }
}
```

### Switching screens

```csharp
ScreenManager.Instance.ChangeScreens<MainMenuScreen>();
```

Or with parameters:

```csharp
ScreenManager.Instance.ChangeScreens(typeof(GameplayScreen), levelId);
```

### Global defaults

Configure shared defaults on `GuiManager.Instance` before loading content:

```csharp
GuiManager.Instance.DefaultFontName = "Fonts/MyFont";
GuiManager.Instance.DefaultForegroundColour = Colour.White;
GuiManager.Instance.DefaultBackgroundColour = Colour.Transparent;
```

Controls inherit values from their parent first, then fall back to these global defaults.

## 📦 Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciXNA.Gui)

### .NET CLI

```bash
dotnet add package NuciXNA.Gui
```

### Package Manager Console

```powershell
Install-Package NuciXNA.Gui
```

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

All NuGet dependencies are restored automatically by `dotnet restore`.

### Build

```bash
dotnet build NuciXNA.sln
```

### Test

```bash
dotnet test NuciXNA.sln
```

### Release

```bash
dotnet pack NuciXNA.Gui/NuciXNA.Gui.csproj -c Release
```

### Dependencies

| Package | Purpose |
|---------|---------|
| `MonoGame.Framework.DesktopGL` | XNA/MonoGame rendering and input runtime |
| `NuciXNA.Graphics` | Sprite rendering utilities |
| `NuciXNA.Input` | Input event abstractions |
| `NuciXNA.Primitives` | Common value types (`Point2D`, `Size2D`, `Colour`) |

## 🗂️ Project Structure

The solution contains the following projects:

- `NuciXNA.Gui`: Main library with controls, screens, and the GUI manager
- `NuciXNA.Gui.UnitTests`: Unit tests for the library

The key directories inside `NuciXNA.Gui/` are:

| Directory | Purpose |
|-----------|---------|
| `Controls/` | `GuiControl` base class and all concrete control implementations |
| `Screens/` | `Screen`, `MenuScreen`, and `ScreenManager` |

## 🤝 Contributing

Contributions are welcome. Please:
- Keep the changes cross-platform
- Keep the existing public contract intact unless a breaking change is intentional
- Keep the pull requests focused and consistent with the existing code style
- Update the documentation when behaviour changes
- Properly test all changes, including edge cases and error conditions
- Add unit tests for any new or changed functionality

## 🔗 Related Projects

- [NuciXNA.DataAccess](https://github.com/hmlendea/nucixna.dataaccess): Data access utilities for the NuciXNA stack
- [NuciXNA.Graphics](https://github.com/hmlendea/nucixna.graphics): Sprite rendering utilities
- [NuciXNA.Input](https://github.com/hmlendea/nucixna.input): Input event abstractions
- [NuciXNA.Primitives](https://github.com/hmlendea/nucixna.primitives): Common value types

## 💝 Support

Found a bug or have a suggestion? [Open an issue](https://github.com/hmlendea/nucixna.gui/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or giving a ⭐️ on GitHub!

## 📄 License

Licensed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for details.