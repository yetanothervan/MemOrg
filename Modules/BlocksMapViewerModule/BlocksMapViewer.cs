using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace BlocksMapViewerModule
{
    public class BlocksMapViewer : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public BlocksMapViewer(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.BlocksMapViewerRegion, typeof(ContentView));
        }
    }
}
