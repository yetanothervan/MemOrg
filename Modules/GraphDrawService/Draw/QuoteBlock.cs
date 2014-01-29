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

            Point curPt = p;
            curPt.Offset(Margin, Margin);
            foreach (var child in _childs)
            {
                result.AddRange(child.Render(curPt));
                curPt.Offset(0.0, child.GetSize().Height + Margin);
            }
            return result;
        }

        public Size GetSize()
        {
            double height = _childs.Sum(child => (child.GetSize().Height + Margin)) + Margin;
            double width = _childs.Max(child => (child.GetSize().Height)) + Margin * 2;
            return new Size {Height = height, Width = width};
        }
    }
}
