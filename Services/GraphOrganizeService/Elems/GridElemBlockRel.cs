using System;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlockRel : GridElemBlock, IGridElemBlockRel
    {
        public GridElemBlockRel(Block block, IGrid myGrid)
            : base(block, myGrid)
        {
        }
    }
}