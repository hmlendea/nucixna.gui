using Assembly = System.Reflection.Assembly;

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;

using NuciXNA.Gui;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NuciXNA.Gui.ArchitectureTests
{
    /// <summary>
    /// Single ArchUnitNET model of the NuciXNA.Gui production assembly.
    /// </summary>
    internal static class ProductionArchitecture
    {
        internal static Assembly ProductionAssembly { get; } = typeof(GuiManager).Assembly;

        internal static Architecture Model { get; } = new ArchLoader()
            .LoadAssemblies(ProductionAssembly)
            .Build();

        internal static IObjectProvider<IType> ProductionTypes { get; } = Types()
            .That()
            .ResideInAssembly(ProductionAssembly)
            .As("NuciXNA.Gui production types");
    }
}
