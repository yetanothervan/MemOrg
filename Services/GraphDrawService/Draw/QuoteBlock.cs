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
    public class QuoteBlock : Component
    {
        private readonly IDrawStyle _style;
        private const double Margin = 5.0;
        
        public QuoteBlock(IDrawStyle style)
        {
            _style = style;
        }

        public override List<Visual> Render(Point p)
        {
            var result = new List<Visual>();
            
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetActualSize());
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);
            result.AddRange(new VerticalStackLayout(Childs, Margin).Render(p));

            return result;
        }

        public override Size GetActualSize()
        {
            return new VerticalStackLayout(Childs, Margin).CalculateSize();
        }
    }
}
