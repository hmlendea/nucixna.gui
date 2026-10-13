# GuiMenuText

**Type:** `NuciXNA.Gui.Controls.GuiMenuText`
**Base:** `GuiMenuItem`
**Source:** [`NuciXNA.Gui/Controls/GuiMenuText.cs`](../../../NuciXNA.Gui/Controls/GuiMenuText.cs)

## Contract

`GuiMenuText` adds no fields, events, or lifecycle hooks. It overrides `IsSelectable` with a getter that always returns false.

```csharp
public override bool IsSelectable => false;
```

All other `GuiMenuItem` members and activation handlers remain inherited. Consequently:
- `MenuScreen` directional and mouse-move selection skip the item.
- The item still inherits base hit testing, pointer events, focus methods, and `Triggered` activation behaviour.
- The inherited setter is no longer exposed through the concrete override.

## Tests And Change Impact

[`GuiMenuTextTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuTextTests.cs) verifies the label contract. Changes should preserve its distinction from interactive items or explicitly document a behavioural break.

## Related Documentation

- [GuiMenuItem](./GuiMenuItem.md)
- [MenuScreen](../MenuScreen.md)
