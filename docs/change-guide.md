# Change Guide

## General Procedure

1. Identify the owning type and its caller/callee lifecycle.
2. Read the relevant API reference, flow, invariant, and existing tests.
3. State the current contract and intended difference before editing.
4. Make the smallest ownership-aligned change.
5. Add focused tests for normal, boundary, failure, and ordering cases.
6. Run the narrow test project/filter, then the complete solution.
7. Update public/API/flow/root documentation affected by the contract.

## Change Matrix

| Change | Primary Source | Required Neighbours | Tests And Documentation |
|--------|----------------|---------------------|-------------------------|
| Base control lifecycle/property/input | [`GuiControl.cs`](../NuciXNA.Gui/Controls/GuiControl.cs) and [`IGuiControl.cs`](../NuciXNA.Gui/Controls/IGuiControl.cs) | `GuiManager`, every concrete control | Base lifecycle/property tests, input tests, API/invariants/flows |
| Root registration/focus/order/defaults | [`GuiManager.cs`](../NuciXNA.Gui/GuiManager.cs) | `Screen`, `MenuScreen`, `GuiControl` | New manager tests, input/render flows, configuration |
| Screen lifecycle/clear/input | [`Screen.cs`](../NuciXNA.Gui/Screens/Screen.cs) and [`IScreen.cs`](../NuciXNA.Gui/Screens/IScreen.cs) | `ScreenManager`, `GuiManager`, derived screens | Screen fixtures plus manager integration |
| Transition behaviour | [`ScreenManager.cs`](../NuciXNA.Gui/Screens/ScreenManager.cs) | `GuiMenuLink`, graphics effects/content, screen disposal | New transition tests and transition flow |
| Menu layout/navigation | [`MenuScreen.cs`](../NuciXNA.Gui/Screens/MenuScreen.cs) | `GuiMenuItem`, `GuiManager.FocusControl` | New MenuScreen cases and behaviour docs |
| Concrete visual control | Owning file under [`Controls/`](../NuciXNA.Gui/Controls/) | Base hook order, owned children, graphics package | Type tests/API/presentation docs |
| Selector semantics | [`GuiMenuListSelector.cs`](../NuciXNA.Gui/Controls/GuiMenuListSelector.cs) and event args | Dictionary ordering, input handlers | Existing selector fixtures plus zero/one/order edges |
| Cursor mode/lifecycle | [`Cursor.cs`](../NuciXNA.Gui/Cursor.cs) | Input subscriptions, graphics sprites, host | New cursor tests/configuration/API docs |
| Package/framework | Project manifests and workflow | Public exposed dependency types, fonts/content | Restore/build/all tests/manual host validation/dependency docs |

## Lifecycle Rules To Preserve

- Derived hooks do not replace public lifecycle validation/order.
- Create/register children inside `DoLoadContent` before base child loading continues.
- Derived concrete-control hooks call base hooks when they depend on base child/events.
- Unsubscribe every external event subscribed during load.
- Keep unload valid for every successfully loaded object.
- Do not register children as manager roots.
- Treat screen unload as ownership of the complete global root registry.

## Public API Changes

Before changing a public member:
- Search consumers/tests and the complete API reference.
- Assess source and binary compatibility, interface implementers, NuGet versioning, and dependency type exposure.
- Prefer additive members when semantics remain coherent.
- Do not rename symbols by documentation-only aliases.
- Update XML comments, API reference, examples, README, and migration/release notes as appropriate.

## Input Changes

Input has two paths: direct `InputManager` subscriptions and manager click polling. A change to one can create duplicates, ordering changes, or gaps in the other. Test:
- Enabled/visible/focused filters.
- Parent-child and overlapping roots.
- Root reverse traversal and handled flag reset.
- Focus transfer and root-only lookup.
- MenuScreen handlers versus focused selector handlers.
- Load/unload subscription balance.

## Transition Changes

Preserve or explicitly revise:
- Constructor timing versus load timing.
- Active request arbitration.
- Old-screen update/draw during cover.
- Overlay draw order and resize behaviour.
- Old disposal/root clear before new load.
- Post-failure state and rollback.

No focused transition tests currently protect these contracts; add them first.

## Documentation Synchronisation

Update:
- [API Reference](./api-reference/INDEX.md) for public/edge semantics.
- Relevant [flow](./flows/) for ordering/causality.
- [Invariants](./invariants.md) for enforced/intended rules.
- [Error Handling](./error-handling.md) for failures/recovery.
- [Architecture](../ARCHITECTURE.md), [Security](../SECURITY.md), or [Privacy](../PRIVACY.md) when boundaries change.
- [INDEX](./INDEX.md) when files are added, removed, or renamed.
