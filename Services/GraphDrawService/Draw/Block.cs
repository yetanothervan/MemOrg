using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphDrawService.Draw
{
    abstract class Block : IComponent, IGridElem
    {
        private readonly IGridElem _gridElem;
        protected readonly Brush Brush;
        protected readonly Pen Pen;

        protected const double Margin = 5.0;

        protected Block(Brush brush, Pen pen, IGridElem gridElem)
        {
            _gridElem = gridElem;
            Brush = brush;
            Pen = pen;
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

        public abstract List<DrawingVisual> Render(Point p);

        public virtual Size GetSize()
        {
            return new VerticalStackLayout(Childs, Margin).CalculateSize();
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
