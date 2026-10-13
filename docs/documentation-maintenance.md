# Documentation Maintenance

## Ownership And Status

- Repository maintainers own correctness.
- `docs/INDEX.md` records last review date, owner, and coverage status.
- Documentation describes verified current behaviour unless a section is explicitly labelled proposed/unknown.
- Source and executable tests outrank stale prose when they disagree; correct the prose in the same change.

## Document Boundaries

| Document | Canonical Scope |
|----------|-----------------|
| Root [`README.md`](../README.md) | User orientation, installation, concise usage, links |
| Root [`ARCHITECTURE.md`](../ARCHITECTURE.md) | High-level current architecture |
| Root [`SECURITY.md`](../SECURITY.md) | Supported versions and private vulnerability reporting policy |
| Root [`PRIVACY.md`](../PRIVACY.md) | Data handling policy |
| Root [`CONTRIBUTING.md`](../CONTRIBUTING.md) | Contribution process |
| [`docs/architecture.md`](./architecture.md) | Detailed architecture mechanics complementing root architecture |
| [`docs/api-reference/`](./api-reference/) | Exact public .NET contracts and edge semantics |
| [`docs/flows/`](./flows/) | Ordered causal execution paths |
| Concern guides | Data, errors, logging, security, tests, build, and change impact |

Do not duplicate root policies under `docs`; link them and add implementation detail only. This library has no HTTP controllers, so the API index records non-applicability and documents .NET types rather than inventing endpoints.

## Update Triggers

| Source Change | Documentation To Review |
|---------------|-------------------------|
| Public type/member/default/event | API reference, README usage, configuration, examples |
| Lifecycle/order/ownership | Architecture, flow, invariants, change guide |
| Failure/recovery | Error handling, troubleshooting, API reference |
| Dependency/TFM/workflow/package | Dependencies, build/deployment, README, contributing |
| Input/navigation | Input flow, behaviour, relevant control refs |
| Data persistence/network/logging/telemetry | Privacy, security, logging, state, architecture |
| New test/gap closure | Testing and API `Tests And Change Impact` sections |
| New/renamed documentation | INDEX catalogue and cross-links |

## Evidence Standard

For material claims, inspect:
1. Owning implementation.
2. Callers and callees across abstraction boundaries.
3. Public interfaces/models.
4. Configuration/manifests/workflows.
5. Relevant tests.
6. Contradictory paths and TODOs.

Label facts as implemented, inferred host responsibility, absent, or unknown. Never infer methods, defaults, performance numbers, support guarantees, recovery, or security controls from familiar framework patterns.

## Style

- Use kebab-case file/directory names except required `INDEX.md` and root policy names.
- Use title-case headings in accordance with repository instructions.
- Use British spelling.
- Wrap fully qualified code symbols in backticks when ambiguity matters.
- Use repository-relative links and verify every target.
- Prefer Mermaid for causal/topology diagrams, tables for contracts, and concise prose for rationale.
- Avoid invented examples that call non-existent APIs.

## Verification Checklist

Before completing a documentation change:
- Every catalogued file exists, and every file appears in `docs/INDEX.md`.
- Relative links resolve, including case-sensitive paths.
- Public names/signatures/defaults match source.
- Mermaid fences are balanced and diagrams match prose.
- No placeholder/template comments remain.
- No unsupported timing, security, compatibility, or test-result claim remains.
- Build/test commands use `NuciXNA.GUI.slnx`.
- Root documents remain distinct and mutually linked where relevant.
- Review date/coverage status are updated only after validation.

## Review Cadence

Review documentation with every behavioural/public/build change. In addition, perform a periodic repository-wide review after dependency/framework upgrades or before a release. There is no generated-doc pipeline, so drift prevention depends on contributor review and executable validation.
