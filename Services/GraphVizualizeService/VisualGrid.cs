using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GraphOrganizeService.VisualElems;
using GraphVizualizeService.VisualElems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphVizualizeService
{
    public class VisualGrid : IVisual, IGrid, IComponent
    {
        private readonly IGrid _grid;
        //private readonly List<IVisualGridElem> _elems;

        public VisualGrid(IGrid grid)
        {
            _grid = grid;
            //_elems = new List<IVisualGridElem>();
        }

        public IComponent Visualize(IDrawer drawer)
        {
            _mySelf = drawer.DrawGrid();
            foreach (var gridElem in _grid)
            {
                if (gridElem is IGridElemBlockOthers)
                    Childs.Add(new VisualGridElemBlock(gridElem as IGridElemBlockOthers).Visualize(drawer));
                else if (gridElem is IGridElemBlockRel)
                    Childs.Add(new VisualGridElemBlockRel(gridElem as IGridElemBlockRel).Visualize(drawer));
                else if (gridElem is IGridElemBlockSource)
                    Childs.Add(new VisualGridElemBlockSource(gridElem as IGridElemBlockSource).Visualize(drawer));
                else if (gridElem is IGridElemBlockTag)
                    Childs.Add(new VisualGridElemBlockTag(gridElem as IGridElemBlockTag).Visualize(drawer));
                else if (gridElem is IGridElemTag)
                    Childs.Add(new VisualGridElemTag(gridElem as IGridElemTag).Visualize(drawer));
                else
                    throw new NotImplementedException();
            }
            return _mySelf;
        }

        public IEnumerator<IGridElem> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int RowCount { get { return _grid.RowCount; } }
        public int RowLength { get { return _grid.RowLength; } }
        public void PlaceElem(int row, int col, IGridElem elem)
        {
            throw new NotImplementedException();
        }

        private IComponent _mySelf;
        public List<IComponent> Childs
        {
            get { return _mySelf.Childs; }
            set { _mySelf.Childs = value; }
        }
        
        public List<DrawingVisual> Render(Point p)
        {
            return _mySelf != null ? _mySelf.Render(p) : null;
        }

        public Size GetSize()
        {
            return _mySelf != null ? _mySelf.GetSize() : new Size();
        }
    }
}