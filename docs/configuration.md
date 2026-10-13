# Configuration

NuciXNA.Gui has no repository-owned JSON, XML, environment-variable, or command-line configuration. Consumers configure the library through public properties before or during the MonoGame lifecycle.

## Configuration Sources

Configuration comes from:
- [`NuciXNA.Gui.GuiManager`](../NuciXNA.Gui/GuiManager.cs), for global control defaults.
- [`NuciXNA.Gui.Screens.ScreenManager`](../NuciXNA.Gui/Screens/ScreenManager.cs), for the initial screen and host-owned `SpriteBatch` reference.
- Individual controls, whose explicit values override inherited values.
- [`NuciXNA.Gui.Cursor`](../NuciXNA.Gui/Cursor.cs), for cursor assets, size, scale, and offset.
- MonoGame content assets supplied by the consuming application.

There is no precedence between files or environments because no external configuration provider exists.

## GuiManager Configuration

Access global defaults through `NuciXNA.Gui.GuiManager.Instance`.

### Default Styling Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DefaultBackgroundColour` | `Colour` | `Colour.Transparent` | Default background colour |
| `DefaultForegroundColour` | `Colour` | `Colour.Black` | Default foreground and text colour |
| `DefaultFontName` | `string` | `"DefaultFont"` | Default SpriteFont asset name |
| `DefaultTextureLayout` | `TextureLayout` | `TextureLayout.Stretch` | Default image layout |
| `DefaultMargins` | `int` | `8` | Public manager value; no production control currently reads it |

### Configuration Example

```csharp
protected override void LoadContent()
{
    GuiManager.Instance.DefaultFontName = "Fonts/UIFont";
    GuiManager.Instance.DefaultForegroundColour = Colour.Black;
    GuiManager.Instance.DefaultBackgroundColour = Colour.Transparent;
    GuiManager.Instance.DefaultTextureLayout = TextureLayout.Stretch;
}
```

### Property Inheritance

`BackgroundColour`, `ForegroundColour`, and `FontName` resolve from an explicit control value, then the parent, then `GuiManager`. `Opacity` resolves from the control, then its parent, then the hard-coded value `1.0f`; `GuiManager` has no opacity default. `GuiImage.TextureLayout` uses `DefaultTextureLayout` when unset. `GuiText.Margins` defaults to `0` independently of `DefaultMargins`.

## ScreenManager Configuration

Access screen configuration through `NuciXNA.Gui.Screens.ScreenManager.Instance`.

### Properties

| Property | Type | Default | Required | Description |
|----------|------|---------|----------|-------------|
| `StartingScreenType` | `Type` | `null` | Yes | Type instantiated by `LoadContent()` and cast to `IScreen` |
| `SpriteBatch` | `SpriteBatch` | `null` | No repository consumer | Host-set reference retained by the manager |
| `Size` | `Size2D` | Current back-buffer size | Read-only | Refreshed outside transitions during `Update()` |
| `Transitioning` | `bool` | `false` | Read-only | Indicates an active screen transition |

### Configuration Example

```csharp
protected override void LoadContent()
{
    ScreenManager.Instance.StartingScreenType = typeof(MainMenuScreen);
    ScreenManager.Instance.LoadContent();
}
```

### Validation

- Set `StartingScreenType` before `LoadContent()`.
- The type must be constructible without arguments and implement `NuciXNA.Gui.Screens.IScreen`.
- The implementation performs no explicit validation. Reflection, construction, or cast exceptions propagate to the caller.
- Transition colour, fade speed, and fill-image path are fixed in `ScreenManager`; no transition-duration property exists.

## Cursor Configuration

`NuciXNA.Gui.Cursor` is a normal sealed class, not a singleton. The consuming application owns each instance.

### Configuration

| Property | Default | Effect |
|----------|---------|--------|
| `ContentFile` | `null` | At load, empty uses the idle/click pair and non-empty uses one sprite; runtime mode changes do not reconcile click subscriptions |
| `LocationOffset` | `Point2D.Empty` | Offset applied to the mouse location |
| `Frames` | `1` | Public cursor animation metadata; not consumed internally |
| `SpriteSize` | `Size2D.Empty` | Preserves the texture sprite's size unless set |
| `Scale` | `Scale2D.One` | Scale applied to cursor sprites |

### Content Requirements

