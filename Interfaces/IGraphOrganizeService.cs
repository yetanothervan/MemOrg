using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphOrganizeService
    {
        IGraph GetGraph(IGraphFilter filter);
        IGridLayout GetLayout(IGraph graph);
        ITreeLayout GetTagLayout(IGraph graph);
    }
}
