using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphVizualizeService
{
    public class GraphVizualizeService : IGraphVizualizeService
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

        public IComponent VisualizeTree(ITree tree, IVisualizeOptions options, IDrawer drawer)
        {
            var visTree = new VisualTree(tree);
            return visTree.Visualize(drawer, options);
        }

        public IComponent StackPanel(IVisualizeOptions options, IDrawer drawer)
        {
            return drawer.DrawStackBox();
        }
    }
}
