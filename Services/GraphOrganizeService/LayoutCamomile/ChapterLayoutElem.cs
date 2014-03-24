using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutElem
    {
        public ChapterLayoutElem()
        {
            Placed = false;
            Row = 0;
            Col = 0;
            GridLinkPart = null;
        }
        public IPage Page
        {
            get { return _page; }
            set
            {
                _page = value;
                _page.Parent = this;
            }
        }

        public GridLinkPart GridLinkPart;
        public bool IsGridLinkPart { get { return GridLinkPart != null; }}
        
        private IPage _page;

        public bool Placed;
        public int Row;
        public int Col;
    }
}