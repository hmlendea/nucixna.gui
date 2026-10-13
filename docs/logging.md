# Logging

## Current Status

NuciXNA.Gui has no logging implementation. No logging package, abstraction, console/file writer, telemetry SDK, trace source, metric, or audit sink appears in production source or the project manifest. The library does not catch exceptions to log them.

## Required Concern Matrix

| Concern | Current Behaviour |
|---------|-------------------|
| Logging framework | None; no configuration source |
| Log levels | None; Debug/Information/Warning/Error/Fatal semantics are undefined |
| Structured fields | None; no standard timestamp, control ID, screen ID, user ID, request ID, or exception fields |
| Correlation | None; no correlation ID, trace ID, span ID, activity, or cross-component context |
| Sinks/destinations | None; no console, file, database, event log, or external service |
| Redaction/masking | None because the library emits no logs |
| Retention/rotation/archive | Not applicable; no logs are retained |
| Sampling | None |
| Performance model | No logging overhead; host event handlers can add synchronous overhead |
| Error logging | None; exceptions propagate without stack/context capture by the library |
| Audit logging | None; no separate security event stream |
| Runtime configuration | None; there are no level switches or reload semantics |

## Observable Signals

The library exposes synchronous events and readable state, not logs:
- Lifecycle events before/after load, unload, update, draw, and dispose.
- Property/state/focus/visibility events.
- Input and menu trigger/selection/toggle events.
- `IsContentLoaded`, `IsDisposed`, `IsFocused`, `IsHovered`, `Transitioning`, IDs, geometry, and selection state.

These can support host diagnostics, but subscribing changes runtime work and subscriber exceptions propagate.

## Host Logging Guidance

A consuming application that adds logs should:
- Log at host lifecycle/event boundaries rather than modifying the library solely for output.
- Include stable host correlation/session fields; generated control/screen IDs are runtime-local and caller-mutable.
- Avoid logging arbitrary `GuiText.Text`, selector values, constructor parameters, file paths, or input payloads unless needed and classified.
- Mask secrets/personal data before emission.
- Preserve exception type, stack, inner exception, operation, and relevant non-sensitive control/screen IDs.
- Keep high-frequency update/draw/mouse-move logs sampled or disabled outside diagnosis.
- Use non-blocking sinks only when their threading model remains compatible with host state access.
- Define sink retention, access control, rotation, and deletion in the consuming application's policy.

## Security-Relevant Events

The library has no authentication, authorisation, account, network, or administrative events, so it defines no audit log. Dependency load failures, reflection failures, invalid lifecycle calls, or repeated transition requests may be operationally relevant to a host but are not security audit events by default.

## Adding Library Logging

Before adding a logging dependency or API:
1. Define whether logging belongs in this library or the host.
2. Specify framework, levels, event IDs, fields, correlation source, sinks, and configuration ownership.
3. Define redaction and prohibited fields.
4. Define exception and audit semantics.
5. Benchmark high-frequency paths and test disabled-level allocation.
6. Update [Privacy](../PRIVACY.md), [Security](../SECURITY.md), [Architecture](../ARCHITECTURE.md), configuration, dependencies, and tests.
