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
        private readonly IGridElem _gridElem;
        public Block(IDrawStyle style, IGridElem gridElem)
        {
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
            throw new NotImplementedException();
        }

        public Size GetSize()
        {
            throw new NotImplementedException();
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
