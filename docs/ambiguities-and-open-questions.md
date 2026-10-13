# Ambiguities And Open Questions

This ledger records explicit TODOs, implementation contradictions, and consequential gaps. It does not assert proposed answers.

## Explicit Source TODOs

| Location | TODO | Impact |
|----------|------|--------|
| [`GuiControl.Parent`](../NuciXNA.Gui/Controls/GuiControl.cs) and [`IGuiControl.Parent`](../NuciXNA.Gui/Controls/IGuiControl.cs) | Parent should not be public | Direct assignment bypasses private child ownership and permits cycles |
| [`GuiMenuItem.Triggered`](../NuciXNA.Gui/Controls/GuiMenuItem.cs) | Consider custom handler/arguments | Current event conveys no activation source/context |
| [`GuiMenuItem.OnMouseEntered`](../NuciXNA.Gui/Controls/GuiMenuItem.cs) | Play selection sound | Audio ownership/configuration is undefined and absent |

## Public Surface Questions

| Observation | Open Question |
|-------------|---------------|
| `GuiManager`/`ScreenManager` expose public constructors while production code hardcodes static `Instance` | Are alternate instances supported, or should constructors be restricted? |
| `IGuiControl.Parent` is concrete `GuiControl`, and child registration casts the interface | Is independent `IGuiControl` implementation intended? |
| Generic screen APIs constrain `IScreen`, but reflected creation casts to `Screen` | Should the API support independent `IScreen`, or tighten constraints/signatures? |
| `ScreenManager.SpriteBatch` is public but unused | Is it legacy API, future state, or removable in a breaking release? |
| `GuiManager.DefaultMargins` is public but unused | Should `GuiText` consume it, or should it be removed/deprecated? |
| IDs are public/mutable after dictionary registration | Should identity become immutable or manager re-keying be supported? |

## Lifecycle And Failure Questions

| Observation | Open Question |
|-------------|---------------|
| Load hooks/events subscribe before loaded flag and have no rollback | What valid state/recovery should follow partial load? |
| Manager unload unloads roots but does not dispose them | Is unload-only intentional ownership, or should roots be disposed? |
| Screen transition disposes old before pending load; failure leaves partial transition | What rollback/fallback screen policy is intended? |
| `ScreenManager.UnloadContent` has no loaded/null guard and retains fields | Is manager reuse after unload supported? |
| Cursor has no loaded guard; mode changes use current mode for unsubscription | Should it have explicit lifecycle/mode state? |
| Finalisers call the public `Dispose()`/managed cleanup path | Is finalisation required for these managed wrappers? |

## Behaviour Questions

| Observation | Open Question |
|-------------|---------------|
| `GuiText` draws `TextSprite` in `DoDraw`, then child background afterwards | Is background-over-text order intentional? |
| Disabled roots still draw; disabled children do not | Is this asymmetry intentional? |
| Root click polling is reverse dictionary snapshot, drawing is forward enumeration, no order API | Is dictionary order intended as z-order? |
| `FocusControl` indexes roots only; child polling can request child focus | Should descendants be focusable/indexed? |
| Direct mouse-button events and manager click polling are separate paths | What once-per-click guarantee is intended? |
| Empty MenuScreen direction can index `-1` | Should empty menus ignore navigation? |
| Empty selector next throws while previous can no-op | What symmetric empty-state contract is intended? |
| Horizontal MenuScreen direction overlaps focused selector direction | Should one handler consume/prevent the other? |
| Screen clear colour is cached only at load | Should runtime background changes affect draw immediately? |
| Tooltip clamps against size not refreshed during transition | Should resize continue while transitioning? |
| Restore resolves vulnerable `SixLabors.ImageSharp 3.1.12` through NuciXNA.Graphics -> NuciXNA.DataAccess | Which compatible upstream package update removes the advisories, and what rendering/content regressions must be verified? |

## Missing Verification

Focused tests are absent for:
- `GuiManager` registration/focus/input/draw ordering.
- `ScreenManager` reflection/transitions/failures/resizing.
- `MenuScreen` layout/navigation boundaries.
- `Cursor` modes/subscriptions/lifecycle.
- `GuiText`, `GuiImage`, and `GuiTooltip` composition/rendering.
- Partial lifecycle failures, event-handler exceptions, parent cycles, and concurrency.

## Resolution Process

For each item:
1. Confirm intended public contract with maintainers.
2. Add a failing characterisation or desired-behaviour test.
3. Implement the smallest coherent change.
4. Assess compatibility/versioning.
5. Update API reference, flows, invariants, troubleshooting, and release notes.
6. Remove or revise this ledger entry only after evidence exists.
