using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace ChapterViewer.ModuleDefinition
{
    public class ChapterViewerModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public ChapterViewerModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.ChapterViewerRegion, typeof(ContentView));
        }
    }
}
