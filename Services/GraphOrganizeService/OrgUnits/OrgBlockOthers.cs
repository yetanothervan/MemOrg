using DAL.Entity;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockOthers : OrgBlock, IOrgBlockOthers
    {
        public OrgBlockOthers(Block block)
            : base(block)
        {
        }
    }
}