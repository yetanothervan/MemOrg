using System.Globalization;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;

namespace BL.Graph2Plane
{
    public class PlaneBlock
    {
        public PlaneBlock(Block block, double desiredTextWidth)
        {
            _text = new FormattedText(block.Caption,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Times New Roman"), 10, Brush);
            _text.MaxTextWidth = desiredTextWidth;
        }

        private static readonly Brush Brush;
        private static readonly Pen Pen;
        private readonly FormattedText _text;

        public double TextWidth { get { return _text.Width; } }
        public double TextHeight { get { return _text.Height; } }

        static PlaneBlock()
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
