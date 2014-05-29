using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public abstract class OrgBlock : IOrgBlock
    {
        private readonly IPage _page;

        protected OrgBlock(IPage page, IEnumerable<ConnectionPoint> conPoints)
        {
            _page = page;
            ConnectionPoints = conPoints ?? new List<ConnectionPoint>();
        }

        public IPage Page
        {
            get { return _page; }
        }

        public IEnumerable<ConnectionPoint> ConnectionPoints { get; private set; }
    }
}