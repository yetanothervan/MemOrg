using DAL.Entity;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockRel : OrgBlock, IOrgBlockRel
    {
        public OrgBlockRel(Block block)
            : base(block)
        {
        }
    }
}