using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DevExpress.XtraPrinting.Native;
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
                var rect = new Rect(p, GetSize());                
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);

            if (Childs.Count > 0)
            {
                p.Offset(Margin, Margin);
                result.AddRange(Childs[0].Render(p));
            }
            return result;
        }

        public override Size GetSize()
        {
            if (Childs.Count == 0)
                return new Size(0, 0);
            Size s = Childs[0].GetSize();
            s.Width += Margin * 2;
            s.Height += Margin * 2;
            return s;
        }
    }
}
