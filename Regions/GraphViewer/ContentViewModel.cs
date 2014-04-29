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
        private readonly IEventAggregator _eventAggregator;
        private readonly IDrawer _drawer;
        private IVisualizeOptions _options;

        IOrgGrid _rawGrid;
        IOrgGrid _camoGrid;
        IOrgGrid _tagTrees;
        IOrgGrid _blockTrees;

        public ContentViewModel(IGraphOrganizeService graphOrganizeService, 
            IGraphDrawService graphDrawService, IGraphVizualizeService graphVizualizeService,
            IEventAggregator eventAggregator)
        {
            _modifyedParticles = new HashSet<Particle>();
            _graphOrganizeService = graphOrganizeService;
            _graphDrawService = graphDrawService;
            _graphVizualizeService = graphVizualizeService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ParticleChanged>().Subscribe(OnParticleChanged);
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

        private void RefreshGraph()
        {
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
            Offset = new Vector(0, 400 - Grid.GetActualSize().Height);
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
            var block = _graphDrawService.GetByVisual(vis) as Block;

            if (block != null)
            {
                var mPart = _modifyedParticles.FirstOrDefault(p => p.Block.BlockId == block.BlockId);
                if (mPart != null)
                {
                    var part = block.Particles.FirstOrDefault(p => p.ParticleId == mPart.ParticleId);
                    if (part != null)
                    {
                        block.Particles.Remove(part);
                        block.Particles.Add(mPart);
                        _modifyedParticles.Remove(mPart);
                    }
                }
                _eventAggregator.GetEvent<BlockSelected>().Publish(block);
            }
        }

        private readonly HashSet<Particle> _modifyedParticles;
        private void OnParticleChanged(Particle obj)
        {
            var pt = _modifyedParticles.FirstOrDefault(p => p.ParticleId == obj.ParticleId);
            if (pt == null)
                _modifyedParticles.Add(obj);
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
