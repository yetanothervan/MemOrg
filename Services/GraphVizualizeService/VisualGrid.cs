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

        public VisualGrid(IGrid grid)
        {
            _grid = grid;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            _mySelf = drawer.DrawGrid();
            foreach (var gridElem in _grid)
            {
                if (gridElem is IGridElemBlockOthers)
                    Childs.Add(new VisualGridElemBlock(gridElem as IGridElemBlockOthers).Visualize(drawer, options));
                else if (gridElem is IGridElemBlockUserText)
                    Childs.Add(new VisualGridElemBlockUserText(gridElem as IGridElemBlockUserText).Visualize(drawer, options));
                else if (gridElem is IGridElemBlockRel)
                    Childs.Add(new VisualGridElemBlockRel(gridElem as IGridElemBlockRel).Visualize(drawer, options));
                else if (gridElem is IGridElemBlockSource)
                    Childs.Add(new VisualGridElemBlockSource(gridElem as IGridElemBlockSource).Visualize(drawer, options));
                else if (gridElem is IGridElemBlockTag)
                    Childs.Add(new VisualGridElemBlockTag(gridElem as IGridElemBlockTag).Visualize(drawer, options));
                else if (gridElem is IGridElemTag)
                    Childs.Add(new VisualGridElemTag(gridElem as IGridElemTag).Visualize(drawer, options));
                else if (gridElem is ITree)
                    Childs.Add(new VisualTree(gridElem as ITree).Visualize(drawer, options));
                else
                    throw new NotImplementedException();
            }
            foreach (var gridLink in Links)
            {
                Childs.Add(new VisualGridLink(gridLink).Visualize(drawer, options));
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

        public List<GridLink> Links
        {
            get { return _grid.Links; }
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