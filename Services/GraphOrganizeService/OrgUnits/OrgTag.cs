using DAL.Entity;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgTag : IOrgTag
    {
        public OrgTag(Tag tag)
        {
            _tag = tag;
        }

        private readonly Tag _tag;
        public Tag Tag
        {
            get { return _tag; }
        }
    }
}