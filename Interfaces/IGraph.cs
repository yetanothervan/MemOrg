using System.Collections;
using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IGraph
    {
        IGraphService GraphService { get; }
        IList<IBook> Books { get; }
    }
}