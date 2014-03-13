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
                var component = CreateLinkedBoxWithBlock(gridElem, drawer, options);
            //    gridElem.Component = component;
                Childs.Add(component);
            }
            foreach (var gridLink in Links)
            {
// ReSharper disable once NotAccessedVariable
            //    var begin = gridLink.Begin;
// ReSharper disable once NotAccessedVariable
            //    var end = gridLink.End;
            //    begin.GridElem = _grid.First(e => e.ColIndex == gridLink.Begin.Col && e.RowIndex == gridLink.Begin.Row).Component;
            //    end.GridElem = _grid.First(e => e.ColIndex == gridLink.End.Col && e.RowIndex == gridLink.End.Row).Component;
            //    Childs.Add(new VisualGridLink(gridLink).Visualize(drawer, options));
            }
            return _mySelf;
        }

        private IComponent CreateLinkedBoxWithBlock(IGridElem elem, IDrawer drawer, IVisualizeOptions options)
        {
            var result = drawer.DrawGrid();

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
            
            return result;
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

        public double RenderWidth { get; set; }
        public double RenderHeight { get; set; }
        public HorizontalAligment HorizontalAligment { get; set; }
        public VerticalAligment VerticalAligment { get; set; }
    }
}