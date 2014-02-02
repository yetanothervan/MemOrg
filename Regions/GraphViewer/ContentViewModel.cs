using System;
using System.Windows;
using System.Windows.Navigation;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;

namespace GraphViewer
{
    public class ContentViewModel : ViewModelBase
    {
        private readonly IGraphVizulaizeService _graphVizulaizeService;
        private readonly IDrawer _drawer;
        private readonly IGrid _iGrid;

        public ContentViewModel(IGraphOrganizeService graphOrganizeService, 
            IGraphDrawService graphDrawService, IGraphVizulaizeService graphVizulaizeService)
        {
            _graphVizulaizeService = graphVizulaizeService;
            var headersToggleCommand = new DelegateCommand(ToggleHeaders, () => true);
            GlobalCommands.ToggleHeadersCompositeCommand.RegisterCommand(headersToggleCommand);

            MyText = "Some of my texts";
            IGraph graph = graphOrganizeService.GetGraph(null);
            IGridLayout layout = graphOrganizeService.GetLayout(graph);
            
            _iGrid = graphOrganizeService.GetGrid(layout);
            IDrawStyle style = graphDrawService.GetStyle();
            
            _drawer = graphDrawService.GetDrawer(style);
            IVisualizeOptions options = graphVizulaizeService.GetVisualizeOptions();
            Grid = graphVizulaizeService.VisualizeGrid(_iGrid, options, _drawer);
        }
        
        private bool _headersOnly = true;

        private void ToggleHeaders()
        {
            _headersOnly = !_headersOnly;
            IVisualizeOptions options = _graphVizulaizeService.GetVisualizeOptions();
            options.HeadersOnly = _headersOnly;
            Grid = _graphVizulaizeService.VisualizeGrid(_iGrid, options, _drawer);
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
