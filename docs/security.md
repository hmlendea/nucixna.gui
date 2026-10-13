# Security Model

This implementation-focused guide complements the root [Security Policy](../SECURITY.md), which defines reporting and support scope.

## System And Trust Boundary

NuciXNA.Gui executes inside the consuming application's process and authority. It does not implement authentication, authorisation, sandboxing, network listeners/clients, persistence, telemetry, secret storage, or cryptography.

Inputs are trusted host/runtime values:
- Screen `Type` values and constructor parameters.
- Text, content identifiers, geometry, colours, effects, and event handlers.
- NuciXNA.Input events/state.
- MonoGame graphics objects and timing.

The library performs limited lifecycle/range checks but does not sanitise displayed text or content identifiers because it is not an HTML/SQL/shell boundary.

## Threat And Risk Table

| Risk | Existing Control | Residual Risk / Host Duty |
|------|------------------|---------------------------|
| Untrusted screen type/constructor | Reflection cast and runtime exceptions | Only pass application-owned types/arguments; no allow-list exists |
| Malicious/invalid content identifier | NuciXNA.Graphics resolution and exception propagation | Restrict identifiers at host boundary; library has no path policy |
| Resource exhaustion | Visibility/enabled filtering and click early exit | Deep/large/cyclic graphs, large text/assets, effects, and repeated loads can exhaust resources |
| Parent cycle | None | Avoid direct cyclic `Parent` assignment; no detection prevents recursion/stack overflow |
| Event-handler failure/re-entrancy | Synchronous .NET events | Keep handlers trusted, bounded, and re-entrancy conscious |
| Lifecycle misuse | Loaded-state exceptions on controls/screens | Managers/cursor have fewer guards; host must serialise/balance calls |
| Supply-chain compromise | Pinned direct package versions in project file | Monitor NuGet/.NET/MonoGame/NuciXNA advisories and verify restore sources |
| Sensitive display data | No persistence/network/logging | Host still controls in-memory text, screenshots, overlays, and any external logging |
| Partial transition failure | Exception propagation | Pending load can fail after old screen disposal; define host recovery |

## Data And Secrets

Production source contains no credential/token/API-key fields and emits no logs or telemetry. Display strings and selector values can nevertheless contain host/user data in memory. The host determines access, lifetime, screenshots/capture, crash-dump exposure, and any persistence/logging.

See the root [Privacy Policy](../PRIVACY.md) and [State and Persistence](./state-and-persistence.md).

## Dependency And Content Security

Direct runtime dependencies are listed in [Dependencies](./dependencies.md). Assets/fonts are resolved through NuciXNA.Graphics. The repository does not validate asset provenance, signature, size, or decoded resource limits. Treat content build inputs and package feeds as supply-chain boundaries.

## Denial Of Service And Robustness

Although there is no remote interface, a consuming application can feed adversarial/unbounded values. Relevant cases include:
- Parent cycles and very deep control trees.
- Large control counts, text, textures, sprite sheets, or effects.
- Repeated cursor/control loads and event subscriptions.
- Event handlers with expensive or recursive work.
- Empty-menu direction input and selector edge exceptions.
- Repeated invalid transitions/reflection construction.

These are robustness concerns whose exploitability depends on whether the host permits untrusted parties to influence those values.

## Reporting

Report suspected vulnerabilities through the private route defined in [`SECURITY.md`](../SECURITY.md). Do not include secrets, personal data, or private host assets in reports. Dependency vulnerabilities should identify the affected package/version and a repository-relevant execution path.

## Security Change Checklist

When adding networking, persistence, logging, user-provided content, scripting, plug-ins, or reflection from untrusted configuration:
1. Define the new trust boundary and threat model.
2. Validate/limit inputs at the owning boundary.
3. Define authentication/authorisation and secret sources where relevant.
4. Define data minimisation, retention, deletion, redaction, and telemetry controls.
5. Define dependency/content provenance and update process.
6. Add abuse/failure tests.
7. Update root security/privacy policies and architecture before release.
