using System;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlockTag : GridElemBlock, IGridElemBlockTag
    {
        private readonly Tag _tag;

        public GridElemBlockTag(Block block, Tag tag, IGrid myGrid)
            : base(block, myGrid)
        {
            _tag = tag;
        }

        public Tag Tag
        {
            get { return _tag; }
        }
    }
}