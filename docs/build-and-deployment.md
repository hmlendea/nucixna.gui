# Build And Deployment

## Build Artefacts

NuciXNA.Gui is a class library, not a standalone process. Build output is a .NET assembly; packaging can produce a NuGet package. There is no application deployment, container, installer, infrastructure, database migration, service health check, or runtime environment configuration in this repository.

## Prerequisites

- .NET 10 SDK compatible with `net10.0`.
- NuGet access to the packages in [`NuciXNA.Gui.csproj`](../NuciXNA.Gui/NuciXNA.Gui.csproj) and the test project.
- On the Ubuntu CI image, Microsoft core fonts are installed before build/test.

## Local Commands

```bash
dotnet restore NuciXNA.GUI.slnx
dotnet build NuciXNA.GUI.slnx --configuration Release --no-restore
dotnet test NuciXNA.GUI.slnx --configuration Release --no-build
dotnet pack NuciXNA.Gui/NuciXNA.Gui.csproj --configuration Release --no-build
```

Omit `--no-restore`/`--no-build` when the prerequisite step has not run. Debug and Release are the solution's ordinary SDK configurations.

## Solution And Projects

[`NuciXNA.GUI.slnx`](../NuciXNA.GUI.slnx) contains:

| Project | Output | Packable |
|---------|--------|----------|
| [`NuciXNA.Gui`](../NuciXNA.Gui/NuciXNA.Gui.csproj) | Library assembly and NuGet package | Yes (SDK default) |
| [`NuciXNA.Gui.UnitTests`](../NuciXNA.Gui.UnitTests/NuciXNA.Gui.UnitTests.csproj) | NUnit test assembly | No (`IsPackable=false`) |

## Package Metadata

The library manifest currently declares:

| Field | Value |
|-------|-------|
| Package/version | `NuciXNA.Gui` / `3.4.2` |
| Target framework | `net10.0` |
| Description | GUI management and basic widgets for NuciXNA |
| Repository | `https://github.com/hmlendea/nucixna.gui` |
| Licence expression | `GPL-3.0-or-later` |
| Tags | `XNA MonoGame` |

The repository contains no release script, signing configuration, symbol/source package configuration, changelog automation, or NuGet publication workflow. `dotnet pack` builds an artefact; publication is a separate maintainer action not defined here.

## Continuous Integration

[`.github/workflows/dotnet.yml`](../.github/workflows/dotnet.yml) runs for pushes and pull requests targeting `master` on `ubuntu-latest`:
1. `actions/checkout@v6`.
2. `actions/setup-dotnet@v5` with `10.0.x`.
3. Accept/install `ttf-mscorefonts-installer` and rebuild font cache.
4. `dotnet restore`.
5. `dotnet build --no-restore`.
6. `dotnet test --no-build --verbosity normal`.

CI does not pack, publish, upload artefacts, collect coverage, scan dependencies, sign packages, or create releases.

## Consumer Deployment

A consuming MonoGame application deploys the library and transitive runtime dependencies with its own artefacts/content. It must also provide conventional content identifiers such as `ScreenManager/FillImage`, configured fonts, and cursor assets when those code paths are used. Platform/runtime packaging belongs to that host.

## Release Checklist

Before manually publishing a package:
1. Update version/package metadata deliberately.
2. Run restore, Release build, and all tests from a clean checkout.
3. Inspect package contents and dependency versions.
4. Verify public API and documentation compatibility.
5. Exercise content loading/rendering/input/transitions in a representative host because unit tests do not cover them.
6. Review licence, security advisories, and release notes.
7. Publish through the maintainer's authorised NuGet process; credentials must never enter repository files or logs.

## Failure Diagnosis

- Restore failure: inspect NuGet source reachability and exact package/version availability.
- Linux font/test failure: reproduce CI font installation.
- Build failure after dependency update: inspect public dependency API changes.
- Pack failure: build Release first or remove `--no-build`.
- Host runtime content failure: verify content identifiers and NuciXNA.Graphics setup; the package does not embed the host's game assets.
