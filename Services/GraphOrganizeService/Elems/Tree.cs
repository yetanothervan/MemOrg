using System.Collections.Generic;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public class Tree : GridElem, ITree 
    {
        public Tree(IGrid myGrid) : base(myGrid)
        {
        }

        public IGridElem MyElem { get; set; }
        public ICollection<ITree> Subtrees { get; set; }
    }
}