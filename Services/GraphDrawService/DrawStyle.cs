using System.Windows.Media;
using MemOrg.Interfaces;
using Microsoft.Practices.Unity.Utility;

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
            MaxTextBlockWidth = 200;
            MaxTextBlockHeight = 200;
            TextEmSize = 12;
            CaptionEmSize = 14;

            var p = CreateGridElemBrushNPen(Color.FromArgb(127, 255, 255, 255), Color.FromRgb(100, 100, 100));
            QuoteBlockBrush = p.First;
            QuoteBlockPen = p.Second;

            p = CreateGridElemBrushNPen(Color.FromRgb(255, 0, 0), Color.FromRgb(0, 0, 0));
            SourceBlockBrush = p.First;
            SourceBlockPen = p.Second;

            p = CreateGridElemBrushNPen(Color.FromRgb(127, 127, 127), Color.FromRgb(0, 0, 0));
            TagBlockBrush = p.First;
            TagBlockPen = p.Second;

            p = CreateGridElemBrushNPen(Color.FromRgb(255, 255, 0), Color.FromRgb(0, 0, 0));
            RelationBlockBrush = p.First;
            RelationBlockPen = p.Second;

            p = CreateGridElemBrushNPen(Color.FromRgb(0, 127, 0), Color.FromRgb(0, 0, 0));
            UserTextBlockBrush = p.First;
            UserTextBlockPen = p.Second;

            p = CreateGridElemBrushNPen(Color.FromRgb(127, 127, 127), Color.FromRgb(0, 0, 0));
            TagBrush = p.First;
            TagPen = p.Second;

            p = CreateGridElemBrushNPen(Color.FromRgb(255, 255, 255), Color.FromRgb(0, 0, 0));
            OthersBlockBrush = p.First;
            OthersBlockPen = p.Second;

            p = CreateGridElemBrushNPen(Color.FromRgb(245, 245, 245), Color.FromRgb(127, 127, 127), 1.0);
            OthersBlockNoParticlesBrush = p.First;
            OthersBlockNoParticlesPen = p.Second;
        }

        Pair<Brush, Pen> CreateGridElemBrushNPen(Color brushColor, Color penColor, double penSize = 2.0)
        {
            Brush brush = new SolidColorBrush(brushColor);
            brush.Freeze();
            var quoteBlockPenBrush = new SolidColorBrush(penColor);
            quoteBlockPenBrush.Freeze();
            var pen = new Pen(quoteBlockPenBrush, penSize);
            pen.Freeze();
            return new Pair<Brush, Pen>(brush, pen);
        }
        
        public double MaxTextBlockWidth { get; set; }
        public double MaxTextBlockHeight { get; set; }
        public Typeface TextTypeface { get; set; }
        public double TextEmSize { get; set; }
        public Brush TextBrush { get; set; }
        public Typeface CaptionTypeface { get; set; }
        public double CaptionEmSize { get; set; }
        public Brush CaptionBrush { get; set; }
        public Brush QuoteBlockBrush { get; set; }
        public Pen QuoteBlockPen { get; set; }
        public Brush OthersBlockBrush { get; set; }
        public Pen OthersBlockPen { get; set; }
        public Brush OthersBlockNoParticlesBrush { get; set; }
        public Pen OthersBlockNoParticlesPen { get; set; }
        public Brush SourceBlockBrush { get; set; }
        public Pen SourceBlockPen { get; set; }
        public Brush TagBlockBrush { get; set; }
        public Pen TagBlockPen { get; set; }
        public Brush RelationBlockBrush { get; set; }
        public Pen RelationBlockPen { get; set; }
        public Brush UserTextBlockBrush { get; set; }
        public Pen UserTextBlockPen { get; set; }
        public Brush TagBrush { get; set; }
        public Pen TagPen { get; set; }
    }
}
