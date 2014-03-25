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

        public override List<DrawingVisual> Render(Point p1, Point? p2)
        {
            var result = new List<DrawingVisual>();
            
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p1, GetActualSize());
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);
            result.AddRange(new VerticalStackLayout(Childs, Margin).Render(p1));

            return result;
        }

        public override Size GetActualSize()
        {
            return new VerticalStackLayout(Childs, Margin).CalculateSize();
        }
    }
}
