# Control Interaction

## Effective Availability

A control can interact only when effectively enabled and visible. Parent disabled/hidden state gates descendants. `Enable`/`Disable` and `Show`/`Hide` change local flags and synchronously raise their matching events.

Root drawing differs: `GuiManager.Draw` checks loaded and visible, not root enabled state. A disabled root can remain visible while its update/input stops; disabled descendants do not draw.

## Pointer Movement

Loaded controls subscribe to `InputManager.MouseMoved`:
- Movement unchanged or entirely outside both previous/current bounds is ignored.
- Relevant movement raises `MouseMoved`.
- Entering raises `MouseEntered` and sets `IsHovered` true.
- Moving within keeps `IsHovered` true.
- Leaving raises `MouseLeft` and sets `IsHovered` false.

Hover is event-driven. Moving/resizing a control under a stationary pointer does not recalculate it.

## Clicking And Focus

There are two paths:
1. Direct `MouseButtonPressed` events raise `MouseButtonPressed`; left button also raises `Clicked` and sets the shared handled flag.
2. `GuiManager.Update` polls left clicks across roots in reverse snapshot order. An eligible control is focused, raises `Clicked`, and sets the flag.

Within a root, polling tests the parent before children. `FocusControl` indexes roots only; a child focus request cannot be resolved by ID.

## Keyboard And Gamepad

Loaded controls receive held/pressed/released events only when enabled, visible, and focused. Base controls expose the events without interpreting them. `GuiMenuItem` interprets Enter/`E` and gamepad A as `Triggered`; selectors interpret direction buttons/keys.

## Control State Outcomes

### Toggle

`SetState`, `SwitchOn`, `SwitchOff`, or `SwitchState` update `IsOn`. Only a genuine change raises `StateChanged`.

### List Selector

Replacing a non-empty item collection automatically selects the first entry. Selection events expose mutable selected index/key/value. Next/previous wraps except one-item calls are no-ops. Empty next throws; empty previous can no-op.

### Link

Trigger constructs the pending target immediately. The old screen remains active under the black overlay; the target loads only at the opaque swap.

### Tooltip

Starts hidden. When shown/updated, it shifts its local location to keep its display rectangle within cached screen size where possible.

### Cursor

The host draws a separately constructed cursor. Paired mode switches between idle/click sprites from left-button events; custom mode uses one sprite when selected before load.

## Lifecycle Effects

Input subscriptions exist only between successful load and unload. Screen transition disposal unloads the old screen, which unloads and clears all roots. A retained external control reference does not remain active after that registry/lifecycle transition.

## Unsupported Interactions

The base control contract has no mouse-release event, drag/capture, double-click, wheel, text-input, clipboard, accessibility, tab-order, clipping, or gesture abstraction. Hosts must add these deliberately and define focus/ownership/event-order semantics.

## Related Documentation

- [Input Handling](../flows/input-handling.md)
- [Presentation Components](../components/presentation.md)
- [API Reference](../api-reference/INDEX.md)
- [Troubleshooting](../troubleshooting.md)
