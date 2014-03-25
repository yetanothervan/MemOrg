using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockSource : OrgBlock, IOrgBlockSource
    {
        public OrgBlockSource(Block block, List<NESW> conPoints)
            : base(block, conPoints)
        {
        }
    }
}