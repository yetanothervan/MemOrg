using System;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlockUserText : GridElemBlock, IGridElemBlockUserText
    {
        public GridElemBlockUserText(Block block, IGrid myGrid)
            : base(block, myGrid)
        {
        }
    }
}