using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    public class Backing : Component
    {
        private readonly IDrawStyle _style;
        private const double Margin = 3;

        public Backing(IDrawStyle style)
        {
            _style = style;
        }

        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetActualSize());                
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);

            if (Childs.Any())
            {
                p.Offset(Margin, Margin);
                result.AddRange(Childs.First().Render(p));
            }
            return result;
        }

        public override Size GetActualSize()
        {
            if (!Childs.Any())
                return new Size(0, 0);
            Size s = Childs.First().GetActualSize();
            s.Width += Margin * 2;
            s.Height += Margin * 2;
            return s;
        }
    }
}
