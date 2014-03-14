using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IGraphVizualizeService
    {
        IVisualizeOptions GetVisualizeOptions();
        IComponent VisualizeGrid(IOrgGrid grid, IVisualizeOptions options, IDrawer drawer);
        IComponent VisualizeTree(ITree tree, IVisualizeOptions options, IDrawer drawer);
        IComponent StackPanel(IVisualizeOptions options, IDrawer drawer);
    }
}