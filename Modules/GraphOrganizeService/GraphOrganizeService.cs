using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
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
            IGraph graph = new Graph(_graphService);
            return graph;
        }

        public IGrid GetGrid(IGridLayout layout)
        {
            IGrid grid = new Grid(layout);
            return grid;
        }

        public IComponent GetVisualGrid(IGrid grid, IDrawer drawer)
        {
            var visGrid = new VisualGrid(grid);
            return visGrid.Prerender(drawer);
        }

        public IGridLayout GetLayout(IGraph graph)
        {
            return new LayoutRawSquare(graph);
        }
    }
}
