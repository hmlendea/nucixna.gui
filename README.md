[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucixna.gui)](https://github.com/hmlendea/nucixna.gui/releases/latest) [![Build Status](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml) [![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# NuciXNA.Gui

GUI management and lightweight widgets for the NuciXNA stack on top of MonoGame/XNA.

`NuciXNA.Gui` provides:

- a central GUI manager (`GuiManager`) for registering, updating and drawing controls
- reusable controls such as text, image, menu entries, selectors and toggles
- screen abstractions (`Screen`, `MenuScreen`) and a transition-enabled `ScreenManager`
- default styling values (font, colors, texture layout, margins) inherited by controls

## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciXNA.Gui)

### .NET CLI

```bash
dotnet add package NuciXNA.Gui
```

### Package Manager

```powershell
Install-Package NuciXNA.Gui
```

## Requirements

- .NET target framework: `net10.0`
- MonoGame DesktopGL (or compatible runtime)
- NuciXNA dependencies (restored automatically from NuGet):
  - `NuciXNA.Graphics`
  - `NuciXNA.Input`
  - `NuciXNA.Primitives`

## Quick Start

The typical flow is:

1. Configure your starting screen type in `ScreenManager`
2. Call `LoadContent()` once
3. Forward your game loop `Update(...)` and `Draw(...)` to the manager
4. Build screens by deriving from `Screen` or `MenuScreen`
5. Register controls through `GuiManager.Instance.RegisterControls(...)`

Example screen with a basic label:

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

Game-level integration example:

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

## Available Controls

- `GuiText`: text rendering with alignment, margins and optional fade effect
- `GuiImage`: textured control with tint, source rectangle and sprite effects
- `GuiMenuItem`: base interactive menu element
- `GuiMenuText`: non-selectable menu text
- `GuiMenuLink`: menu item with action semantics
- `GuiMenuToggle`: on/off menu item with state change events
- `GuiMenuListSelector`: cycles through predefined values
- `GuiTooltip`: helper control for tooltip-like text

## Screen System

- `Screen`: base class handling lifecycle (`LoadContent`, `Update`, `Draw`, `UnloadContent`) and input event wiring
- `MenuScreen`: helper for menu-based screens with auto layout and keyboard/mouse selection
- `ScreenManager`: singleton manager that tracks current screen and supports transition effects when switching

To switch screens at runtime:

```csharp
ScreenManager.Instance.ChangeScreens<MainMenuScreen>();
```

or with parameters:

```csharp
ScreenManager.Instance.ChangeScreens(typeof(GameplayScreen), levelId);
```

## Defaults and Styling

Global defaults are configured on `GuiManager.Instance`:

- `DefaultBackgroundColour`
- `DefaultForegroundColour`
- `DefaultFontName`
- `DefaultTextureLayout`
- `DefaultMargins`

Controls inherit values from parents first, then fall back to these global defaults.

## Development

### Build

```bash
dotnet build NuciXNA.Gui.sln
```

### Test

```bash
dotnet test NuciXNA.Gui.sln
```

### Pack

```bash
dotnet pack NuciXNA.Gui/NuciXNA.Gui.csproj -c Release
```

## Contributing

Contributions are welcome.

Please:

- keep changes cross-platform
- preserve public APIs unless the change is intentionally breaking
- keep pull requests focused and consistent with existing style
- update documentation when behaviour changes
- add or update tests for new behaviour

## Related Projects

- [NuciXNA.DataAccess](https://github.com/hmlendea/nucixna.dataaccess)
- [NuciXNA.Graphics](https://github.com/hmlendea/nucixna.graphics)
- [NuciXNA.GUI](https://github.com/hmlendea/nucixna.gui)
- [NuciXNA.Input](https://github.com/hmlendea/nucixna.input)
- [NuciXNA.Primitives](https://github.com/hmlendea/nucixna.Primitives)

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.