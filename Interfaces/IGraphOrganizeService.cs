using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphOrganizeService
    {
        IGraph GetGraph(IGraphFilter filter);
        IGrid GetGrid(IGridLayout layout);
        IGridLayout GetLayout(IGraph graph);
        IGridLayout GetTagLayout(IGraph graph);
    }
}
