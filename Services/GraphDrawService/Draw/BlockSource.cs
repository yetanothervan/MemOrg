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
    class BlockSource : Block
    {
        public BlockSource(IDrawStyle style, IGridElem gridElem)
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
                dc.DrawRoundedRectangle(Style.SourceBlockBrush, Style.SourceBlockPen, rect, 10.0, 10.0);
            }
            result.Add(dv);
            result.AddRange(DrawerFuncs.RenderStackLayout(p, Childs, Margin));
            return result;
        }
        
    }
}
