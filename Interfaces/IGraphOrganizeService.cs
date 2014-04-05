using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphOrganizeService
    {
        IGraph GetGraph(IGraphFilter filter);
        IGridLayout GetFullLayout(IGraph graph);
        IGridLayout GetLayout(IGraph graph);
        ITreeLayout GetTagLayout(IGraph graph);
        ITreeLayout GetChapterTreeLayout(IGraph graph);
    }
}
