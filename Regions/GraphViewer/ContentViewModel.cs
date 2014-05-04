using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IGraphOrganizeService _graphOrganizeService;
        private readonly IGraphDrawService _graphDrawService;
        private readonly IGraphVizualizeService _graphVizualizeService;
        private readonly IGraphService _graphService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDrawer _drawer;
        private IVisualizeOptions _options;

        IOrgGrid _rawGrid;
        IOrgGrid _camoGrid;
        IOrgGrid _tagTrees;
        IOrgGrid _blockTrees;

        public ContentViewModel(IGraphOrganizeService graphOrganizeService, 
            IGraphDrawService graphDrawService, IGraphVizualizeService graphVizualizeService, IGraphService _graphService,
            IEventAggregator eventAggregator)
        {
            _currentPage = null;
            _modifyedBlocks = new List<Block>();
            _graphOrganizeService = graphOrganizeService;
            _graphDrawService = graphDrawService;
            _graphVizualizeService = graphVizualizeService;
            this._graphService = _graphService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ParticleChanged>().Subscribe(OnParticleChanged);
            _eventAggregator.GetEvent<BlockChanged>().Subscribe(OnBlockChanged);
            _eventAggregator.GetEvent<PageSelected>().Subscribe(OnPageSelected);
            var headersToggleCommand = new DelegateCommand(ToggleHeaders, () => true);
            var refreshGraphCommand = new DelegateCommand(RefreshGraph, () => true);
            GlobalCommands.ToggleHeadersCompositeCommand.RegisterCommand(headersToggleCommand);
            GlobalCommands.RefreshGraphViewCompositeCommand.RegisterCommand(refreshGraphCommand);
            
            MyText = "Some of my texts";
            IDrawStyle style = _graphDrawService.GetStyle();
            _drawer = _graphDrawService.GetDrawer(style);
            _options = _graphVizualizeService.GetVisualizeOptions();
            
            RefreshGraph();
        }

        private IPage _currentPage;
        private void OnPageSelected(IPage obj)
        {
            _currentPage = obj;
        }

        private readonly List<Block> _modifyedBlocks;
        private void OnBlockChanged(Block obj)
        {
            var pt = _modifyedBlocks.FirstOrDefault(b => b.BlockId == obj.BlockId);
            if (pt == null)
                _modifyedBlocks.Add(obj);
        }

        Point GetOffsetOfPage(IPage page)
        {
            var vis = Visuals.OfType<ILogicalBlock>()
                .FirstOrDefault(p => p.Data is IPage && (p.Data as IPage).Block.BlockId == page.Block.BlockId);
                        
            var childBounds = VisualTreeHelper.GetDescendantBounds(vis as ContainerVisual);
            return childBounds.TopLeft;
        }

        private void RefreshGraph()
        {
            var oldPageOffset = new Point(0, 0);
            if (_currentPage != null && Visuals != null)
                oldPageOffset = GetOffsetOfPage(_currentPage);
            

            IGraph graph = _graphOrganizeService.GetGraph(null);
            IGridLayout rawLayout = _graphOrganizeService.GetFullLayout(graph);
            IGridLayout camoLayout = _graphOrganizeService.GetLayout(graph);
            ITreeLayout tagLayout = _graphOrganizeService.GetTagLayout(graph);
            ITreeLayout blockLayout = _graphOrganizeService.GetChapterTreeLayout(graph);

            _camoGrid = camoLayout.CreateGrid();
            _rawGrid = rawLayout.CreateGrid();
            _tagTrees = tagLayout.CreateTreesGrid();
            _blockTrees = blockLayout.CreateTreesGrid();
            
            UpdateGrid(_options);

            if (_currentPage != null)
            {
                var newPageOffset = GetOffsetOfPage(_currentPage);
                var newOffset = new Vector(Offset.X - (newPageOffset.X - oldPageOffset.X),
                    Offset.Y - (newPageOffset.Y - oldPageOffset.Y));
                Offset = newOffset;
            }
            else 
                Offset = new Vector(0, 400 - _component.GetActualSize().Height);
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

            _component = stack;
            var visuals = _component.Render(new Point(0, 0));
            Visuals = visuals;
        }
        private bool _headersOnly = true;

        public void VisualMouseDown(Visual vis)
        {
            var page = _graphDrawService.GetByVisual(vis) as IPage;

            if (page != null)
            {
                var block = _modifyedBlocks.FirstOrDefault(b => b.BlockId == page.Block.BlockId);
                if (block != null)
                {
                    var blockFromBase = _graphService.BlockAll.First(b => b.BlockId == block.BlockId);
                    page.Block = blockFromBase;
                    _modifyedBlocks.Remove(block);
                }
                _eventAggregator.GetEvent<PageSelected>().Publish(page);
            }
        }

        private void OnParticleChanged(Particle obj)
        {
            var pt = _modifyedBlocks.FirstOrDefault(b => b.BlockId == obj.Block.BlockId);
            if (pt == null)
                _modifyedBlocks.Add(obj.Block);
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

        private IComponent _component;
        private IList<Visual> _visuals;
        public IList<Visual> Visuals
        {
            get
            {
                return _visuals;
            }
            set
            {
                if (_visuals != value)
                {
                    _visuals = value;
                    RaisePropertyChangedEvent("Visuals");
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
