# Contributing to NuciXNA.Gui

This document covers the guidelines and processes for contributing to this project, including how to report issues, suggest enhancements, submit code changes, and follow project standards.

## 📑 Table of Contents

- [How to Contribute](#how-to-contribute)
  - [Reporting Issues](#reporting-issues)
  - [Suggesting Enhancements](#suggesting-enhancements)
  - [Code Contributions](#code-contributions)
    - [Prerequisites](#prerequisites)
    - [Development Setup](#development-setup)
    - [Making Changes](#making-changes)
    - [Pull Request Guidelines](#pull-request-guidelines)
  - [Testing](#testing)
  - [Documentation](#documentation)
- [Security](#security)
- [License](#license)
- [Getting Help](#getting-help)

## 🤝 How to Contribute

### Reporting Issues

- Search existing issues first.
- Use the issue templates if available.
- Provide clear reproduction steps.
- Include environment details.

### Suggesting Enhancements

- Check the roadmap and existing discussions.
- Explain the use case and expected behavior.
- Consider implementation complexity.

### Code Contributions

#### Prerequisites

- .NET 10 SDK
- Git

#### Development Setup

```bash
# Clone the repository
git clone https://github.com/hmlendea/NuciXNA.Gui.git
cd NuciXNA.Gui

# Install dependencies
dotnet restore NuciXNA.GUI.slnx
```

#### Making Changes

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes.
4. Run tests: `dotnet test NuciXNA.GUI.slnx`
5. Commit with clear and descriptive messages.
6. Push to your fork.
7. Open a Pull Request.

#### Pull Request Guidelines

- Target the `master` branch.
- Keep PRs focused and atomic.
- Update documentation if applicable.
- Add tests for any new functionality.
- Ensure the CI checks pass.

### Testing

```bash
# Run all tests
dotnet test NuciXNA.GUI.slnx

# Run specific test project
dotnet test NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj
```

### Documentation

- Update relevant docs for changes.
- Follow the documentation style guide.
- Preview changes locally if possible.

## 🔒 Security

Report security vulnerabilities per the [Security Policy](./SECURITY.md).

## 📄 License

By contributing, you agree that your contributions will be licensed under the GNU General Public License v3.0 or later.

## ❓ Getting Help

- Use the [GitHub issue tracker](https://github.com/hmlendea/nucixna.gui/issues).
- Check existing issues and [documentation](./docs/INDEX.md) first.