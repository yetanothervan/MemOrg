using System.Collections.Generic;
using System.Linq;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.Chapter
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
            }
        }

        public PageEdge ParentPageEdge { get; set; }

        private List<GridLinkPart> _gridLinkParts;
        public bool IsGridLinkPart { get { return _gridLinkParts != null && _gridLinkParts.Count != 0; }}
        public IReadOnlyList<GridLinkPart> GridLinkParts { get { return _gridLinkParts; }}

        public void AddCon(ConnectionPoint con)
        {
            if (ConnectionPoints == null) ConnectionPoints = new List<ConnectionPoint>();
            if (!ConnectionPoints.Contains(con)) ConnectionPoints.Add(con);
        }

        public void AddCon(NESW dir, GridLinkPartType type)
        {
            var cp = new ConnectionPoint(dir, type);
            AddCon(cp);
        }

        public void AddGridLink(GridLinkPart part)
        {
            if (_gridLinkParts == null)
                _gridLinkParts= new List<GridLinkPart>();
            if (_gridLinkParts.Any(p => p.Direction == part.Direction && p.Type == part.Type))
                return;
            _gridLinkParts.Add(part);
        }

        public List<ConnectionPoint> ConnectionPoints { get; set; }
        public HorizontalAligment HorizontalAligment;
        
        private IPage _page;

        public bool Placed;
        public int Row;
        public int Col;
    }
}