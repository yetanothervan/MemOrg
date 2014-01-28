using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphOrganizeService
    {
        IGraph GetGraph(IGraphFilter filter);
        IGrid GetGrid(IGraph graph, IGridLayout layout);
        IVisualGrid GetVisualGrid(IGrid grid, IDrawer drawer);
        IGridLayout GetLayout();
    }
}
