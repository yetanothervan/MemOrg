using System;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlockTag : GridElem
    {
        private readonly Block _block;
        private readonly Tag _tag;

        public GridElemBlockTag(Block block, Tag tag, IGrid myGrid)
            : base(myGrid)
        {
            _block = block;
            _tag = tag;
        }

        public Block Block
        {
            get { return _block; }
        }
        
        public Tag Tag
        {
            get { return _tag; }
        }
    }
}