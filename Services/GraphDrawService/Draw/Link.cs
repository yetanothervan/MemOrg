using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphDrawService.Draw
{
    class Link : Component 
    {
        private readonly Pen _pen;
        private readonly IReadOnlyList<GridLinkPart> _gridLinkParts;

        public Link(Pen pen, IReadOnlyList<GridLinkPart> gridLinkParts)
        {
            _pen = pen;
            _gridLinkParts = gridLinkParts;
        }

        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();
            if (_gridLinkParts == null)
                return result;

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                if (PreferSize == null)
                    return result;

                var halfWidth = (PreferSize != null ? PreferSize.Value.Width/2 : 5);
                var halfHeight = (PreferSize != null ? PreferSize.Value.Height/2 : 5);

                var c = new Point(p.X + halfWidth, p.Y + halfHeight);

                var w = new Point(p.X, c.Y);
                var e = new Point(c.X + halfWidth, c.Y);
                
                var n = new Point(c.X, p.Y);
                var s = new Point(c.X, c.Y + halfHeight);

                foreach (var part in _gridLinkParts)
                {
                    if (part.Direction == GridLinkPartDirection.NorthEast
                        || part.Direction == GridLinkPartDirection.NorthWest
                        || part.Direction == GridLinkPartDirection.NorthSouth)
                        dc.DrawLine(_pen, n, c);
                    if (part.Direction == GridLinkPartDirection.SouthEast
                        || part.Direction == GridLinkPartDirection.WestSouth
                        || part.Direction == GridLinkPartDirection.NorthSouth)
                        dc.DrawLine(_pen, c, s);
                    if (part.Direction == GridLinkPartDirection.NorthWest
                        || part.Direction == GridLinkPartDirection.WestEast
                        || part.Direction == GridLinkPartDirection.WestSouth)
                        dc.DrawLine(_pen, w, c);
                    if (part.Direction == GridLinkPartDirection.NorthEast
                        || part.Direction == GridLinkPartDirection.SouthEast
                        || part.Direction == GridLinkPartDirection.WestEast)
                        dc.DrawLine(_pen, c, e);
                }
            }
            result.Add(dv);
            return result;
        }
        
        public override Size GetActualSize()
        {
            return new Size(10, 10);
        }
    }
}
