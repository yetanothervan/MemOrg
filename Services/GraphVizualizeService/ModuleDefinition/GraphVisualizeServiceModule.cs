using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GraphVizualizeService.ModuleDefinition
{
    public class GraphVisualizeServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public GraphVisualizeServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IGraphVizulaizeService, GraphVizualizeService>();
        }
    }
}
