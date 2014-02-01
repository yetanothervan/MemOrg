using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphDrawService.Draw
{
    class BlockOthers : Block
    {
        public BlockOthers(IDrawStyle style, IGridElem gridElem)
            : base(style, gridElem)
        {
        }
        
        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetSize());
                dc.DrawRectangle(Style.QuoteBlockBrush, Style.QuoteBlockPen, rect);
            }
            result.Add(dv);
            result.AddRange(DrawerFuncs.RenderStackLayout(p, Childs, Margin));

            return result;
        }
    }
}
