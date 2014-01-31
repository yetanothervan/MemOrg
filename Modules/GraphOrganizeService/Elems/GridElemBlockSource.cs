using System;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService.Elems
{
    public class GridElemBlockSource : GridElem
    {
        private readonly Block _block;
        public GridElemBlockSource(Block block, IGrid myGrid)
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