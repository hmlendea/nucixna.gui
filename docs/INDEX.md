# NuciXNA.Gui Documentation

## Repository Summary

NuciXNA.Gui is a .NET 10 library of retained GUI controls, menu navigation, cursor rendering, and screen lifecycle utilities for MonoGame/NuciXNA hosts. `GuiManager` coordinates registered root controls, `ScreenManager` coordinates the active screen and fade overlay, and the host supplies synchronous update/draw cadence; the repository defines no frame-rate guarantee, persistence, networking, HTTP API, or logging service.

## Root Document Map

- [ARCHITECTURE.md](../ARCHITECTURE.md) — High-level architecture overview
- [SECURITY.md](../SECURITY.md) — Security vulnerability reporting policy
- [PRIVACY.md](../PRIVACY.md) — Privacy and personal data handling
- [LICENSE](../LICENSE) — GPL-3.0-or-later license terms
- [CONTRIBUTING.md](../CONTRIBUTING.md) — Contribution guidelines
- [README.md](../README.md) — Project overview and usage examples

## Documentation Catalogue

### Master Index
- [INDEX.md](./INDEX.md) - Repository summary, complete catalogue, navigation, and maintenance metadata

### Architecture & Design
- [architecture.md](./architecture.md) — Detailed architecture complementing root ARCHITECTURE.md
- [design-decisions.md](./design-decisions.md) — Key architectural/design choices and rationale
- [components/presentation.md](./components/presentation.md) — GUI controls and rendering deep dive
- [components/host-and-composition.md](./components/host-and-composition.md) — GuiManager, ScreenManager, and composition root
- [components/application-services.md](./components/application-services.md) — Application-facing services and contracts

### Execution Flows
- [flows/startup-and-rendering.md](./flows/startup-and-rendering.md) — Application startup, game loop, and rendering pipeline
- [flows/screen-transitions.md](./flows/screen-transitions.md) — Screen change flow with transitions
- [flows/input-handling.md](./flows/input-handling.md) — Input processing from MonoGame to controls

### User-Facing Behaviour
- [behaviour/menu-navigation.md](./behaviour/menu-navigation.md) - Menu layout, focus, direction mapping, and activation
- [behaviour/control-interaction.md](./behaviour/control-interaction.md) - Pointer, focus, keyboard/gamepad, stateful controls, and unsupported gestures

### Data & State
- [data-model.md](./data-model.md) — Domain entities, relationships, and in-memory state
- [state-and-persistence.md](./state-and-persistence.md) — State management, transient nature, no persistence

### External Interactions
- [dependencies.md](./dependencies.md) — External and internal dependencies
- [integrations.md](./integrations.md) — External system integrations (MonoGame, NuciXNA.* packages)

### Cross-Cutting Concerns
- [error-handling.md](./error-handling.md) — Error taxonomy, handling patterns, recovery
- [concurrency-and-scheduling.md](./concurrency-and-scheduling.md) — Threading model, game loop, async considerations
- [logging.md](./logging.md) — Logging framework (none built-in), observability via events
- [security.md](./security.md) — Security model, threats, mitigations (complements root SECURITY.md)
- [invariants.md](./invariants.md) — System-wide invariants and contracts

### Development & Operations
- [build-and-deployment.md](./build-and-deployment.md) — Build pipeline, NuGet packaging, CI/CD
- [configuration.md](./configuration.md) — Programmatic configuration, no external config files
- [testing.md](./testing.md) — Test strategy, organisation, coverage (NUnit, unit tests)
- [change-guide.md](./change-guide.md) — How to modify common areas safely
- [documentation-maintenance.md](./documentation-maintenance.md) — How to keep docs current, ownership

### Reference & Support
- [api-reference/INDEX.md](./api-reference/INDEX.md) — API reference index
- [api-reference/GuiManager.md](./api-reference/GuiManager.md) — GuiManager API
- [api-reference/GuiControl.md](./api-reference/GuiControl.md) — GuiControl base class API
- [api-reference/ScreenManager.md](./api-reference/ScreenManager.md) — ScreenManager API
- [api-reference/Screen.md](./api-reference/Screen.md) — Screen base class API
- [api-reference/MenuScreen.md](./api-reference/MenuScreen.md) - MenuScreen and MenuScreenAxis API
- [api-reference/Cursor.md](./api-reference/Cursor.md) - Host-owned cursor API
- [api-reference/Interfaces.md](./api-reference/Interfaces.md) - IGuiControl and IScreen contracts
- [api-reference/Controls/GuiText.md](./api-reference/Controls/GuiText.md) — GuiText control API
- [api-reference/Controls/GuiImage.md](./api-reference/Controls/GuiImage.md) — GuiImage control API
- [api-reference/Controls/GuiMenuItem.md](./api-reference/Controls/GuiMenuItem.md) — GuiMenuItem control API
- [api-reference/Controls/GuiMenuText.md](./api-reference/Controls/GuiMenuText.md) - GuiMenuText label API
- [api-reference/Controls/GuiMenuLink.md](./api-reference/Controls/GuiMenuLink.md) — GuiMenuLink control API
- [api-reference/Controls/GuiMenuToggle.md](./api-reference/Controls/GuiMenuToggle.md) — GuiMenuToggle control API
- [api-reference/Controls/GuiMenuListSelector.md](./api-reference/Controls/GuiMenuListSelector.md) — GuiMenuListSelector control API
- [api-reference/Controls/GuiTooltip.md](./api-reference/Controls/GuiTooltip.md) — GuiTooltip control API
- [api-reference/Controls/ListSelectionEventArgs.md](./api-reference/Controls/ListSelectionEventArgs.md) - Selector event payload API
- [api-usage-examples.md](./api-usage-examples.md) — Example usage patterns
- [repository-overview.md](./repository-overview.md) — Repository purpose, scope, entry points
- [repository-structure.md](./repository-structure.md) — Source tree layout, module organisation
- [quick-start.md](./quick-start.md) — Getting started guide for newcomers
- [faq.md](./faq.md) — Frequently asked questions
- [troubleshooting.md](./troubleshooting.md) — Common issues and solutions
- [ambiguities-and-open-questions.md](./ambiguities-and-open-questions.md) — Unresolved items, TODOs, known gaps

## Navigation Aids

- **Start here (newcomers):** [quick-start.md](./quick-start.md) → [repository-overview.md](./repository-overview.md) → [architecture.md](./architecture.md)
- **Deep dive (component owners):** [components/presentation.md](./components/presentation.md) → [components/host-and-composition.md](./components/host-and-composition.md) → [api-reference/INDEX.md](./api-reference/INDEX.md)
- **Flows (debuggers):** [flows/startup-and-rendering.md](./flows/startup-and-rendering.md) → [flows/input-handling.md](./flows/input-handling.md) → [flows/screen-transitions.md](./flows/screen-transitions.md)

## Maintenance Metadata

- **Last reviewed:** 2026-10-13
- **Owner:** Horațiu Mlendea
- **Coverage status:** Comprehensive - 49 documentation files catalogued; links, current API names, SLNX, Release build, and 409 tests validated