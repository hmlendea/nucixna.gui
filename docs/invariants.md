# Invariants

This document separates enforced invariants from conventions and known gaps.

## Enforced Lifecycle Invariants

| Invariant | Enforcement |
|-----------|-------------|
| A loaded control/screen cannot be loaded again | `InvalidOperationException` at `LoadContent()` entry |
| An unloaded control/screen cannot update, draw, or unload | `InvalidOperationException` at method entry |
| Opacity assignments remain within `[0.0f, 1.0f]` | `GuiControl.Opacity` clamps before storing |
| Disposal is idempotent after `IsDisposed` becomes true | Guard inside object lock |
| A registered root ID is unique and non-null at insertion | `Dictionary.Add` exceptions |
| Invalid selector key/value/index is rejected when it differs from current selection | Explicit checks and exceptions |

## Ownership Invariants

- Manager registry values are roots; descendants are owned by a parent private child list.
- `RegisterChild`/`RegisterChildren` both append and assign `Parent`.
- Screen load hooks register roots before base screen loading calls `GuiManager.LoadContent`.
- Screen unload owns the complete global root-registry unload/clear, not a per-screen subset.
- Cursor lifecycle is host-owned and independent of manager lifecycles.

These ownership rules are partly conventional: public `Parent` can be assigned directly, and public manager constructors permit alternate instances. The source TODO explicitly questions public `Parent`.

## Ordering Invariants

### Control Load

`ContentLoading` -> subscribe input -> `DoLoadContent` -> load children -> set flags -> `ContentLoaded`.

### Control Update

`Updating` -> prune disposed/null children and restore parent links -> `DoUpdate` -> update enabled children -> `Updated`.

### Control Draw

`Drawing` -> `DoDraw` -> draw enabled/visible children -> `Drawn`.

### Screen Lifecycle

Screen hook executes before the corresponding `GuiManager` operation for load/update/draw/unload. Draw clears the graphics device before the screen hook.

### Input

Manager click polling traverses root snapshot in reverse and stops at the shared handled flag. It resets that flag before root updates. Direct input-manager event subscribers form a separate path.

### Transition

The old screen remains current until the overlay is opaque. It is disposed before the pending screen is assigned/loaded. Pending screen update/draw never occurs before load.

## Effective-State Invariants

- A disabled parent makes a child effectively disabled.
- A hidden parent makes a child effectively hidden.
- Child screen location equals local location plus recursive parent screen location.
- Unset child colours/font/opacity/size resolve through documented ancestor/fallback rules.
- `GuiMenuText.IsSelectable` is always false.
- `GuiMenuToggle.StateChanged` occurs only on a genuine boolean transition.
- Non-empty `GuiMenuListSelector.SetItems` selects index `0` after `ItemsChanged`.

## Not Enforced

| Intended Or Required Condition | Gap |
|--------------------------------|-----|
| Parent graph is acyclic | No cycle detection; recursive getters can overflow |
| `Parent` matches actual child ownership | Direct assignment can diverge from private list |
| Registered `Id` remains stable | ID is publicly mutable after dictionary insertion |
| Lifecycle calls are serialised | No thread-affinity or general concurrency guard |
| Transition completes or rolls back | Exceptions at opaque swap can leave partial state |
| Cursor load/unload is balanced once | No loaded guard; repeated load can duplicate subscriptions |
| Menu has at least one item before direction input | Empty-list direction can index `-1` |
| Selector empty next/previous is symmetric | Next throws; previous can return early |

## Verification Map

Control/screen lifecycle, many property rules, toggles, links, and selectors have NUnit coverage under [`NuciXNA.Gui.UnitTests/`](../NuciXNA.Gui.UnitTests/). Manager input order, transitions, MenuScreen, Cursor, GuiText, GuiImage, and GuiTooltip invariants lack focused tests.

When changing an invariant, update its implementation, tests, [API Reference](./api-reference/INDEX.md), relevant flow, and compatibility notes together.
