using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphVizualizeService
{
    public class GraphVizualizeService : IGraphVizulaizeService
    {
        public IVisualizeOptions GetVisualizeOptions()
        {
            return new VisualizeOptions();
        }
        
        public IComponent VisualizeGrid(IGrid grid, IVisualizeOptions options, IDrawer drawer)
        {
            var visGrid = new VisualGrid(grid);
            return visGrid.Visualize(drawer, options);
        }
    }
}
