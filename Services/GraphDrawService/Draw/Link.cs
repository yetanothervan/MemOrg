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
    class Link : Component, IGridLink
    {
        private readonly GridLink _gridLink;
        private readonly Pen _pen;

        public Link(GridLink gridLink, Pen pen)
        {
            _gridLink = gridLink;
            _pen = pen;
        }

        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                dc.DrawLine(_pen, new Point(p.X, p.Y + RenderHeight / 2), 
                    new Point(p.X + RenderWidth, p.Y + RenderHeight / 2));
            }
            result.Add(dv);
            return result;
        }
        
        public override Size GetSize()
        {
            //var height = (RenderHeight >= 0) ? RenderHeight : 20.0;
            //var width = (RenderWidth >= 0) ? RenderWidth : 20.0;
            return new Size(30, 20);
        }

        public GridLinkPoint Begin { get { return _gridLink.Begin; } }
        public GridLinkPoint End { get { return _gridLink.End; } }
    }
}
