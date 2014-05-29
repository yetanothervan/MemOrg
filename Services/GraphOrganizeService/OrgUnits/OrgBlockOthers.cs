using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockOthers : OrgBlock, IOrgBlockOthers
    {
        public OrgBlockOthers(IPage page, List<ConnectionPoint> conPoints)
            : base(page, conPoints)
        {
        }
    }
}