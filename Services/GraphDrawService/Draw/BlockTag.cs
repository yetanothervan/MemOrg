using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphDrawService.Draw
{
    class BlockTag : Block
    {
        public BlockTag(IDrawStyle style, IGridElem gridElem) : base(style, gridElem)
        {
        }

        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetSize());
                dc.DrawRoundedRectangle(Style.TagBlockBrush, Style.TagBlockPen, rect, 10.0, 10.0);
            }
            result.Add(dv);
            result.AddRange(new VerticalStackLayout(Childs, Margin).Render(p));
            return result;
        }
    }
}