using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    class BlockRectangle : Block
    {
        public BlockRectangle(Brush brush, Pen pen)
            : base(brush, pen)
        {
        }

        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetActualSize());
                dc.DrawRectangle(Brush, Pen, rect);
            }
            result.Add(dv);
            result.AddRange(new VerticalStackLayout(Childs, Margin).Render(p));

            return result;
        }
    }
}
