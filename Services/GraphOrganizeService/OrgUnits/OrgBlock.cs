using DAL.Entity;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public abstract class OrgBlock : IOrgBlock
    {
        private readonly Block _block;

        protected OrgBlock(Block block)
        {
            _block = block;
        }

        public Block Block
        {
            get { return _block; }
        }
    }
}