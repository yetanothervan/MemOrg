using System.Collections.Generic;
using System.Linq;
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
            _gridLinkParts = null;
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

        private List<GridLinkPart> _gridLinkParts;
        public bool IsGridLinkPart { get { return _gridLinkParts != null && _gridLinkParts.Count != 0; }}
        public IReadOnlyList<GridLinkPart> GridLinkParts { get { return _gridLinkParts; }}

        public void AddGridLink(GridLinkPart part)
        {
            if (_gridLinkParts == null)
                _gridLinkParts= new List<GridLinkPart>();
            if (_gridLinkParts.Any(p => p.Direction == part.Direction && p.Type == part.Type))
                return;
            _gridLinkParts.Add(part);
        }

        public List<NESW> ConnectionPoints { get; set; }
        public HorizontalAligment HorizontalAligment;
        
        private IPage _page;

        public bool Placed;
        public int Row;
        public int Col;
    }
}