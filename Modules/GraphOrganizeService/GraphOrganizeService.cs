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

        public IGrid GetGrid(IGraph graph, IGridLayout layout)
        {
            IGrid grid = new Grid(graph, layout);
            return grid;
        }

        public IVisualGrid GetVisualGrid(IGrid grid, IDrawer drawer)
        {
            IVisualGrid visGrid = new VisualGrid(grid);
            visGrid.Prerender(drawer);
            return visGrid;
        }

        public IGridLayout GetLayout()
        {
            return new LayoutRawSquare();
        }
    }
}
