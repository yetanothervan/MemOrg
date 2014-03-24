using System.Collections.Generic;
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
            ConnectionPoints = new List<NESW>();
        }

        public Block Block
        {
            get { return _block; }
        }

        public IEnumerable<NESW> ConnectionPoints { get; private set; }
    }
}