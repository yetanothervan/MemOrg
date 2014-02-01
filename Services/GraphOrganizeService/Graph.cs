using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class Graph : IGraph
    {
        private readonly IGraphService _graphService;

        public Graph(IGraphService graphService)
        {
            _graphService = graphService;
        }

        public IGraphService GraphService
        {
            get { return _graphService; }
        }
    }
}
