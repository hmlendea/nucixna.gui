# Troubleshooting

## Content Already Loaded

**Symptom:** `InvalidOperationException: Content already loaded`.

**Cause:** `LoadContent()` was called twice on the same control/screen without unload.

**Action:** Make host lifecycle calls once per loaded interval. Do not manually load roots that `Screen.LoadContent()` will load through `GuiManager`.

## Content Not Loaded

**Symptom:** `InvalidOperationException` during update, draw, or unload.

**Cause:** The control/screen was never loaded, load failed before its flag was set, or it was already unloaded/disposed.

**Action:** Trace lifecycle events/hooks and original load exception. Partial load has no automatic rollback; clean up subscriptions/resources deliberately.

## Starting Screen Construction Fails

**Symptom:** Reflection, cast, or constructor exception in `ScreenManager.LoadContent()`.

**Action:** Set `StartingScreenType` before load to a parameterless type derived from `Screen`. Independent `IScreen` implementations are incompatible with the manager's concrete cast path.

## Screen Change Construction Fails

**Symptom:** `ChangeScreens` throws immediately.

**Action:** Ensure target derives from `Screen` and constructor arguments exactly match. Constructors execute at request time; content loads later.

## Screen Change Is Ignored

**Symptom:** No exception and no new transition.

**Cause:** `ScreenManager.Transitioning` was already true.

**Action:** Wait or queue the desired target in host state.

## Transition Fails At Opaque

**Symptom:** Old screen disappears, exception propagates, manager remains transitioning.

**Cause:** Old disposal or pending load failed after the overlay reached full opacity.

**Action:** Diagnose the first exception and content/hook/root involved. The library has no rollback; recreate/reset at host level or implement/test a recovery policy.

## Control Does Not Update

Check:
- It is a registered root or a child registered through protected child registration.
- `IsContentLoaded` is true.
- It and every ancestor are enabled.
- It has not been disposed and removed.
- A screen transition has not replaced/cleared its registry.

## Control Does Not Draw

Check loaded and effective visibility. Root enabled state does not prevent root draw, but child draw requires enabled and visible. Verify host `SpriteBatch.Begin`/`End`, content, and draw layering.

## Control Does Not Receive Keyboard Or Gamepad Input

Check enabled, visible, and focused state. `GuiManager.FocusControl` searches registered roots only; passing a descendant cannot focus it. Verify NuciXNA.Input is advanced according to its package contract.

## Click Goes To An Unexpected Control

Manager polling traverses root snapshot in reverse, then each root parent-first through children. Direct input-manager button events are a separate path. There is no public z-index/order API. Inspect `DisplayRectangle` and the shared handled flag timing.

## Hover State Is Stale

Hover changes only on mouse-move events. Moving/resizing a control under a stationary pointer does not recompute `IsHovered`; trigger host pointer movement or revise/test the model.

## Menu Direction Input Crashes

An empty `MenuScreen.Items` list can reach index `-1` when a direction event attempts focus. Add at least one selectable item or guard input via a library fix with tests.

## Selector Empty-List Behaviour Is Surprising

On an empty selector, next throws at index `0`; previous can no-op because current and computed index are both `-1`. Call only after non-empty `SetItems` or define/test a new symmetric contract.

## Duplicate Control ID

`GuiManager.RegisterControl(s)` uses `Dictionary.Add`. Use unique non-null root IDs and do not mutate IDs after registration.

## Screen Background Colour Does Not Change

`Screen` converts/caches `BackgroundColour` during `LoadContent()`. Changing it while loaded raises the property event but does not refresh the cached clear colour. Reload or change the implementation with tests.

## Cursor Uses The Wrong Click Behaviour

Choose paired versus custom `ContentFile` before `LoadContent()`. Runtime mode changes reload only the idle sprite and do not reconcile click sprite/subscriptions. Balance one load with one unload.

## Tooltip Is Outside Or Mis-Sized

Verify `ScreenManager.Size`, tooltip `Size`, parent offsets, and `BorderSize`. Oversized tooltips and invalid border sizes are not validated; screen size is not refreshed during transitions.

## Linux Build Or Test Font Failure

Reproduce the font installation in [the CI workflow](../.github/workflows/dotnet.yml), which installs `ttf-mscorefonts-installer` and refreshes the font cache.

## No Diagnostic Output

The library has no logger. Subscribe host diagnostics to public events and preserve the original exception. Avoid high-frequency update/draw/mouse logs and sensitive displayed values. See [Logging](./logging.md).
