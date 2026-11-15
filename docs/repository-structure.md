# Repository Structure

## Source Tree Layout

```
NuciXNA.Gui/                          # Main library project
├── Controls/                         # GUI control implementations
│   ├── GuiControl.cs                 # Abstract base class for all controls
│   ├── IGuiControl.cs                # Control interface definition
│   ├── GuiText.cs                    # Text display control with formatting
│   ├── GuiImage.cs                   # Image/sprite display control
│   ├── GuiMenuItem.cs                # Base menu item with selection/focus
│   ├── GuiMenuText.cs                # Simple text menu item
│   ├── GuiMenuLink.cs                # Menu item that navigates to a screen
│   ├── GuiMenuToggle.cs              # Boolean toggle menu item
│   ├── GuiMenuListSelector.cs        # Cyclic list selector menu item
│   ├── GuiTooltip.cs                 # Tooltip control with border/background
│   └── ListSelectionEventArgs.cs     # Event args for list selection changes
├── Screens/                          # Screen management
│   ├── IScreen.cs                    # Screen interface
│   ├── Screen.cs                     # Abstract base class for screens
│   ├── MenuScreen.cs                 # Menu-oriented screen with navigation
│   ├── MenuScreenAxis.cs             # Enum: Vertical | Horizontal
│   └── ScreenManager.cs              # Singleton screen lifecycle manager
├── Cursor.cs                         # Custom cursor with idle/click states
├── GuiManager.cs                     # Singleton GUI control manager
└── NuciXNA.Gui.csproj                # Project file (net10.0)

NuciXNA.Gui.UnitTests/                # Unit test project
├── Controls/                         # Control tests
│   ├── ControlTests.cs               # Basic control tests
│   ├── GuiControlLifecycleTests.cs   # LoadContent/UnloadContent/Dispose tests
│   ├── GuiControlPropertyTests.cs    # Property inheritance and events tests
│   ├── GuiMenuItemTests.cs           # GuiMenuItem property tests
│   ├── GuiMenuItemExtendedTests.cs   # GuiMenuItem behavior tests
│   ├── GuiMenuLinkTests.cs           # GuiMenuLink property tests
│   ├── GuiMenuLinkExtendedTests.cs   # GuiMenuLink behavior tests
│   ├── GuiMenuListSelectorTests.cs   # GuiMenuListSelector property tests
│   ├── GuiMenuListSelectorExtendedTests.cs # GuiMenuListSelector behavior tests
│   ├── GuiMenuTextTests.cs           # GuiMenuText tests
│   ├── GuiMenuToggleTests.cs         # GuiMenuToggle property tests
│   ├── GuiMenuToggleExtendedTests.cs # GuiMenuToggle behavior tests
│   └── ListSelectionEventArgsTests.cs # ListSelectionEventArgs tests
├── Screens/                          # Screen tests
│   ├── ScreenTests.cs                # Screen base class tests
│   └── ScreenExtendedTests.cs        # Screen behavior tests
├── Helpers/                          # Test utilities
│   ├── DummyControl.cs               # Minimal GuiControl implementation
│   └── DummyScreen.cs                # Minimal Screen implementation
└── NuciXNA.Gui.UnitTests.csproj      # Test project file (net10.0)

NuciXNA.Gui.ArchitectureTests/        # Architecture/fitness test project
├── ArchitectureResources.cs          # Embedded production, test, and project sources
├── ProductionArchitecture.cs         # Reusable ArchUnitNET production model
├── SourceParsing.cs                  # Shared Roslyn parsing helpers
├── DependencyRuleTests.cs            # Assemblies, namespaces, references, public surface
├── DependencyPinConventionTests.cs   # Exact package versions, references, non-vacuity guards
├── ProjectConventionTests.cs         # Project properties, packages, and references
├── SolutionConventionTests.cs        # SDK-style solution structure and order
├── SourceFileConventionTests.cs      # Files, namespaces, syntax, using directives
├── LayoutAndApiConventionTests.cs    # Namespace/folder mapping, layout, collections, contracts
├── TypeDeclarationConventionTests.cs # Contracts, colocation, XML documentation
├── TypeSurfaceConventionTests.cs     # Enumerations, interface docs, static state, suite self-checks
├── MemberOrganisationTests.cs        # Accessibility, field/member order, formatting
├── NamingConventionTests.cs          # Type, method, field, and fixture naming
├── FrameworkUsageConventionTests.cs  # MonoGame/NuciXNA wrappers, async, dummies, lifecycle ownership
├── TestSourceConventionTests.cs      # Fixture attributes, Given/When/Then names, reflection, case counts
└── NuciXNA.Gui.ArchitectureTests.csproj # Architecture project file (net10.0)

.github/
└── workflows/
    ├── dotnet.yml                    # CI pipeline (build, test)
    ├── security.yml                  # Dependency vulnerability check and CodeQL analysis
    ├── github-release.yml            # Package upload to the GitHub release
    └── nuget-release.yml             # Package publication to NuGet.org

Root files:
├── ARCHITECTURE.md                   # Architecture documentation
├── CONTRIBUTING.md                   # Contribution guidelines
├── LICENSE                           # GPL-3.0-or-later license
├── NuciXNA.GUI.slnx                  # Solution file (SLNX format)
├── PRIVACY.md                        # Privacy policy
├── README.md                         # Project overview
├── SECURITY.md                       # Security policy
└── docs/                             # Detailed documentation (this directory)
```

## Module Organisation

### NuciXNA.Gui (Main Library)

