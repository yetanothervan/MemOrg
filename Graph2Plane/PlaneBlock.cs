using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Models;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;

namespace Graph2Plane
{
    public class PlaneBlock
    {
        public PlaneBlock(Block block)
        {
            _text = new FormattedText(block.Caption,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 10, Brush);
        }

        private static readonly Brush Brush;
        private static readonly Pen Pen;
        private readonly FormattedText _text;

        static PlaneBlock()
        {
            Brush = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            Pen = new Pen(Brush, 2.0);
            Brush.Freeze();
            Pen.Freeze();
        }

        public Point P1, P2;

        public DrawingVisual Render()
        {
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var p3 = new Point {X = P2.X, Y = P1.Y};
                var p4 = new Point {X = P1.X, Y = P2.Y};
                var pt = new Point {X = P1.X + 10.0, Y = P1.Y + 10.0};

                dc.DrawLine(Pen, P1, p3);
                dc.DrawLine(Pen, p3, P2);
                dc.DrawLine(Pen, P2, p4);
                dc.DrawLine(Pen, p4, P1);
                dc.DrawText(_text, pt);
            }
            return dv;
        }
    }
}
