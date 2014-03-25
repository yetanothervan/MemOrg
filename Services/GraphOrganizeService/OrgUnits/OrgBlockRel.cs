using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockRel : OrgBlock, IOrgBlockRel
    {
        public OrgBlockRel(Block block, List<NESW> conPoints)
            : base(block, conPoints)
        {
        }
    }
}