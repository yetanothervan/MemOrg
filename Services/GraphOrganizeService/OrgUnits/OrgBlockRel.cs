using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockRel : OrgBlock, IOrgBlockRel
    {
        public OrgBlockRel(IPage page, List<ConnectionPoint> conPoints)
            : base(page, conPoints)
        {
        }
    }
}