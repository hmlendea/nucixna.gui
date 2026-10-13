# Design Decisions

This document records consequential implementation choices visible in source. The repository contains no architecture decision records, so historical rationale and rejected alternatives are unknown unless the implementation itself demonstrates them.

## 1. Singleton Managers (GuiManager, ScreenManager)

**Observed choice:** `NuciXNA.Gui.GuiManager.Instance` and `NuciXNA.Gui.Screens.ScreenManager.Instance` lazily construct an instance under `System.Threading.Lock`.

**Evidence:** [`GuiManager.cs`](../NuciXNA.Gui/GuiManager.cs) and [`ScreenManager.cs`](../NuciXNA.Gui/Screens/ScreenManager.cs).

**Consequences:** Most production code uses global manager state. Both constructors remain public, so the API permits additional manager instances even though internal code uses `Instance`. Only singleton construction is locked; manager collections and lifecycle methods are not generally thread-safe.

---

## 2. Retained Controls with Per-Frame Drawing

**Observed choice:** Controls retain state and hierarchy between calls. `GuiManager.Draw(SpriteBatch)` traverses loaded, visible roots every time the host calls it; each root then draws visible, enabled children.

**Evidence:** [`GuiManager.Draw`](../NuciXNA.Gui/GuiManager.cs) and [`GuiControl.Draw`](../NuciXNA.Gui/Controls/GuiControl.cs).

**Consequences:** The state model is retained-mode, while rendering follows the host frame loop. The repository implements no dirty-region tracking or render cache and publishes no performance guarantee.

---

## 3. Component-Based Control Hierarchy

**Observed choice:** `GuiControl` owns a private child list populated by protected `RegisterChild` and `RegisterChildren` methods. Concrete controls compose internal children, while root controls live in `GuiManager`'s dictionary.

**Evidence:** [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs), [`GuiText.cs`](../NuciXNA.Gui/Controls/GuiText.cs), and [`GuiTooltip.cs`](../NuciXNA.Gui/Controls/GuiTooltip.cs).

**Consequences:** Parent location and selected presentation/state properties affect descendants. `Parent` remains publicly settable despite an explicit TODO, so callers can bypass child-registration ownership rules.

---

## 4. Event-Based Interaction Model

**Observed choice:** Public property, lifecycle, focus, visibility, and input changes are exposed as .NET events. Loaded controls and screens subscribe directly to the external `InputManager` singleton and unsubscribe during unload.

**Evidence:** [`IGuiControl.cs`](../NuciXNA.Gui/Controls/IGuiControl.cs), [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs), and [`Screen.cs`](../NuciXNA.Gui/Screens/Screen.cs).

**Consequences:** Event delivery is synchronous on the caller/input thread. Correct unload/disposal is material because subscriptions otherwise retain objects. Constructor-raised `Created` events are not observable by subscribers attached after construction.

---

## 5. Template Method Pattern for Lifecycle

**Observed choice:** `GuiControl` and `Screen` expose non-virtual lifecycle methods that validate state, raise events, invoke abstract `DoLoadContent`, `DoUnloadContent`, `DoUpdate`, and `DoDraw` hooks, and coordinate children or `GuiManager`.

**Evidence:** [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs) and [`Screen.cs`](../NuciXNA.Gui/Screens/Screen.cs).

**Consequences:** Subclasses implement only hooks and cannot replace lifecycle ordering. A derived concrete control that overrides another concrete control's hook must call the base hook when it depends on that class's child setup or event registration, as `GuiMenuLink` and `GuiMenuToggle` do.

---

## 6. Dependency on NuciXNA.* Packages

**Observed choice:** The public and internal API directly uses MonoGame plus NuciXNA.Graphics, NuciXNA.Input, and NuciXNA.Primitives types.

**Evidence:** [`NuciXNA.Gui.csproj`](../NuciXNA.Gui/NuciXNA.Gui.csproj) and the public contracts in [`IGuiControl.cs`](../NuciXNA.Gui/Controls/IGuiControl.cs) and [`IScreen.cs`](../NuciXNA.Gui/Screens/IScreen.cs).

**Consequences:** Consumers inherit those package dependencies and type contracts. Upgrading any of them can affect source, binary, rendering, content, or input compatibility.

