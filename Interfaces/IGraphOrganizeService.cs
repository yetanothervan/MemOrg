using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphOrganizeService
    {
        IGraph GetGraph(IGraphFilter filter);
        IGrid GetGrid(IGridLayout layout);
        IComponent GetVisualGrid(IGrid grid, IDrawer drawer);
        IGridLayout GetLayout(IGraph graph);
    }
}
