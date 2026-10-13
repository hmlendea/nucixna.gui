# State And Persistence

## Persistence Boundary

The repository contains no filesystem access, database, serialiser, cache, migration, backup, restore, telemetry store, or network transport. All library-owned state is transient process memory. The consuming host owns any persistence and must map persisted values into new screen/control instances.

See the root [Privacy Policy](../PRIVACY.md) for the repository's data-handling declaration.

## State Ownership

| State | Owner | Creation | Destruction Or Reset |
|-------|-------|----------|----------------------|
| Global control defaults | `GuiManager` instance | Constructor | Instance lifetime; no reset API |
| Root controls | `GuiManager` dictionary | Explicit registration | Complete clear on manager unload; disposed roots removed on update |
| Child controls | Parent `GuiControl` private list | Protected registration during load | Unload recursively unloads then clears; dispose snapshots/disposes |
| Current/pending screens | `ScreenManager` | Starting load/change request | Old current disposed at transition; shutdown disposes current |
| Menu index/layout | `MenuScreen` | Constructor/update/input | Screen disposal |
| Toggle state | `GuiMenuToggle` | Defaults false; explicit/trigger change | Control lifetime |
| Selector items/selection | `GuiMenuListSelector` | Constructor/`SetItems` | Control lifetime or next `SetItems` |
| Cursor state/sprites | Host-owned `Cursor` | Construction/load/input events | Cursor unload/host release |

## Screen Changes And State Loss

A screen transition:
1. Constructs a pending screen.
2. Keeps the old screen active under the overlay.
3. Disposes the old screen at opaque.
4. Old screen unload clears every root from the global `GuiManager`.
5. Assigns and loads the pending screen.

Control state is not transferred. Persist state in a host-owned model or service and pass references/constructor values to the next screen. Because construction occurs at request time but loading occurs after the fade, constructor-captured values can differ from values read later during `DoLoadContent()`.

## Effective Versus Local Control State

`IsEnabled` and `IsVisible` combine local flags with ancestor state. Re-enabling/showing a parent reveals each child's existing local state; it does not overwrite children. Colours, font, opacity, size, and location similarly distinguish local backing values from computed effective values.

There is no public operation to clear an explicitly assigned colour/font/opacity/size back to its unset/inherited state.

## Deferred And Synchronous Changes

- Property setters and state methods mutate immediately and synchronously raise events.
- Selector `SetItems` raises `ItemsChanged` before automatic selection and its selection event.
- Disposed root removal is deferred until `GuiManager.Update`.
- Transition requests construct immediately but defer load/swap until overlay opacity reaches the threshold.
- Cursor `ContentFile` changes while loaded reload the idle sprite immediately, but do not reconcile mode subscriptions.

## Persistence Guidance

A host persistence adapter should:
- Store domain settings/state, not control or graphics-wrapper instances.
- Rebuild controls in `DoLoadContent()` from persisted/host state.
- Update host state from `StateChanged`, `SelectedItemChanged`, or `Triggered` handlers.
- Avoid serialising generated control IDs or selector GUID keys unless the host deliberately treats them as stable; both are runtime-generated and caller-replaceable.
- Complete persistence before requesting a transition if old-screen disposal would remove the only state reference.

## Absent Operational Concerns

| Concern | Status |
|---------|--------|
| Database schema/migrations | None |
| Cache invalidation | None |
| Cross-process consistency | None |
| Transactions | None |
| Retention/deletion jobs | None |
| Backup/restore | None |
| Offline synchronisation | None |

## Change Impact

Adding persistence would introduce a new trust/data boundary and require updates to [Privacy](../PRIVACY.md), [Security](../SECURITY.md), [Architecture](../ARCHITECTURE.md), configuration, error handling, tests, and deployment documentation.