---

## 7. No Built-in Theming System

**Observed choice:** There is no theme, skin, resource-dictionary, or style object. Styling is expressed through manager defaults, inherited control properties, and explicit concrete-control properties.

**Evidence:** [`GuiManager.cs`](../NuciXNA.Gui/GuiManager.cs) and [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs).

**Consequences:** A host can centralise some colours, font, and texture layout through `GuiManager`, but must coordinate richer themes itself. `DefaultMargins` is currently unused by production controls.

---

## 8. No Persistence Layer

**Observed choice:** The repository contains no serialiser, database, file store, cache, migration, or external configuration provider.

**Evidence:** Production source under [`NuciXNA.Gui/`](../NuciXNA.Gui/).

**Consequences:** Control, menu, cursor, and screen state is process-local. Disposing a screen invokes `GuiManager.UnloadContent()`, unloads every registered root, and clears the global registry; a host must preserve any state required by a later screen instance.

---

## 9. Property Inheritance with Explicit Override

**Observed choice:** Unset `BackgroundColour`, `ForegroundColour`, and `FontName` values resolve through the parent chain and then `GuiManager`; `Opacity` resolves through parents and then `1.0f`; `Size` resolves through the parent and then `Size2D.Empty`. Parent enabled/visible state gates descendants.

**Evidence:** Property getters in [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs).

**Consequences:** Effective values can change without assigning the child when an ancestor or manager default changes. Assigning a property stores an explicit value; there is no public reset-to-inherited operation.

---

## 10. Screen Transition with Fade Effect

**Observed choice:** `ScreenManager` overlays a black, tiled `TextureSprite` from `ScreenManager/FillImage` with a `FadeEffect` speed of `3` and a multiplier range of `0.0f` to `2.0f`.

**Evidence:** [`ScreenManager.LoadContent`](../NuciXNA.Gui/Screens/ScreenManager.cs).

**Consequences:** The pending screen is constructed when requested but is not loaded, updated, or drawn during the fade. The old screen continues to update and draw. Once overlay opacity reaches `1.0f`, the manager disposes the old screen, assigns and loads the pending screen, and concludes the transition. The transition parameters are not public configuration.

---

## 11. Input Handling via NuciXNA.Input

**Observed choice:** Loaded controls and screens subscribe directly to `NuciXNA.Input.InputManager.Instance`; `GuiManager.Update` also queries mouse state and the shared `MouseButtonInputHandled` flag.

**Evidence:** [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs), [`Screen.cs`](../NuciXNA.Gui/Screens/Screen.cs), and [`GuiManager.cs`](../NuciXNA.Gui/GuiManager.cs).

**Consequences:** Input package state and event timing are part of the runtime contract. This repository does not invoke or configure the external input manager's host lifecycle, so consumers must follow NuciXNA.Input's contract separately.

---

## 12. Control ID Generation

**Observed choice:** `GuiControl` and `Screen` constructors initialise `Id` with a GUID string, but `Id` remains publicly settable. `GuiManager` registers roots with `Dictionary<string, GuiControl>.Add`.

**Evidence:** [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs), [`Screen.cs`](../NuciXNA.Gui/Screens/Screen.cs), and [`GuiManager.RegisterControl`](../NuciXNA.Gui/GuiManager.cs).

**Consequences:** Generated values are normally distinct but not a guaranteed identity boundary because callers can replace them. Duplicate or null root IDs fail at registration through dictionary exceptions.

---

## 13. Disposal Pattern with Finalizer

**Observed choice:** `GuiControl` and `Screen` implement `IDisposable`, guard repeated disposal with `IsDisposed`, unload loaded content, and declare finalisers that call public `Dispose()`.

**Evidence:** [`GuiControl.Dispose`](../NuciXNA.Gui/Controls/GuiControl.cs) and [`Screen.Dispose`](../NuciXNA.Gui/Screens/Screen.cs).

**Consequences:** Explicit disposal deterministically unsubscribes input events and unloads graphics wrappers. Finalisable objects incur GC overhead. The finaliser invokes the same `Dispose(true)` path as explicit disposal rather than a non-managed-resource path; this is current behaviour, not a recommendation.

---

