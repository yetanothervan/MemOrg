using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace GraphViewer
{
    public class ContentViewModel : ViewModelBase
    {
        private readonly IGraphDrawService _graphDrawService;
        private readonly IGraphVizualizeService _graphVizualizeService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDrawer _drawer;
        private IVisualizeOptions _options;

        readonly IOrgGrid _rawGrid;
        readonly IOrgGrid _camoGrid;
        readonly IOrgGrid _tagTrees;
        readonly IOrgGrid _blockTrees;

        public ContentViewModel(IGraphOrganizeService graphOrganizeService, 
            IGraphDrawService graphDrawService, IGraphVizualizeService graphVizualizeService,
            IEventAggregator eventAggregator)
        {
            _graphDrawService = graphDrawService;
            _graphVizualizeService = graphVizualizeService;
            _eventAggregator = eventAggregator;
            var headersToggleCommand = new DelegateCommand(ToggleHeaders, () => true);
            GlobalCommands.ToggleHeadersCompositeCommand.RegisterCommand(headersToggleCommand);

            MyText = "Some of my texts";
            IGraph graph = graphOrganizeService.GetGraph(null);
            IGridLayout rawLayout = graphOrganizeService.GetFullLayout(graph);
            IGridLayout camoLayout = graphOrganizeService.GetLayout(graph);
            ITreeLayout tagLayout = graphOrganizeService.GetTagLayout(graph);
            ITreeLayout blockLayout = graphOrganizeService.GetChapterTreeLayout(graph);

            _camoGrid = camoLayout.CreateGrid();
            _rawGrid = rawLayout.CreateGrid();
            _tagTrees = tagLayout.CreateTreesGrid();
            _blockTrees = blockLayout.CreateTreesGrid();

            IDrawStyle style = graphDrawService.GetStyle();
            _drawer = graphDrawService.GetDrawer(style);

            _options = _graphVizualizeService.GetVisualizeOptions();
            UpdateGrid(_options);
            Offset = new Vector(0, -300);
        }

        private void UpdateGrid(IVisualizeOptions options)
        {
            var camoVisGrid = _graphVizualizeService.VisualizeGrid(_camoGrid, options, _drawer);
            var rawVisGrid = _graphVizualizeService.VisualizeGrid(_rawGrid, options, _drawer);
            var tagVisTree = _graphVizualizeService.VisualizeGrid(_tagTrees, options, _drawer);
            var blockVisTree = _graphVizualizeService.VisualizeGrid(_blockTrees, options, _drawer);

            var stack = _graphVizualizeService.StackPanel(options, _drawer);
            stack.AddChild(blockVisTree);
            stack.AddChild(tagVisTree);
            stack.AddChild(camoVisGrid);
            stack.AddChild(rawVisGrid);

            Grid = stack;
        }
        private bool _headersOnly = true;

        public void VisualMouseDown(Visual vis)
        {
            var res = _graphDrawService.GetByVisual(vis);
            if (res is Block)
            _eventAggregator.GetEvent<BlockSelected>().Publish(res as Block);
        }

        private void ToggleHeaders()
        {
            _headersOnly = !_headersOnly;
            _options.HeadersOnly = _headersOnly;
            UpdateGrid(_options);
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
