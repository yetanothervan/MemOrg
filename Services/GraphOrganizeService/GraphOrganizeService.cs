using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class GraphOrganizeService : IGraphOrganizeService
    {
        private readonly IGraphService _graphService;
        
        public GraphOrganizeService(IGraphService graphService)
        {
            _graphService = graphService;
        }

        public IGraph GetGraph(IGraphFilter filter)
        {
            return _graphService.Graph;
        }

        public IGridLayout GetFullLayout(IGraph graph)
        {
            return new LayoutRawSquare(graph);
        }

        public IGridLayout GetLayout(IGraph graph)
        {
            return new LayoutCamomile(graph);
        }

        public ITreeLayout GetTagLayout(IGraph graph)
        {
            return new LayoutTagRawSquare(graph);
        }
    }
}
