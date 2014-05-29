using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockTag : OrgBlock, IOrgBlockTag
    {
        public OrgBlockTag(IPage page, List<ConnectionPoint> conPoints)
            : base(page, conPoints)
        {
        }
    }
}