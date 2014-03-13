using System.Collections.Generic;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class Tree : ITree 
    {
        public IOrgTag MyElem { get; set; }
        public ICollection<ITree> Subtrees { get; set; }
    }
}