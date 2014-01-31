using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    class Block : IComponent, IGridElem
    {
        private readonly IDrawStyle _style;
        private readonly IGridElem _gridElem;
        private const double Margin = 5.0;

        public Block(IDrawStyle style, IGridElem gridElem)
        {
            _style = style;
            _gridElem = gridElem;
        }

        private List<IComponent> _childs;

        public List<IComponent> Childs
        {
            get
            {
                if (_childs == null) Childs = new List<IComponent>(0);
                return _childs;
            }
            set { _childs = value; }
        }

        public List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetSize());
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);
            result.AddRange(DrawerFuncs.RenderStackLayout(p, _childs, Margin));

            return result;
        }

        public Size GetSize()
        {
            return DrawerFuncs.CalculateSizeStackLayout(_childs, Margin);
        }

        public void PlaceOn(int row, int col, List<List<IGridElem>> elems)
        {
            throw new NotImplementedException();
        }

        public int RowIndex
        {
            get { return _gridElem.RowIndex; }
        }

        public int ColIndex
        {
            get { return _gridElem.ColIndex; }
        }
    }
}
