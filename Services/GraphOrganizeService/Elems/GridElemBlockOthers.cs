using System;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlockOthers : GridElemBlock, IGridElemBlockOthers
    {
        public GridElemBlockOthers(Block block, IGrid myGrid)
            : base(block, myGrid)
        {
        }
    }
}