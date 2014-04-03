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

        public override List<Visual> Render(Point p)
        {
            var result = new List<Visual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetActualSize());
                dc.DrawRoundedRectangle(Brush, Pen, rect, 10.0, 10.0);
            }
            result.Add(dv);
            result.AddRange(new VerticalStackLayout(Childs, Margin).Render(p));
            return result;
        }
    }
}