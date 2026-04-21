[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucixna.gui)](https://github.com/hmlendea/nucixna.gui/releases/latest) [![Build Status](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucixna.gui/actions/workflows/dotnet.yml) [![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# NuciXNA.Gui

GUI management and basic widgets for the NuciXNA wrapper over MonoGame/XNA.

## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciXNA.Gui)

### .NET CLI

```bash
dotnet add package NuciXNA.Gui
```

### Package Manager

```powershell
Install-Package NuciXNA.Gui
```

## Requirements

- .NET: `net10.0`
- MonoGame DesktopGL (or compatible MonoGame runtime)
- `NuciXNA.Graphics`, `NuciXNA.Input` and `NuciXNA.Primitives` (restored automatically via NuGet)

## Development

### Build

```bash
dotnet build NuciXNA.Gui.sln
```

### Test

```bash
dotnet test NuciXNA.Gui.sln
```

### Pack

```bash
dotnet pack NuciXNA.Gui/NuciXNA.Gui.csproj -c Release
```

## Contributing

Contributions are welcome.

When contributing:

- keep the project cross-platform
- preserve the existing public API unless a breaking change is intentional
- keep the changes focused and consistent with the current coding style
- update the documentation when the behavior changes
- include tests for any new behavior

## Related Projects

- [NuciXNA.DataAccess](https://github.com/hmlendea/nucixna.dataaccess)
- [NuciXNA.Graphics](https://github.com/hmlendea/nucixna.graphics)
- [NuciXNA.GUI](https://github.com/hmlendea/nucixna.gui)
- [NuciXNA.Input](https://github.com/hmlendea/nucixna.input)
- [NuciXNA.Primitives](https://github.com/hmlendea/nucixna.Primitives)

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.