| Module | Responsibility | Key Types |
|--------|----------------|-----------|
| **Controls** | Reusable GUI widgets | `GuiControl`, `GuiText`, `GuiImage`, `GuiMenuItem`, `GuiMenuLink`, `GuiMenuToggle`, `GuiMenuListSelector`, `GuiTooltip` |
| **Screens** | Game screen lifecycle and transitions | `Screen`, `MenuScreen`, `ScreenManager`, `IScreen` |
| **Managers** | Singleton coordination | `GuiManager`, `ScreenManager` |
| **Cursor** | Custom cursor rendering | `Cursor` |

### NuciXNA.Gui.UnitTests (Test Project)

| Module | Responsibility | Key Types |
|--------|----------------|-----------|
| **Controls** | Unit tests for control behavior | `GuiControlLifecycleTests`, `GuiControlPropertyTests`, `GuiMenuItemTests`, etc. |
| **Screens** | Unit tests for screen behavior | `ScreenTests`, `ScreenExtendedTests` |
| **Helpers** | Test doubles and utilities | `DummyControl`, `DummyScreen` |

### NuciXNA.Gui.ArchitectureTests (Architecture/Fitness Test Project)

| Module | Responsibility | Key Types |
|--------|----------------|-----------|
| **Dependencies** | Assembly, namespace, reference, and public-surface rules over the loaded ArchUnitNET model | `DependencyRuleTests`, `ProductionArchitecture` |
| **Project/Solution** | Project properties, package ordering, pinned versions, references, and solution structure | `ProjectConventionTests`, `SolutionConventionTests`, `DependencyPinConventionTests` |
| **Source Conventions** | Roslyn rules for files, namespaces, syntax, using directives, contracts, documentation, member organisation, naming, layout, and framework usage | `SourceFileConventionTests`, `LayoutAndApiConventionTests`, `TypeDeclarationConventionTests`, `TypeSurfaceConventionTests`, `MemberOrganisationTests`, `NamingConventionTests`, `FrameworkUsageConventionTests` |
| **Test Conventions** | Fixture attributes, test naming, reflection bans, case counts, assertion form | `TestSourceConventionTests` |
| **Shared** | Embedded-source access and Roslyn parsing helpers | `ArchitectureResources`, `SourceParsing` |

## Project Files

### NuciXNA.Gui.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <RootNamespace>NuciXNA.Gui</RootNamespace>
    <Version>3.4.2</Version>
    <Description>GUI management and basic widgets for NuciXNA</Description>
    <Authors>Horațiu Mlendea</Authors>
    <Copyright>Copyright 2026 © Horațiu Mlendea</Copyright>
    <RepositoryUrl>https://github.com/hmlendea/nucixna.gui</RepositoryUrl>
    <PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
    <PackageRequireLicenseAcceptance>false</PackageRequireLicenseAcceptance>
    <PackageTags>XNA MonoGame</PackageTags>
  </PropertyGroup>

  <ItemGroup>
    <MonoGameContentReference Include="**\*.mgcb" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="MonoGame.Framework.DesktopGL" Version="3.8.4" />
    <PackageReference Include="NuciXNA.Graphics" Version="2.2.4" />
    <PackageReference Include="NuciXNA.Input" Version="2.1.1" />
    <PackageReference Include="NuciXNA.Primitives" Version="2.1.7" />
  </ItemGroup>
</Project>
```

### NuciXNA.Gui.UnitTests.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>disable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.11.0" />
    <PackageReference Include="MonoGame.Framework.DesktopGL" Version="3.8.4" />
    <PackageReference Include="NuciXNA.Primitives" Version="2.1.8" />
    <PackageReference Include="NUnit" Version="5.0.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="6.3.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\NuciXNA.Gui\NuciXNA.Gui.csproj" />
  </ItemGroup>
</Project>
```

### NuciXNA.Gui.ArchitectureTests.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <RootNamespace>NuciXNA.Gui.ArchitectureTests</RootNamespace>
    <ImplicitUsings>disable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="5.9.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.11.0" />
    <PackageReference Include="NUnit" Version="5.0.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="6.3.1" />
    <PackageReference Include="TngTech.ArchUnitNET" Version="0.13.5" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\NuciXNA.Gui\NuciXNA.Gui.csproj" />
  </ItemGroup>
</Project>
```

### NuciXNA.GUI.slnx

```xml
<Solution>
  <Project Path="NuciXNA.Gui\NuciXNA.Gui.csproj" />
  <Project Path="NuciXNA.Gui.ArchitectureTests\NuciXNA.Gui.ArchitectureTests.csproj" />
  <Project Path="NuciXNA.Gui.UnitTests\NuciXNA.Gui.UnitTests.csproj" />
</Solution>
```

## Build Output

- **NuciXNA.Gui.dll** — Main library assembly
- **NuciXNA.Gui.UnitTests.dll** — Test assembly (not packaged)
- **NuciXNA.Gui.ArchitectureTests.dll** — Architecture/fitness test assembly (not packaged)
- **NuGet package** — `NuciXNA.Gui.{version}.nupkg` from `dotnet pack`

## Naming Conventions

- **Namespaces:** `NuciXNA.Gui`, `NuciXNA.Gui.Controls`, `NuciXNA.Gui.Screens`
- **Classes:** PascalCase (`GuiManager`, `GuiControl`, `ScreenManager`)
- **Interfaces:** `I` prefix (`IGuiControl`, `IScreen`)
- **Events:** Past tense (`Clicked`, `ContentLoaded`, `StateChanged`)
- **Event Args:** `EventArgs` suffix (`ListSelectionEventArgs`, `MouseButtonEventArgs`)
- **Properties:** PascalCase with get/set (`Location`, `Size`, `IsEnabled`)
- **Methods:** PascalCase (`LoadContent`, `Update`, `Draw`, `HandleInput`)
- **Private fields:** camelCase (`children`, `backgroundColour`, `isEnabled`)
