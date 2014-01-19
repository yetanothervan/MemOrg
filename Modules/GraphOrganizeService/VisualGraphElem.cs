using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public abstract class VisualGraphElem : IVisualGraphElem
    {
        public VisualGraphElem(Block block, int desiredTextWidth, VisualGraphBlockType blockType)
        {
            _text = CreateFormattedText(block, desiredTextWidth);
            BlockType = blockType;
        }

        public int GridXPos;
        public int GridYPos;

        public readonly VisualGraphBlockType BlockType;

        private enum FormatType
        {
            Bold,
            Italic
        };

        private struct FormatUnit
        {
            public FormatType Type;
            public int StartIndex;
            public int Count;
        };

        private static FormattedText CreateFormattedText(Block block, double desiredTextWidth)
        {
            var funits = new List<FormatUnit>();
            var pos = 0;
            var sb = new StringBuilder();
            sb.Append(block.Caption);
            sb.Append("\r\n");
            pos += block.Caption.Length + 2;
            funits.Add(new FormatUnit{StartIndex = 0, Count = pos, Type = FormatType.Bold});
            
            foreach (var particle in block.Particles.OrderBy(t => t.Order))
            {
                if (particle is SourceTextParticle)
                {
                    var content = (particle as SourceTextParticle).Content;
                    sb.Append(content);
                    sb.Append("\r\n");
                    pos += content.Length + 2;
                }
                else if (particle is QuoteSourceParticle)
                {
                    var content = (particle as QuoteSourceParticle).SourceTextParticle.Content;
                    sb.Append(content);
                    sb.Append("\r\n");
                    funits.Add(new FormatUnit {StartIndex = pos, Count = content.Length + 2, Type = FormatType.Italic});
                    pos += content.Length + 2;
                }
            }

            var ft = new FormattedText(sb.ToString(),
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Times New Roman"), 12, Brush);
            ft.MaxTextWidth = desiredTextWidth;
            foreach (var formatUnit in funits)
            {
                if (formatUnit.Type == FormatType.Bold)
                    ft.SetFontWeight(FontWeights.Bold, formatUnit.StartIndex, formatUnit.Count);
                if (formatUnit.Type == FormatType.Italic)
                    ft.SetFontStyle(FontStyles.Italic, formatUnit.StartIndex, formatUnit.Count);
            }

            return ft;
        }

        private static readonly Brush Brush;
        private static readonly Pen Pen;
        private readonly FormattedText _text;

        public double TextWidth { get { return _text.Width; } }
        public double TextHeight { get { return _text.Height; } }

        static VisualGraphElem()
        {
            Brush = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            Pen = new Pen(Brush, 2.0);
            Brush.Freeze();
            Pen.Freeze();
        }
        
        public Point P1, P2;

        public DrawingVisual Render(double offsetX, double offsetY)
        {
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var p1 = new Point {X = P1.X + offsetX, Y = P1.Y + offsetY};
                var p2 = new Point {X = P2.X + offsetX, Y = P2.Y + offsetY};
                var p3 = new Point {X = p2.X, Y = p1.Y};
                var p4 = new Point {X = p1.X, Y = p2.Y};
                var pt = new Point {X = p1.X + 10.0, Y = p1.Y + 10.0};

                dc.DrawLine(Pen, p1, p3);
                dc.DrawLine(Pen, p3, p2);
                dc.DrawLine(Pen, p2, p4);
                dc.DrawLine(Pen, p4, p1);
                dc.DrawText(_text, pt);
            }
            return dv;
        }
    }
}