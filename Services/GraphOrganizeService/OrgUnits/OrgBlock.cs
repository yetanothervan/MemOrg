using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using DAL.Entity;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public abstract class OrgBlock : IOrgBlock
    {
        private readonly Block _block;

        protected OrgBlock(Block block, IEnumerable<NESW> conPoints)
        {
            _block = block;
            ConnectionPoints = conPoints ?? new List<NESW>();
        }

        public Block Block
        {
            get { return _block; }
        }

        public IEnumerable<NESW> ConnectionPoints { get; private set; }
    }
}