using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockUserText : OrgBlock, IOrgBlockUserText
    {
        public OrgBlockUserText(Block block)
            : base(block)
        {
        }
    }
}