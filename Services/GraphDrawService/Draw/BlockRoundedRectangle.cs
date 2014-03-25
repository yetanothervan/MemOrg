using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    class BlockRoundedRectangle : Block
    {
        public BlockRoundedRectangle(Brush brush, Pen pen)
            : base(brush, pen)
        {
        }

        public override List<DrawingVisual> Render(Point p1, Point? p2)
        {
            var result = new List<DrawingVisual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p1, GetActualSize());
                dc.DrawRoundedRectangle(Brush, Pen, rect, 10.0, 10.0);
            }
            result.Add(dv);
            result.AddRange(new VerticalStackLayout(Childs, Margin).Render(p1));
            return result;
        }
    }
}