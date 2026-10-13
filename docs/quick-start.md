# Quick Start

## Prerequisites

- A MonoGame/NuciXNA host targeting a framework compatible with `net10.0`.
- NuciXNA.Graphics and NuciXNA.Input initialised according to their package contracts.
- Host content for configured fonts/images plus `ScreenManager/FillImage`; cursor assets are required only when using paired cursor mode.

## Install

```bash
dotnet add package NuciXNA.Gui
```

## Create A Screen

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

public sealed class MainMenuScreen : Screen
{
    protected override void DoLoadContent()
    {
        GuiMenuLink start = new()
        {
            Id = "start",
            Text = "Start",
            TargetScreen = typeof(GameScreen),
            Location = new Point2D(100, 100),
            Size = new Size2D(320, 48)
        };

        GuiManager.Instance.RegisterControl(start);
    }

    protected override void DoUnloadContent() { }
    protected override void DoUpdate(GameTime gameTime) { }
    protected override void DoDraw(SpriteBatch spriteBatch) { }
}
```

Register roots in `DoLoadContent()`. `Screen.LoadContent()` invokes that hook and then loads all manager roots.

## Wire The Host Lifecycle

```csharp
private SpriteBatch spriteBatch;
private Cursor cursor;

protected override void LoadContent()
{
    spriteBatch = new SpriteBatch(GraphicsDevice);

    GuiManager.Instance.DefaultFontName = "Fonts/Menu";

    cursor = new Cursor
    {
        ContentFile = "UI/Cursor"
    };
    cursor.LoadContent();

    ScreenManager.Instance.StartingScreenType = typeof(MainMenuScreen);
    ScreenManager.Instance.LoadContent();

    base.LoadContent();
}

protected override void Update(GameTime gameTime)
{
    // Advance NuciXNA.Input according to that package's host contract.
    cursor.Update(gameTime);
    ScreenManager.Instance.Update(gameTime);
    base.Update(gameTime);
}

protected override void Draw(GameTime gameTime)
{
    spriteBatch.Begin();
    ScreenManager.Instance.Draw(spriteBatch);
    cursor.Draw(spriteBatch);
    spriteBatch.End();

    base.Draw(gameTime);
}

protected override void UnloadContent()
{
    ScreenManager.Instance.UnloadContent();
    cursor.UnloadContent();
    base.UnloadContent();
}
```

`Cursor` is optional and host-owned. `SpriteBatch.Begin`/`End`, cadence, input-manager advancement, and draw layering remain host responsibilities.

## Use A MenuScreen

```csharp
public sealed class SettingsScreen : MenuScreen
{
    public SettingsScreen()
    {
        Axis = MenuScreenAxis.Vertical;
        Spacing = 24;
        Items =
        [
            new GuiMenuToggle { Id = "fullscreen", Text = "Fullscreen" },
            new GuiMenuListSelector { Id = "difficulty", Text = "Difficulty" },
            new GuiMenuLink { Id = "back", Text = "Back", TargetScreen = typeof(MainMenuScreen) }
        ];

        ((GuiMenuListSelector)Items[1]).SetItems("Easy", "Normal", "Hard");
    }
}
```

`MenuScreen` registers/loads `Items`, lays them out, and manages focus navigation.

## Verify

```bash
dotnet restore NuciXNA.GUI.slnx
dotnet build NuciXNA.GUI.slnx
dotnet test NuciXNA.GUI.slnx
```

Continue with [API Usage Examples](./api-usage-examples.md), [Input Handling](./flows/input-handling.md), and [Troubleshooting](./troubleshooting.md).