With no `ContentFile`, provide the conventional assets:
```
Cursors/idle
Cursors/click
```

Changing `ContentFile` after loading immediately unloads and reloads only the idle sprite. It does not add/remove the click sprite or button subscriptions from the original load mode. The application must still call `UnloadContent()` when the cursor is no longer required.

---

## Control-Level Configuration

Controls configure presentation and behaviour through their public properties. State transitions use methods because `IsEnabled` has a private setter and `IsVisible` is not publicly settable.

```csharp
var link = new GuiMenuLink
{
    Id = "start-game",
    Text = "Start Game",
    FontName = "Fonts/Large",
    ForegroundColour = Colour.Yellow,
    BackgroundColour = Colour.Blue,
    Opacity = 0.8f,
    TargetScreen = typeof(GameScreen)
};

link.Enable();
link.Show();
```

### Inheritance Precedence

```
1. Explicit control property, when the property has been set.
2. Parent control effective value.
3. Further ancestors in order.
...
N. Applicable `GuiManager` default, or the property's hard-coded fallback.
```

---

## Content Configuration

### Content Lifecycle

The public lifecycle methods do not accept a `ContentManager`; NuciXNA graphics types resolve assets through their own integration layer.

```csharp
protected override void LoadContent()
{
    cursor = new Cursor();
    cursor.LoadContent();

    ScreenManager.Instance.StartingScreenType = typeof(MainMenuScreen);
    ScreenManager.Instance.LoadContent();
}
```

### Required Content Assets

| Component | Asset Path | Type | Notes |
|-----------|------------|------|-------|
| Cursor | `Cursors/idle` | Texture | Used when `ContentFile` is empty |
| Cursor | `Cursors/click` | Texture | Used when `ContentFile` is empty |
| Screen manager | `ScreenManager/FillImage` | Texture | Black tiled transition overlay |
| Text controls | `DefaultFont` or configured name | Sprite font | Resolved from `FontName` |
| Image controls | `GuiImage.ContentFile` | Texture | Set per image |

### Font Configuration

```csharp
GuiManager.Instance.DefaultFontName = "Fonts/UIFont";
control.FontName = "Fonts/UIFont";
```

---

## Runtime Configuration Changes

### Changing Defaults at Runtime

```csharp
// Unset control properties resolve defaults on every getter call.
GuiManager.Instance.DefaultForegroundColour = Colour.Red;
```

Existing controls that inherit the default observe the new value; controls with an explicit value do not.

### Changing Screen at Runtime

```csharp
// From any control event handler
startButton.Clicked += (s, e) =>
{
    ScreenManager.Instance.ChangeScreens<GameScreen>();
};
```

## Configuration Validation

The repository has no configuration validator. A host may validate `StartingScreenType` before calling `LoadContent()`, but that is host policy rather than library behaviour. Missing or invalid assets fail through the NuciXNA graphics/content layer.

## Configuration Anti-Patterns

| Anti-Pattern | Problem | Solution |
|--------------|---------|----------|
| Calling `ScreenManager.LoadContent()` before setting `StartingScreenType` | Reflection receives a null type | Set a constructible `IScreen` type first |
| Treating `Cursor` as a singleton | `Cursor.Instance` does not exist | Construct and retain a cursor instance |
| Passing a `ContentManager` to lifecycle methods | No such overload exists | Call parameterless `LoadContent()` |
| Setting `IsEnabled` or `IsVisible` directly | Setters are not public | Use `Enable()`/`Disable()` and `Show()`/`Hide()` |
| Expecting transition timing to be configurable | Fade speed is fixed in source | Change `ScreenManager` deliberately and test the transition |

---

## Minimal Configuration

```csharp
protected override void LoadContent()
{
    GuiManager.Instance.DefaultFontName = "Fonts/UIFont";

    cursor = new Cursor();
    cursor.LoadContent();

    ScreenManager.Instance.StartingScreenType = typeof(MainMenuScreen);
    ScreenManager.Instance.LoadContent();
    base.LoadContent();
}

protected override void Update(GameTime gameTime)
{
    cursor.Update(gameTime);
    ScreenManager.Instance.Update(gameTime);
    base.Update(gameTime);
}
```

The host remains responsible for updating `NuciXNA.Input.InputManager` according to that package's contract; this repository neither configures nor invokes its update lifecycle.