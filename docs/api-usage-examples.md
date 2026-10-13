# API Usage Examples

## Scope

This repository has no HTTP API, so there are no request/response payloads. The examples below show .NET calls, relevant preconditions, state changes, and emitted events.

## Configure Defaults

```csharp
GuiManager.Instance.DefaultFontName = "Fonts/Menu";
GuiManager.Instance.DefaultForegroundColour = Colour.White;
GuiManager.Instance.DefaultBackgroundColour = Colour.Transparent;
GuiManager.Instance.DefaultTextureLayout = TextureLayout.Stretch;
```

Controls with unset colour/font/layout values resolve these defaults dynamically. `DefaultMargins` currently has no production consumer.

## Register Root Controls

```csharp
protected override void DoLoadContent()
{
    GuiText title = new()
    {
        Id = "title",
        Text = "Settings",
        Location = new Point2D(32, 24),
        Size = new Size2D(480, 64)
    };

    GuiManager.Instance.RegisterControl(title);
}
```

Precondition: unique, non-null root `Id`. Result: base `Screen.LoadContent()` subsequently loads the registered root.

## Toggle State

```csharp
GuiMenuToggle toggle = new() { Text = "Fullscreen" };
toggle.StateChanged += (sender, e) => settings.Fullscreen = toggle.IsOn;

toggle.SetState(settings.Fullscreen);
toggle.SwitchState();
```

`SetState` with the current value emits nothing. A genuine change updates `IsOn` and synchronously raises `StateChanged`.

## Select Key/Value Items

```csharp
GuiMenuListSelector selector = new() { Text = "Resolution" };
selector.SetItems(
    new KeyValuePair<string, string>("1280x720", "1280 x 720"),
    new KeyValuePair<string, string>("1920x1080", "1920 x 1080"));

selector.SelectedItemChanged += (sender, e) =>
{
    ListSelectionEventArgs selected = (ListSelectionEventArgs)e;
    settings.ResolutionKey = selected.SelectedKey;
};

selector.SelectItemByKey("1920x1080");
```

`SetItems` copies input, raises `ItemsChanged`, resets, then selects index `0`. The event payload reports only the new mutable index/key/value.

## Request A Parameterised Screen

```csharp
public sealed class GameScreen(int levelId) : Screen
{
    protected override void DoLoadContent() { }
    protected override void DoUnloadContent() { }
    protected override void DoUpdate(GameTime gameTime) { }
    protected override void DoDraw(SpriteBatch spriteBatch) { }
}

ScreenManager.Instance.ChangeScreens(typeof(GameScreen), 7);
```

The constructor runs immediately. The object remains unloaded until the overlay is opaque; then the old screen is disposed and the new screen loads. Requests made while transitioning are ignored.

## Use A GuiMenuLink

```csharp
GuiMenuLink link = new()
{
    Text = "Level 7",
    TargetScreen = typeof(GameScreen),
    Parameters = [7]
};
```

Mouse click, focused Enter/`E`, or focused gamepad A raises inherited `Triggered`; the link forwards target and parameters to `ScreenManager`.

## Build A Composite Control

```csharp
public sealed class IconLabel : GuiControl
{
    private GuiImage icon;
    private GuiText label;

    protected override void DoLoadContent()
    {
        icon = new GuiImage
        {
            ContentFile = "UI/Save",
            Size = new Size2D(32, 32)
        };
        label = new GuiText
        {
            Text = "Save",
            Location = new Point2D(40, 0),
            Size = new Size2D(120, 32)
        };

        RegisterChildren(icon, label);
    }

    protected override void DoUnloadContent() { }
    protected override void DoUpdate(GameTime gameTime) { }
    protected override void DoDraw(SpriteBatch spriteBatch) { }
}
```

Do not compose by assigning `Parent` alone; that does not add to the parent's private child list.

## Use Cursor Modes

```csharp
Cursor paired = new(); // Cursors/idle + Cursors/click
paired.LoadContent();

Cursor custom = new()
{
    ContentFile = "UI/Cursor",
    LocationOffset = new Point2D(-2, -2)
};
custom.LoadContent();
```

Choose the mode before load. Runtime `ContentFile` mode changes reload only the idle sprite and do not reconcile click subscriptions.

## Focus A Root

```csharp
GuiManager.Instance.FocusControl(link);
```

The manager unfocuses focused registered roots, then focuses a root with the matching ID. Descendants are not indexed; passing a child will unfocus roots but cannot locate/focus that child.
