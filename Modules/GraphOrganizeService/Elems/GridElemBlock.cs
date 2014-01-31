using System;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlock : GridElem
    {
        private readonly Block _block;
        public GridElemBlock(Block block, IGrid myGrid)
            : base(myGrid)
        {
            _block = block;
        }

        public Block Block
        {
            get { return _block; }
        }
    }
}