## 14. MenuScreen Navigation Logic

**Observed choice:** `MenuScreen` centres and spaces `Items` along a vertical or horizontal axis, tracks `SelectedItemIndex`, and changes focus from mouse movement, keyboard direction keys, or gamepad direction buttons.

**Evidence:** [`MenuScreen.cs`](../NuciXNA.Gui/Screens/MenuScreen.cs) and [`MenuScreenAxis.cs`](../NuciXNA.Gui/Screens/MenuScreenAxis.cs).

**Consequences:** Navigation wraps and skips non-selectable items. Key/button mappings and centring rules are embedded in the class. Direction input with an empty `Items` list can reach an invalid index; no test currently covers that boundary.

---

## 15. GuiMenuListSelector Cyclic Selection

**Observed choice:** `GuiMenuListSelector` stores an insertion-ordered dictionary copy, automatically selects index `0` after non-empty `SetItems`, and wraps next/previous selection. Keyboard, gamepad, and mouse handlers invoke the same selection methods.

**Evidence:** [`GuiMenuListSelector.cs`](../NuciXNA.Gui/Controls/GuiMenuListSelector.cs).

**Consequences:** Single-item next/previous calls are no-ops. On an empty collection, next reaches invalid index `0` and throws, while previous computes the existing index `-1` and returns before validation. String-only items receive generated GUID keys. `GetItems()` returns a defensive dictionary copy; `Keys` and `Values` expose the current dictionary enumerables.

---

## 16. Cursor Implementation

**Observed choice:** `Cursor` is a separately constructed sealed class, not a `GuiControl` and not a singleton. It subscribes to mouse movement and, in paired-sprite mode, mouse-button events.

**Evidence:** [`Cursor.cs`](../NuciXNA.Gui/Cursor.cs).

**Consequences:** The host owns load, update, draw, and unload calls and decides draw order. At load time, empty `ContentFile` uses `Cursors/idle` and `Cursors/click`, while a non-empty value uses one sprite without click subscriptions. Changing the value while loaded reloads only the idle sprite and does not reconcile click sprite/subscriptions from the original mode.

---

## 17. Tooltip Screen Boundary Clamping

**Observed choice:** `GuiTooltip` composes border, background, and text children and mutates its own `Location` during update when its display rectangle exceeds `ScreenManager.Instance.Size`.

**Evidence:** [`GuiTooltip.cs`](../NuciXNA.Gui/Controls/GuiTooltip.cs).

**Consequences:** The requested location can be adjusted on either axis every update. Tooltip visibility starts as false and must be changed through `Show()`. Bounds depend on the screen manager's latest back-buffer size.

---

## 18. No Async/Await in Core Library

**Observed choice:** All public lifecycle, navigation, selection, and rendering APIs are synchronous. Production source contains no `Task`, `async`, worker thread, timer, or scheduler.

**Evidence:** Production source under [`NuciXNA.Gui/`](../NuciXNA.Gui/).

**Consequences:** Work performed by hooks, event handlers, content loading, and rendering remains on the caller's thread. A host introducing background work must marshal access and respect graphics/input thread requirements; the library supplies no coordination.

---

## 19. GPL-3.0-or-later License

**Observed choice:** The project manifest declares `GPL-3.0-or-later`, and the repository contains the corresponding licence text.

**Evidence:** [`NuciXNA.Gui.csproj`](../NuciXNA.Gui/NuciXNA.Gui.csproj) and [`LICENSE`](../LICENSE).

**Consequences:** Distribution and derivative-work obligations are governed by that licence. This document does not provide legal interpretation, and the historical licence rationale is not recorded.

---

## 20. .NET 10.0 Target Framework

**Observed choice:** Both projects target only `net10.0`; the GitHub Actions workflow installs .NET `10.0.x`.

**Evidence:** [`NuciXNA.Gui.csproj`](../NuciXNA.Gui/NuciXNA.Gui.csproj), [`NuciXNA.Gui.UnitTests.csproj`](../NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj), and [`.github/workflows/dotnet.yml`](../.github/workflows/dotnet.yml).

**Consequences:** Building and testing require a compatible .NET 10 SDK. The repository provides no multi-target compatibility claim, and the selection rationale is not recorded.