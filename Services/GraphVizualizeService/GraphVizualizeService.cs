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
            throw new NotImplementedException();
        }

        public IComponent VisualizeGrid(IGrid grid, IDrawer drawer)
        {
            var visGrid = new VisualGrid(grid);
            return visGrid.Visualize(drawer);
        }
    }
}
