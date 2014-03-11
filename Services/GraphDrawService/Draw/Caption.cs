using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    internal class Caption : Component
    {
        private readonly FormattedText _text;

        public Caption(string text, IDrawStyle style)
        {
            _text = new FormattedText(text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, style.CaptionTypeface, style.CaptionEmSize, style.CaptionBrush);
            _text.MaxTextWidth = style.MaxTextBlockWidth;
            _text.MaxTextHeight = style.MaxTextBlockHeight;
        }
        
        public override List<DrawingVisual> Render(Point p)
        {
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                dc.DrawText(_text, p);
            }
            return new List<DrawingVisual> {dv};
        }

        public override Size GetSize()
        {
            return new Size(_text.Width, _text.Height);
        }
    }
}
