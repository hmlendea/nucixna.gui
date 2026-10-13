# Error Handling

## Strategy

NuciXNA.Gui is predominantly fail-fast and exception-transparent. Base control/screen lifecycles explicitly reject invalid loaded state. Managers and concrete controls generally permit reflection, dictionary, graphics, content, event-handler, and argument exceptions to propagate. There is no global exception handler, result type, retry, fallback screen, logger, or rollback transaction.

## Exception Taxonomy

| Source | Condition | Observable Result |
|--------|-----------|-------------------|
| `GuiControl.LoadContent` / `Screen.LoadContent` | Already loaded | `InvalidOperationException` |
| Control/screen unload/update/draw | Not loaded | `InvalidOperationException` |
| `GuiManager.RegisterControl(s)` | Duplicate/null ID or null control | Dictionary/dereference exception |
| `ScreenManager.LoadContent` | Null/invalid starting type or constructor | Reflection/cast exception |
| `ScreenManager.ChangeScreens` | Invalid target or constructor arguments | Reflection/cast/constructor exception |
| `GuiMenuListSelector.SelectItemByIndex` | Different index outside range | `IndexOutOfRangeException` |
| Selector key/value lookup | Not found | `ArgumentOutOfRangeException` |
| Graphics/content wrappers | Missing/invalid asset/device state | Dependency exception |
| Event subscribers/derived hooks | Handler/hook throws | Exception propagates to input/lifecycle caller |

## Suppressed And Silent Cases

- `TrySelectItem(int)` suppresses only `IndexOutOfRangeException`.
- `TrySelectItem(string)` selects by value and suppresses only `ArgumentOutOfRangeException`.
- `ChangeScreens` returns silently while `Transitioning` is true.
- Repeated toggle assignment to the existing state is a no-op.
- Selector assignment to the current index/key/value returns before validation/event delivery.
- Repeated disposal returns after the `IsDisposed` guard.

## Partial-Failure Semantics

### Control Load

Input subscriptions occur before `DoLoadContent` and child loading. If a hook/child fails, `IsContentLoaded` remains false but subscriptions or partially loaded resources can remain. A later `UnloadContent()` is rejected because the flag is false, so recovery is host/derived-type specific.

### Screen Load

Screen input subscriptions and derived `DoLoadContent` occur before `GuiManager.LoadContent`. A root failure can leave preceding roots loaded while screen loaded state remains false. No rollback unload is attempted.

### Manager Load/Unload

Manager loops have no transaction. One root can throw after earlier roots were processed. Registry clear in unload occurs only after all unload calls complete.

### Transition Swap

At opaque, old-screen disposal precedes pending assignment/load. A pending load failure occurs after old state has been destroyed; `Transitioning` cleanup is not protected by `finally`.

### Cursor Mode

Cursor has no loaded guard. Repeated loads or changing mode while loaded can leave mismatched sprite references/subscriptions; unload chooses unsubscription from the current `ContentFile` value.

## Host Responsibilities

- Validate screen types/constructor arguments before transition requests.
- Validate unique root IDs before registration.
- Ensure required content identifiers resolve.
- Serialise lifecycle calls on the host's appropriate thread.
- Catch/report exceptions at host lifecycle boundaries if continued execution is possible.
- Avoid logging secrets or arbitrary displayed text without a deliberate redaction policy.
- Define recovery explicitly; do not assume managers restore prior state.

## Change Guidance

When adding recovery:
1. Define the owner of rollback and the valid post-failure state.
2. Unsubscribe events and unload partial resources safely.
3. Preserve the original exception (including inner exception) when wrapping adds useful context.
4. Do not include sensitive text, paths, or host state unnecessarily in messages.
5. Add tests for failures at every ordered lifecycle stage.
6. Update [Logging](./logging.md), [Troubleshooting](./troubleshooting.md), and flow documentation.

## Test Gaps

Current tests cover several lifecycle misuse and selector exceptions, but not partial child/root loading, event-handler failures, reflection failures, graphics/content errors, transition rollback, cursor repeated loading, or concurrent lifecycle calls.
