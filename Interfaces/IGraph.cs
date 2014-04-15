using System.Collections;
using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IGraph
    {
        IGraphService GraphService { get; }
        IEnumerable<IBook> Books { get; }
    }
}