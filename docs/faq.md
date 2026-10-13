# Frequently Asked Questions

## Is NuciXNA.Gui An Immediate-Mode GUI?

No. Controls retain state and parent-child relationships. They are redrawn whenever the host calls draw, which is ordinary game-loop rendering rather than an immediate-mode control model.

## Does The Library Provide An HTTP API?

No. It is an in-process .NET library. [API Reference](./api-reference/INDEX.md) documents .NET types; authentication, routes, responses, and throttling are not applicable.

## Who Owns The Game Loop?

The consuming MonoGame host. It supplies load/unload calls, `GameTime`, `SpriteBatch`, batching, input-manager lifecycle, update/draw cadence, and cursor draw order.

## Is Cursor A Singleton?

No. Construct and retain a `Cursor` instance. `GuiManager` and `ScreenManager` expose static `Instance`; their constructors are also public.

## Where Should Controls Be Registered?

Register root controls with `GuiManager` during a screen's `DoLoadContent()`. Composite controls add descendants through protected `RegisterChild`/`RegisterChildren`. Do not register descendants as roots.

## Why Does Assigning Parent Not Display Or Update A Child?

`Parent` only changes the reference used for effective values/coordinates. It does not append to the parent's private child list. Use protected registration inside a composite control.

## How Is Focus Different From Menu Selection?

`IsFocused` belongs to controls and gates keyboard/gamepad events. `MenuScreen.SelectedItemIndex` tracks navigation and calls `GuiManager.FocusControl` for root items. There is no `GuiMenuItem.IsSelected` property.

## Can I Configure Transition Duration?

Not through the public API. Transition asset, tint, effect speed/range, and threshold are fixed in `ScreenManager`.

## Are Both Screens Loaded During A Transition?

No. The pending screen is constructed at request time but stays unloaded. The old screen updates/draws under the black overlay, is disposed at opaque, and then the pending screen loads.

## How Do I Preserve State Across Screens?

Store it in a host-owned model/service and pass or read it from new screen instances. Old screen and controls are disposed/unloaded, and the global root registry is cleared.

## Does The Library Persist, Log, Or Send Data?

No built-in persistence, logging, telemetry, or networking exists. The host controls any such behaviour. See [State and Persistence](./state-and-persistence.md), [Logging](./logging.md), and [Privacy](../PRIVACY.md).

## Why Does DefaultMargins Not Affect GuiText?

`GuiManager.DefaultMargins` exists and defaults to `8`, but no production control currently reads it. `GuiText.Margins` defaults independently to `0`.

## How Do I Remove One Root Control?

There is no unregister method. Disposing a registered root marks it disposed; the next `GuiManager.Update()` removes it. Screen unload clears all roots.

## Why Did A Screen Change Request Do Nothing?

`ScreenManager.ChangeScreens` silently returns while `Transitioning` is true. Queue the intent in host code or wait for transition completion.

## Are Rendering/Input Paths Fully Tested?

No. Existing NUnit tests cover substantial base lifecycle/properties and menu-control state, but managers, transitions, MenuScreen, Cursor, and concrete rendering/input integration have major gaps. See [Testing](./testing.md).
