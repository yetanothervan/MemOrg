using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GraphVizualizeService.VisualElems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService
{
    public class VisualGrid : IVisual, IGrid, IComponent
    {
        private readonly IOrgGrid _grid;

        public VisualGrid(IOrgGrid grid)
        {
            _grid = grid;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            _mySelf = drawer.DrawGrid();
            foreach (var gridElem in _grid)
            {
                IComponent component;
                if (gridElem.Content is GridLinkPart)
                    component = drawer.DrawLink();
                else 
                    component = CreateLinkedBoxWithBlock(gridElem, drawer, options);
                var ge = drawer.DrawGridElem(gridElem.RowIndex, gridElem.ColIndex);
                ge.Childs.Add(component);
                Childs.Add(ge);
            }

            var back = drawer.DrawBacking();
            back.Childs.Add(_mySelf);
            return back;
        }
        
        private IComponent CreateLinkedBoxWithBlock(IGridElem elem, IDrawer drawer, IVisualizeOptions options)
        {
            IComponent result;

            var gc = elem.Content;

            if (gc is IOrgBlockOthers)
                result = new VisualGridElemBlock(gc as IOrgBlockOthers).Visualize(drawer, options);
            else if (gc is IOrgBlockUserText)
                result = new VisualGridElemBlockUserText(gc as IOrgBlockUserText).Visualize(drawer, options);
            else if (gc is IOrgBlockRel)
                result = new VisualGridElemBlockRel(gc as IOrgBlockRel).Visualize(drawer, options);
            else if (gc is IOrgBlockSource)
                result = new VisualGridElemBlockSource(gc as IOrgBlockSource).Visualize(drawer, options);
            else if (gc is IOrgBlockTag)
                result = new VisualGridElemBlockTag(gc as IOrgBlockTag).Visualize(drawer, options);
            else if (gc is IOrgTag)
                result = new VisualGridElemTag(gc as IOrgTag).Visualize(drawer, options);
            else if (gc is ITree)
                result = new VisualTree(gc as ITree).Visualize(drawer, options);
            else
                throw new NotImplementedException();

            var grid = drawer.DrawGrid();
            var gridcenter = drawer.DrawGridElem(1, 1);
            gridcenter.Childs.Add(result);
            grid.Childs.Add(gridcenter);
            
            var orgBlock = gc as IOrgBlock;
            if (orgBlock != null)
            {
                bool up = orgBlock.ConnectionPoints.Any(p => p == NESW.North);
                bool left = orgBlock.ConnectionPoints.Any(p => p == NESW.West);
                bool right = orgBlock.ConnectionPoints.Any(p => p == NESW.East);
                bool down = orgBlock.ConnectionPoints.Any(p => p == NESW.South);

                if (left) AddBoxLink(drawer, grid, 1, 0);
                if (right) AddBoxLink(drawer, grid, 1, 2);
                if (up) AddBoxLink(drawer, grid, 0, 1);
                if (down) AddBoxLink(drawer, grid, 2, 0);

                if (left && right) grid.HorizontalAligment = HorizontalAligment.Center;
                else if (left)
                    grid.HorizontalAligment = HorizontalAligment.Left;
                else if (right)
                    grid.HorizontalAligment = HorizontalAligment.Right;
            }

            return grid;
        }

        private void AddBoxLink(IDrawer drawer, IComponent grid, int row, int col)
        {
            var gridElem = drawer.DrawGridElem(row, col);
            gridElem.Childs.Add(drawer.DrawLink());
            grid.Childs.Add(gridElem);
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
        
        public List<DrawingVisual> Render(Point p1, Point? p2)
        {
            return _mySelf != null ? _mySelf.Render(p1, p2) : null;
        }

        public Size GetActualSize()
        {
            return _mySelf != null ? _mySelf.GetActualSize() : new Size();
        }
        
        public HorizontalAligment HorizontalAligment { get; set; }
        public VerticalAligment VerticalAligment { get; set; }
    }
}