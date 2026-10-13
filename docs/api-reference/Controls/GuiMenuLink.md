# GuiMenuLink

**Type:** `NuciXNA.Gui.Controls.GuiMenuLink`
**Base:** `GuiMenuItem`
**Source:** [`NuciXNA.Gui/Controls/GuiMenuLink.cs`](../../../NuciXNA.Gui/Controls/GuiMenuLink.cs)

## Purpose

`GuiMenuLink` converts `GuiMenuItem.Triggered` into a `ScreenManager.ChangeScreens` request.

## Public Members

| Member | Type | Semantics |
|--------|------|-----------|
| `TargetScreen` | `Type` | Target type; raises `TargetScreenChanged` on assignment |
| `Parameters` | `object[]` | Constructor arguments; raises `ParametersChanged` on assignment |

All `GuiMenuItem` members/events are inherited.

## Lifecycle And Trigger

`DoLoadContent()` calls the base hook, then subscribes `OnTriggered`. `DoUnloadContent()` calls the base hook, then unsubscribes. On trigger:

```csharp
ScreenManager.Instance.ChangeScreens(TargetScreen, Parameters);
```

The pending screen is constructed synchronously at trigger time. It is loaded only after the transition overlay reaches full opacity.

## Failure And Edge Semantics

- The link does not validate null type, `IScreen` compatibility, constructor arguments, or transition state.
- Reflection/cast/constructor exceptions propagate from `ScreenManager`.
- Triggering during an active transition is silently ignored by the manager.
- Generic interface-only `IScreen` implementations still fail because the manager's reflected path casts to concrete `Screen`.

## Tests And Change Impact

[`GuiMenuLinkTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuLinkTests.cs) and [`GuiMenuLinkExtendedTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuLinkExtendedTests.cs) cover link properties/behaviour. End-to-end reflection, parameter forwarding, ignored requests, and transition failures lack manager-level tests.

## Related Documentation

- [GuiMenuItem](./GuiMenuItem.md)
- [ScreenManager](../ScreenManager.md)
- [Screen Transitions](../../flows/screen-transitions.md)
