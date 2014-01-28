using DAL.Entity;

namespace GraphOrganizeService
{
    public class GridElemBasedOnBlock : GridElem
    {
        private readonly Block _block;
        private readonly GridElemBasedOnBlockType _type;
        public GridElemBasedOnBlock(Block block, GridElemBasedOnBlockType type)
        {
            _block = block;
            _type = type;
        }

        public Block Block
        {
            get { return _block; }
        }

        public GridElemBasedOnBlockType Type
        {
            get { return _type; }
        }
    }
}