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
    public class QuoteBlock : IComponent
    {
        private readonly IDrawStyle _style;
        private const double Margin = 5.0;
        private List<IComponent> _childs;
        
        public QuoteBlock(IDrawStyle style)
        {
            _style = style;
        }

        public List<IComponent> Childs
        {
            get
            {
                if (_childs == null) Childs = new List<IComponent>(0);
                return _childs;
            }
            set { _childs = value; }
        }

        public List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();
            
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetSize());
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);
            result.AddRange(new VerticalStackLayout(Childs, Margin).Render(p));

            return result;
        }

        public Size GetSize()
        {
            return new VerticalStackLayout(Childs, Margin).CalculateSize();
        }
    }
}
