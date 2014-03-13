using System.Collections.Generic;
using MemOrg.Interfaces.OrgUnits;

namespace MemOrg.Interfaces
{
    public interface ITree
    {
        IOrgTag MyElem { get; set; }
        ICollection<ITree> Subtrees { get; set; }
    }
}