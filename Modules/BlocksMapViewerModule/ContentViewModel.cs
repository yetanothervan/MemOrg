using System;
using System.Windows;
using MemOrg.Interfaces;

namespace GraphViewer
{
    public class ContentViewModel : ViewModelBase
    {
        public ContentViewModel(IGraphOrganizeService graphOrganizeService, IGraphDrawService graphDrawService)
        {
            MyText = "Some of my texts";
            IGraph graph = graphOrganizeService.GetGraph(null);
            IGridLayout layout = graphOrganizeService.GetLayout(graph);
            IGrid grid = graphOrganizeService.GetGrid(layout);
            IDrawStyle style = graphDrawService.GetStyle();
            IDrawer drawer = graphDrawService.GetDrawer(style);
            Grid = graphOrganizeService.GetVisualGrid(grid, drawer);
        }

        private string _myText;
        public string MyText
        {
            get
            {
                return _myText;
            }
            set
            {
                if (_myText != value)
                {
                    _myText = value;
                    RaisePropertyChangedEvent("MyText");
                }
            }
        }

        private IComponent _grid;
        public IComponent Grid
        {
            get
            {
                return _grid;
            }
            set
            {
                if (_grid != value)
                {
                    _grid = value;
                    RaisePropertyChangedEvent("Grid");
                }
            }
        }

        private Vector _offset;
        public Vector Offset
        {
            get { return _offset; }
            set
            {
                if (Math.Abs(_offset.X - value.X) > 0.5
                    || Math.Abs(_offset.Y - value.Y) > 0.5)
                {
                    _offset = value;
                    RaisePropertyChangedEvent("Offset");
                }
            }
        }
    }
}
