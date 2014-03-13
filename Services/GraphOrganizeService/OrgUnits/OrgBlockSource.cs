using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockSource : OrgBlock, IOrgBlockSource
    {
        public OrgBlockSource(Block block)
            : base(block)
        {
        }
    }
}