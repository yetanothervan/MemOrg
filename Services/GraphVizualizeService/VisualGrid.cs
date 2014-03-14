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
                var component = CreateLinkedBoxWithBlock(gridElem, drawer, options);
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
            
            var linkBegPoints = _grid.Links.Where(l => l.Begin.Col == elem.ColIndex && l.Begin.Row == elem.RowIndex).ToList();
            var linkEndPoints = _grid.Links.Where(l => l.End.Col == elem.ColIndex && l.End.Row == elem.RowIndex).ToList();
            bool up = linkBegPoints.Any(l => l.Begin.ConnectionPoint == NESW.North)
                      || linkEndPoints.Any(l => l.End.ConnectionPoint == NESW.North);
            bool left = linkBegPoints.Any(l => l.Begin.ConnectionPoint == NESW.West)
                      || linkEndPoints.Any(l => l.End.ConnectionPoint == NESW.West);
            bool right = linkBegPoints.Any(l => l.Begin.ConnectionPoint == NESW.East)
                      || linkEndPoints.Any(l => l.End.ConnectionPoint == NESW.East);
            bool down = linkBegPoints.Any(l => l.Begin.ConnectionPoint == NESW.South)
                      || linkEndPoints.Any(l => l.End.ConnectionPoint == NESW.South);

            var grid = drawer.DrawGrid();
            var gridcenter = drawer.DrawGridElem(1, 1);
            gridcenter.Childs.Add(result);
            grid.Childs.Add(gridcenter);

            if (left) AddBoxLink(drawer, grid, 1, 0);
            if (right) AddBoxLink(drawer, grid, 1, 2);
            if (up) AddBoxLink(drawer, grid, 0, 1);
            if (down) AddBoxLink(drawer, grid, 2, 0);

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
        
        public List<DrawingVisual> Render(Point p)
        {
            return _mySelf != null ? _mySelf.Render(p) : null;
        }

        public Size GetSize()
        {
            return _mySelf != null ? _mySelf.GetSize() : new Size();
        }

        public double RenderWidth { get; set; }
        public double RenderHeight { get; set; }
        public HorizontalAligment HorizontalAligment { get; set; }
        public VerticalAligment VerticalAligment { get; set; }
    }
}