using System;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlockSource : GridElemBlock, IGridElemBlockSource
    {
        public GridElemBlockSource(Block block, IGrid myGrid)
            : base(block, myGrid)
        {
        }
    }
}