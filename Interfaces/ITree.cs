using System.Collections.Generic;
using MemOrg.Interfaces.GridElems;

namespace MemOrg.Interfaces
{
    public interface ITree
    {
        IGridElem MyElem { get; set; }
        ICollection<ITree> Subtrees { get; set; }
    }
}