# Contributing to NuciXNA.Gui

This document covers the guidelines and processes for contributing to this project, including how to report issues, suggest enhancements, submit code changes, and follow project standards.

## 📑 Table of Contents

- [How to Contribute](#how-to-contribute)
- [Code of Conduct](#code-of-conduct)
- [Security Policy](#security-policy)
- [License](#license)

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

- .NET SDK 10.0 or later
- Git

#### Development Setup

```bash
# Clone the repository
git clone https://github.com/hmlendea/NuciXNA.Gui.git
cd NuciXNA.Gui

# Restore dependencies
dotnet restore NuciXNA.GUI.slnx
```

#### Making Changes

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes.
4. Run tests: `dotnet test NuciXNA.GUI.slnx`
5. Run build: `dotnet build NuciXNA.GUI.slnx --configuration Release`
6. Commit with clear and descriptive messages.
7. Push to your fork.
8. Open a Pull Request.

#### Pull Request Guidelines

- Target the `dev` branch.
- Keep PRs focused and atomic.
- Update documentation if applicable.
- Add tests for any new functionality.
- Ensure the CI checks pass.

### Code Style

Follow the project's coding standards:
- C# coding conventions per `csharp.coding.instructions.md`
- Run `dotnet format NuciXNA.GUI.slnx` before committing.

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

## 📋 Code of Conduct

This project follows the [Code of Conduct](CODE_OF_CONDUCT.md).

## 🛡️ Security Policy

See [Security Policy](SECURITY.md) for vulnerability reporting.

## 📄 License

This project is licensed under the [MIT License](LICENSE).