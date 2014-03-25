using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockTag : OrgBlock, IOrgBlockTag
    {
        private readonly Tag _tag;

        public OrgBlockTag(Block block, Tag tag, List<NESW> conPoints)
            : base(block, conPoints)
        {
            _tag = tag;
        }

        public Tag Tag
        {
            get { return _tag; }
        }
    }
}