# API Reference

## Scope

NuciXNA.Gui is a class library, not an HTTP service. The repository contains no controllers, routes, middleware, authentication, authorisation, API version negotiation, request throttling, or wire-format error model. Consequently there are no endpoint files to document. This directory documents the public .NET surface instead.

## HTTP Endpoint Summary

| Method | Path | Controller | Action | Authentication |
|--------|------|------------|--------|----------------|
| N/A | N/A | None | No HTTP surface exists | N/A |

## HTTP Policies

| Concern | Repository Status |
|---------|-------------------|
| Authentication and authorisation | Not applicable; no HTTP or remote-call boundary |
| Versioning | No HTTP versioning; package metadata currently reports version `3.4.2` |
| Rate limiting and throttling | Not applicable |
| Request/response errors | Not applicable; .NET exceptions and events form the library boundary |

## Public Type Index

| Area | Type | Responsibility |
|------|------|----------------|
| Managers | [GuiManager](./GuiManager.md) | Root-control registry, control lifecycle, click/focus arbitration, defaults |
| Managers | [ScreenManager](./ScreenManager.md) | Current/pending screen ownership and black-overlay transitions |
| Screens | [Screen](./Screen.md) | Screen template lifecycle and global `GuiManager` coordination |
| Screens | [MenuScreen](./MenuScreen.md) | Centred menu layout and axis-aware focus navigation |
| Controls | [GuiControl](./GuiControl.md) | Retained control state, child ownership, input events, lifecycle template |
| Controls | [GuiText](./Controls/GuiText.md) | Text sprite plus child background image |
| Controls | [GuiImage](./Controls/GuiImage.md) | Texture sprite and visual-effect adapter |
| Controls | [GuiMenuItem](./Controls/GuiMenuItem.md) | Focused menu label and unified trigger event |
| Controls | [GuiMenuText](./Controls/GuiMenuText.md) | Non-selectable menu label |
| Controls | [GuiMenuLink](./Controls/GuiMenuLink.md) | Trigger-to-screen-transition adapter |
| Controls | [GuiMenuToggle](./Controls/GuiMenuToggle.md) | Boolean menu state and label |
| Controls | [GuiMenuListSelector](./Controls/GuiMenuListSelector.md) | Key/value collection and cyclic selection |
| Controls | [GuiTooltip](./Controls/GuiTooltip.md) | Bounded border/background/text composite |
| Host utility | [Cursor](./Cursor.md) | Host-owned custom cursor sprites |
| Contracts | [Interfaces](./Interfaces.md) | `IGuiControl` and `IScreen` public contracts |
| Models | [ListSelectionEventArgs](./Controls/ListSelectionEventArgs.md) | Selected index/key/value event payload |

`MenuScreenAxis` is documented with [MenuScreen](./MenuScreen.md).

## Shared Lifecycle Contract

`GuiControl` and `Screen` use synchronous, stateful lifecycles:
1. Construct.
2. Configure and register roots or children.
3. Call `LoadContent()` once.
4. Call `Update(GameTime)` and `Draw(SpriteBatch)` only while loaded.
5. Call `UnloadContent()` while loaded, or call `Dispose()` and permit it to unload automatically.

Loading twice or updating, drawing, or unloading while not loaded raises `InvalidOperationException`. Manager methods generally provide less explicit validation and permit reflection, dictionary, graphics, and content exceptions to propagate.

## Related Documentation

- [Architecture](../architecture.md)
- [Startup and Rendering](../flows/startup-and-rendering.md)
- [Input Handling](../flows/input-handling.md)
- [Screen Transitions](../flows/screen-transitions.md)
- [Data Model](../data-model.md)
- [Error Handling](../error-handling.md)
