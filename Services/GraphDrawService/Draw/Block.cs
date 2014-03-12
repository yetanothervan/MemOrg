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
    abstract class Block : Component, IGridElem
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
        
        public abstract override List<DrawingVisual> Render(Point p);

        public override Size GetSize()
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

        public IComponent Component {
            get { return this; }
            set { } 
        }
    }
}
