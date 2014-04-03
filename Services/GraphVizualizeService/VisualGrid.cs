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
            _mySelf = drawer.DrawGrid(null, null);
            foreach (var gridElem in _grid.OfType<IOrgGridElem>())
            {
                IComponent component;
                if (gridElem.Content is IReadOnlyList<GridLinkPart>)
                    component = drawer.DrawLink(gridElem.Content as IReadOnlyList<GridLinkPart>);
                else 
                    component = CreateLinkedBoxWithBlock(gridElem, drawer, options);
                var ge = drawer.DrawGridElem(gridElem);
                ge.AddChild(component);
                _mySelf.AddChild(ge);
            }

            var back = drawer.DrawBacking();
            back.AddChild(_mySelf);
            return back;
        }

        private IComponent CreateLinkedBoxWithBlock(IOrgGridElem elem, IDrawer drawer, IVisualizeOptions options)
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

            var gridcenter = drawer.DrawGridElem(1, 1);
            gridcenter.AddChild(result);

            IComponent grid;

            var orgBlock = gc as IOrgBlock;
            if (orgBlock != null)
            {
                bool up = orgBlock.ConnectionPoints.Any(p => p == NESW.North);
                bool left = orgBlock.ConnectionPoints.Any(p => p == NESW.West);
                bool right = orgBlock.ConnectionPoints.Any(p => p == NESW.East);
                bool down = orgBlock.ConnectionPoints.Any(p => p == NESW.South);

                var colsWidths = new Dictionary<int, double>();
                var rowsHeights = new Dictionary<int, double>();

                if (left && elem.HorizontalContentAligment != HorizontalAligment.Left) colsWidths.Add(0, 1);
                if (right && elem.HorizontalContentAligment != HorizontalAligment.Right ) colsWidths.Add(2, 1);
                if (up && elem.VerticalContentAligment != VerticalAligment.Top) rowsHeights.Add(0, 1);
                if (down && elem.VerticalContentAligment != VerticalAligment.Bottom) rowsHeights.Add(2, 1);

                grid = drawer.DrawGrid(colsWidths, rowsHeights);
                grid.AddChild(gridcenter);
                
                if (left) AddBoxLink(drawer, grid, 1, 0, false);
                if (right) AddBoxLink(drawer, grid, 1, 2, false);
                if (up) AddBoxLink(drawer, grid, 0, 1, true);
                if (down) AddBoxLink(drawer, grid, 2, 0, true);
                return grid;
            }

            grid = drawer.DrawGrid(null, null);
            grid.AddChild(gridcenter);
            return grid;
        }

        private void AddBoxLink(IDrawer drawer, IComponent grid, int row, int col, bool vertical)
        {
            var parts = new List<GridLinkPart>
            {
                new GridLinkPart
                {
                    Direction = vertical ? GridLinkPartDirection.NorthSouth : GridLinkPartDirection.WestEast,
                    Type = GridLinkPartType.Relation
                }
            };
            var gridElem = drawer.DrawGridElem(row, col);
            gridElem.AddChild(drawer.DrawLink(parts));
            grid.AddChild(gridElem);
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

        public IGridElem GetElem(int row, int col)
        {
            throw new NotImplementedException();
        }

        private IComponent _mySelf;
        public IEnumerable<IComponent> Childs
        {
            get { return _mySelf.Childs; }
        }

        public void AddChild(IComponent child)
        {
            _mySelf.AddChild(child);
        }

        public List<Visual> Render(Point p)
        {
            return _mySelf != null ? _mySelf.Render(p) : null;
        }

        public Size GetActualSize()
        {
            return _mySelf != null ? _mySelf.GetActualSize() : new Size();
        }

        public Size? PreferSize { get; set; }
        public object Logical { get; set; }
    }
}