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

        public Link(Pen pen)
        {
            _pen = pen;
        }

        public override List<DrawingVisual> Render(Point p1, Point? p2)
        {
            var result = new List<DrawingVisual>();
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                dc.DrawLine(_pen,
                    new Point(
                        p1.X, 
                        p1.Y + (p2 != null ? (p2.Value.Y - p1.Y) / 2 : 20)),
                    new Point(
                        (p2 != null ? p2.Value.X : p1.X + 10),
                        p1.Y + (p2 != null ? (p2.Value.Y - p1.Y) / 2 : 20)
                        ));
            }
            result.Add(dv);
            return result;
        }
        
        public override Size GetActualSize()
        {
            //var height = (Height >= 0) ? Height : 20.0;
            //var width = (Width >= 0) ? Width : 20.0;
            return new Size(10, 20);
        }
    }
}
