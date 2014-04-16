using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GraphManagementService.ModuleDefinition
{
    public class GraphManagementServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public GraphManagementServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IGraphManagementService, GraphManagementService>();
        }
    }
}
