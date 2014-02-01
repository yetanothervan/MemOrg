using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace TempToolbar.ModuleDefinition
{
    public class TempToolbarModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public TempToolbarModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.TempToolbarRegion, typeof(ContentView));
        }
    }
}
