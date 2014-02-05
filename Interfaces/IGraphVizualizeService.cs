using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IGraphVizualizeService
    {
        IVisualizeOptions GetVisualizeOptions();
        IComponent VisualizeGrid(IGrid grid, IVisualizeOptions options, IDrawer drawer);
        IComponent StackPanel(IVisualizeOptions options, IDrawer drawer);
    }
}