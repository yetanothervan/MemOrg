using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService
{
    public class DrawStyle : IDrawStyle
    {
        public DrawStyle()
        {
            TextBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            TextBrush.Freeze();
            CaptionBrush = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            CaptionBrush.Freeze();
            TextTypeface = new Typeface("Times New Roman");
            CaptionTypeface = new Typeface("Arial");
            DesiredTextBlockWidth = 200;
            TextEmSize = 12;
            CaptionEmSize = 20;
            QuoteBlockBrush =  new SolidColorBrush(Color.FromArgb(127, 255, 255, 255));
            QuoteBlockBrush.Freeze();
            var quoteBlockPenBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            quoteBlockPenBrush.Freeze();
            const double quoteBlockPenSize = 2.0;
            QuoteBlockPen = new Pen(quoteBlockPenBrush, quoteBlockPenSize);
            QuoteBlockPen.Freeze();
        }

        public double DesiredTextBlockWidth { get; set; }
        public Typeface TextTypeface { get; set; }
        public double TextEmSize { get; set; }
        public Brush TextBrush { get; set; }
        public Typeface CaptionTypeface { get; set; }
        public double CaptionEmSize { get; set; }
        public Brush CaptionBrush { get; set; }
        public Brush QuoteBlockBrush { get; set; }
        public Pen QuoteBlockPen { get; set; }
    }
}
