using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace BlocksMapViewer.ModuleDefinition
{
    public class BlocksMapViewerModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public BlocksMapViewerModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.BlocksMapViewerRegion, typeof(ContentView));
        }
    }
}
