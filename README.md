[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucixna.gui)](https://github.com/hmlendea/nucixna.gui/releases/latest)
[![Build Status](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/NuciXNA.Gui)](https://nuget.org/packages/NuciXNA.Gui)
[![License](https://img.shields.io/github/license/hmlendea/nucixna.gui)](https://github.com/hmlendea/nucixna.gui/blob/master/LICENSE)

# NuciXNA.Gui

GUI management and lightweight widgets for the NuciXNA stack on top of MonoGame/XNA.

## 📑 Table of Contents

- [Capabilities](#capabilities)
- [Usage](#usage)
    - [Setting Up The Screen Manager](#setting-up-the-screen-manager)
    - [Implementing A Screen](#implementing-a-screen)
    - [Switching Screens](#switching-screens)
    - [Global Defaults](#global-defaults)
    - [Examples](#examples)
- [Known Limitations](#known-limitations)
- [Installation](#installation)
    - [Package Manager Installation](#package-manager-installation)
- [Configuration](#configuration)
    - [Settings](#settings)
- [Compatibility](#compatibility)
- [Integrations](#integrations)
- [Extensibility](#extensibility)
- [Privacy and Data](#privacy-and-data)
- [Development](#development)
    - [Requirements](#requirements)
    - [Setup](#setup)
    - [Build](#build)
    - [Test](#test)
    - [Release](#release)
    - [Dependencies](#dependencies)
- [GitHub Actions](#github-actions)
- [Project Structure](#project-structure)
    - [Projects and Packages](#projects-and-packages)
    - [Directories](#directories)
- [Architecture](#architecture)
- [Documentation](#documentation)
- [API Reference](#api-reference)
- [Troubleshooting](#troubleshooting)
- [FAQ](#faq)
- [Contributing](#contributing)
- [Related Projects](#related-projects)
- [Security](#security)
- [Project Engagement](#project-engagement)
- [License](#license)

## ✨ Capabilities

- Central `GuiManager` singleton for registering, updating, and drawing controls
- Reusable controls: `GuiText`, `GuiImage`, `GuiMenuItem`, `GuiMenuText`, `GuiMenuLink`, `GuiMenuToggle`, `GuiMenuListSelector`, `GuiTooltip`
- Screen abstractions (`Screen`, `MenuScreen`) with full lifecycle management
- Transition-enabled `ScreenManager` for screen switching
- Parent-inherited default styling: font, colours, opacity, and margins

## 🚀 Usage

### Setting Up The Screen Manager

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
        spriteBatch.Begin();
        ScreenManager.Instance.Draw(spriteBatch);
        spriteBatch.End();
        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        ScreenManager.Instance.UnloadContent();
        base.UnloadContent();
    }
}
```

### Implementing A Screen

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

### Switching Screens

```csharp
ScreenManager.Instance.ChangeScreens<MainMenuScreen>();
```

Or with parameters:

```csharp
ScreenManager.Instance.ChangeScreens(typeof(GameplayScreen), levelId);
```

### Global Defaults

Configure shared defaults on `GuiManager.Instance` before loading content:

```csharp
GuiManager.Instance.DefaultFontName = "Fonts/MyFont";
GuiManager.Instance.DefaultForegroundColour = Colour.White;
GuiManager.Instance.DefaultBackgroundColour = Colour.Transparent;
```

Controls inherit values from their parent first, then fall back to these global defaults.

### Examples

See [API Usage Examples](./docs/api-usage-examples.md) for toggles, selectors, parameterised screens, composite controls, cursor modes, and focus handling.

## ⚠️ Known Limitations

- The public API is tightly coupled to MonoGame and NuciXNA.Graphics/Input/Primitives types.
- Managers use global static instances internally; alternate instances are not coherent dependency substitutes.
- Rendering/input/transition integration lacks a complete automated host test suite.
- The library provides no persistence, logging, networking, localisation framework, accessibility layer, or configurable transition strategy.

## 📦 Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciXNA.Gui)

### Package Manager Installation

```bash
dotnet add package NuciXNA.Gui
```

Or, via the `Package Manager Console`:

```powershell
Install-Package NuciXNA.Gui
```

## ⚙️ Configuration

Configuration is programmatic. Set manager defaults and `ScreenManager.StartingScreenType` before loading the screen manager; configure controls and host-owned cursors through their public properties. See [Configuration](./docs/configuration.md) for precedence and content requirements.

### Settings

The subsequent settings are recognised:
| Section | Key | Type | Default | Required | Description |
|---------|-----|------|---------|----------|-------------|
| GuiManager | `DefaultBackgroundColour` | `Colour` | `Colour.Transparent` | No | Root control/screen background fallback |
| GuiManager | `DefaultForegroundColour` | `Colour` | `Colour.Black` | No | Root control/screen foreground fallback |
| GuiManager | `DefaultFontName` | `string` | `"DefaultFont"` | No | Root control font fallback |
| GuiManager | `DefaultTextureLayout` | `TextureLayout` | `TextureLayout.Stretch` | No | GuiImage layout fallback |
| ScreenManager | `StartingScreenType` | `Type` | `null` | Yes before load | Parameterless starting `Screen` type |
| Cursor | `ContentFile` | `string` | `null` | No | Custom single sprite; empty uses conventional pair |

## 🧩 Compatibility

| Component | Supported Versions | Notes |
|-----------|--------------------|-------|
| .NET | `net10.0` | Sole target framework |
| MonoGame.Framework.DesktopGL | `3.8.4` | Declared runtime package version |
| NuciXNA.Graphics | `2.2.4` | Declared runtime package version |
| NuciXNA.Input | `2.1.1` | Declared runtime package version |
| NuciXNA.Primitives | `2.1.7` | Declared runtime package version |

## 🔌 Integrations

| Integration | Compatibility | Purpose | Required |
|-------------|---------------|---------|----------|
| MonoGame DesktopGL | `3.8.4` | Host game loop, timing, and sprite batching | Yes |
| NuciXNA.Graphics | `2.2.4` | Graphics manager, sprites, layouts, and effects | Yes |
| NuciXNA.Input | `2.1.1` | Input events/state and click arbitration | Yes |
| NuciXNA.Primitives | `2.1.7` | Geometry, colour, and scale value types | Yes |

## 🧱 Extensibility

Create custom controls by inheriting `GuiControl` and implementing its four lifecycle hooks. Create screens by inheriting `Screen`, or menu screens by inheriting `MenuScreen`. Register roots through `GuiManager`; register descendants through protected child registration.

| Extension Point | Contract | Purpose |
|-----------------|----------|---------|
| Custom control | `GuiControl` hooks and lifecycle invariants | Additional retained UI components |
| Custom screen | `Screen` hooks | Host-specific screen content and logic |
| Menu screen | `MenuScreen.Items` and navigation contract | Centred selectable menus |

## 🛡️ Privacy and Data

NuciXNA.Gui processes only host-provided UI state in memory and has no built-in persistence, network transfer, telemetry, or logging.

For the detailed description of how the application handles privacy and personal data, see [PRIVACY.md](./PRIVACY.md).

| Data | Purpose | Storage | Retention | Optional |
|------|---------|---------|-----------|----------|
| Host-provided text, selections, layout, and content identifiers | Render and interact with the host UI | Consuming process memory | Until controls/screens are cleared or the process ends | Determined by the host |

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Setup

```bash
git clone https://github.com/hmlendea/nucixna.gui.git
cd nucixna.gui
dotnet restore NuciXNA.GUI.slnx
```

### Build

```bash
dotnet build NuciXNA.GUI.slnx
```

### Test

```bash
dotnet test NuciXNA.GUI.slnx
```

### Release

```bash
dotnet build NuciXNA.GUI.slnx --configuration Release
dotnet test NuciXNA.GUI.slnx --configuration Release --no-build
dotnet pack NuciXNA.Gui/NuciXNA.Gui.csproj --configuration Release --no-build
```

Inspect the package before publishing it through the maintainer's authorised NuGet and GitHub release processes. Publication credentials are not stored in this repository.

### Dependencies

| Package | Version | Scope | Purpose |
|---------|---------|-------|---------|
| `MonoGame.Framework.DesktopGL` | `3.8.4` | Runtime | XNA/MonoGame rendering and input runtime |
| `NuciXNA.Graphics` | `2.2.4` | Runtime | Sprite rendering utilities and effects |
| `NuciXNA.Input` | `2.1.1` | Runtime | Input events, state, and arbitration |
| `NuciXNA.Primitives` | `2.1.7` | Runtime | Common geometry, colour, and scale types |
| `NUnit` | `4.6.1` | Test | Unit test framework |

## ⚙️ GitHub Actions

| Workflow | Purpose | What it does |
|----------|---------|--------------|
| [.NET](https://github.com/hmlendea/nucixna.gui/blob/master/.github/workflows/dotnet.yml) | Validate pushes and pull requests to `master` | Installs .NET 10 and Microsoft core fonts, then restores, builds, and tests on Ubuntu |

## 🗂️ Project Structure

The solution contains the library and its NUnit test project, plus detailed documentation and one validation workflow.

### Projects and Packages

| Project | Type | Purpose |
|---------|------|---------|
| `NuciXNA.Gui/NuciXNA.Gui.csproj` | .NET library/NuGet package | Controls, screens, managers, and cursor |
| `NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj` | NUnit test project | Unit tests and test doubles |

### Directories

| Directory | Purpose |
|-----------|---------|
| `NuciXNA.Gui/Controls/` | `GuiControl`, concrete controls, and selection event model |
| `NuciXNA.Gui/Screens/` | `Screen`, `MenuScreen`, contracts, and `ScreenManager` |
| `NuciXNA.Gui.UnitTests/` | Control/screen fixtures and test helpers |
| `docs/` | Architecture, flows, API reference, concerns, and support guides |

## 🏗️ Architecture

NuciXNA.Gui uses retained control trees, a global root-control manager, screen lifecycle templates, synchronous NuciXNA.Input events, and per-frame MonoGame rendering.

See the [architecture documentation](./ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 📚 Documentation

The documentation index is the entry point for repository-specific architecture, behaviour, development, and diagnostic knowledge.

| Resource | Description |
|----------|-------------|
| [Documentation Index](./docs/INDEX.md) | Complete catalogue and navigation |
| [Quick Start](./docs/quick-start.md) | Minimal host and screen setup |
| [Runtime Flows](./docs/flows/) | Startup/rendering, input, and transitions |
| [Testing](./docs/testing.md) | Test organisation, coverage, and gaps |
| [Troubleshooting](./docs/troubleshooting.md) | Evidence-based diagnostics |

## 🌐 API Reference

See the [.NET API reference](./docs/api-reference/INDEX.md) for managers, screens, interfaces, cursor, controls, lifecycle order, failures, and test impact. The project has no HTTP controllers or endpoints.

## 🩺 Troubleshooting

See [Troubleshooting Guide](./docs/troubleshooting.md) for diagnostic procedures and resolutions to recurring problems.

## ❓ FAQ

See the [Frequently Asked Questions](./docs/faq.md) for responses to frequently raised inquiries.

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Add tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/nucixna.gui/issues) for problems or suggestions

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 🔗 Related Projects

- [NuciXNA.DataAccess](https://github.com/hmlendea/nucixna.dataaccess): Data access utilities for the NuciXNA stack
- [NuciXNA.Graphics](https://github.com/hmlendea/nucixna.graphics): Sprite rendering utilities
- [NuciXNA.Input](https://github.com/hmlendea/nucixna.input): Input event abstractions
- [NuciXNA.Primitives](https://github.com/hmlendea/nucixna.primitives): Common value types

## 🔒 Security

NuciXNA.Gui executes in the consuming process and has no built-in network, persistence, authentication, telemetry, or logging boundary.

For information on reporting security vulnerabilities, see [SECURITY.md](./SECURITY.md).

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/nucixna.gui/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or starring ⭐️ it on GitHub!

[![Donate](https://raw.githubusercontent.com/hmlendea/readme-assets/master/donate_generic.png)](https://hmlendea.go.ro/funding)

## 📄 License

This project is being distributed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for further information.