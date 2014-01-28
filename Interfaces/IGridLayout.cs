using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IGridLayout
    {
        List<List<IGridElem>> DoLayout(IGraph graph);
    }
}