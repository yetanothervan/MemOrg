using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;
using GraphOrganizeService.Elems;
using GraphOrganizeService.VisualElems;
using MemOrg.Interfaces;

namespace GraphOrganizeService
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

        public IComponent Prerender(IDrawer drawer)
        {
            _mySelf = drawer.DrawGrid();
            foreach (var gridElem in _grid)
            {
                if (gridElem is GridElemBlock)
                    Childs.Add(new VisualGridElemBlock(gridElem as GridElemBlock).Prerender(drawer));
                else if (gridElem is GridElemBlockRel)
                    Childs.Add(new VisualGridElemBlockRel(gridElem as GridElemBlockRel).Prerender(drawer));
                else if (gridElem is GridElemBlockSource)
                    Childs.Add(new VisualGridElemBlockSource(gridElem as GridElemBlockSource).Prerender(drawer));
                else if (gridElem is GridElemBlockTag)
                    Childs.Add(new VisualGridElemBlockTag(gridElem as GridElemBlockTag).Prerender(drawer));
                else if (gridElem is GridElemTag)
                    Childs.Add(new VisualGridElemTag(gridElem as GridElemTag).Prerender(drawer));
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