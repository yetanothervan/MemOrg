using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GraphOrganizeService.ModuleDefinition
{
    public class GraphOrganizeServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public GraphOrganizeServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IGraphOrganizeService, GraphOrganizeService>();
        }
    }
}
