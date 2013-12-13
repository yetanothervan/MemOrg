using GraphViewer;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace GraphViewer.ModuleDefinition
{
    public class GraphViewerModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public GraphViewerModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.GraphViewerRegion, typeof(ContentView));
        }
    }
}
