using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GraphService.ModuleDefinition
{
    public class GraphServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public GraphServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterInstance(typeof(IMemOrgContext), new MemOrgContext());
            _container.RegisterType<IGraphService, GraphService>();
            _container.RegisterType<IBlockRepository, BlockRepository>();
            _container.RegisterType<IRelationRepository, RelationRepository>();
            _container.RegisterType<ITagRepository, TagRepository>();
            _container.RegisterType<IParticlesRepository, ParticlesRepository>();
        }
    }
}
