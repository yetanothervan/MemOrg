using System;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public abstract class GridElemBlock : GridElem, IGridElemBlock
    {
        private readonly Block _block;

        protected GridElemBlock(Block block, IGrid myGrid)
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