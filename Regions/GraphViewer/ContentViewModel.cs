﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;

namespace GraphViewer
{
    public class ContentViewModel : ViewModelBase
    {
        private readonly IGraphVizualizeService _graphVizualizeService;
        private readonly IDrawer _drawer;
        private IVisualizeOptions _options;

        readonly IGrid _rawGrid;
        readonly IGrid _camoGrid;
        readonly IGrid _tagTrees;

        public ContentViewModel(IGraphOrganizeService graphOrganizeService, 
            IGraphDrawService graphDrawService, IGraphVizualizeService graphVizualizeService)
        {
            _graphVizualizeService = graphVizualizeService;
            var headersToggleCommand = new DelegateCommand(ToggleHeaders, () => true);
            GlobalCommands.ToggleHeadersCompositeCommand.RegisterCommand(headersToggleCommand);

            MyText = "Some of my texts";
            IGraph graph = graphOrganizeService.GetGraph(null);
            IGridLayout rawLayout = graphOrganizeService.GetFullLayout(graph);
            IGridLayout camoLayout = graphOrganizeService.GetLayout(graph);
            ITreeLayout tagLayout = graphOrganizeService.GetTagLayout(graph);

            _camoGrid = camoLayout.CreateGrid();
            _rawGrid = rawLayout.CreateGrid();
            _tagTrees = tagLayout.CreateTreesGrid();

            IDrawStyle style = graphDrawService.GetStyle();
            _drawer = graphDrawService.GetDrawer(style);

            _options = _graphVizualizeService.GetVisualizeOptions();
            UpdateGrid(_options);
        }

        private void UpdateGrid(IVisualizeOptions options)
        {
            var rawVisGrid = _graphVizualizeService.VisualizeGrid(_rawGrid, options, _drawer);
            var camoVisGrid = _graphVizualizeService.VisualizeGrid(_camoGrid, options, _drawer);
            var tagVisTree = _graphVizualizeService.VisualizeGrid(_tagTrees, options, _drawer);

            var stack = _graphVizualizeService.StackPanel(options, _drawer);
            stack.Childs = new List<IComponent> { tagVisTree, camoVisGrid, rawVisGrid };

            Grid = stack;
        }
        private bool _headersOnly = true;

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