using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockUserText : OrgBlock, IOrgBlockUserText
    {
        public OrgBlockUserText(IPage page, List<NESW> conPoints)
            : base(page, conPoints)
        {
        }
    }
}