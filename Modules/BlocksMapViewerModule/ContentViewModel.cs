using System;
using System.Windows;
using MemOrg.Interfaces;

namespace GraphViewer
{
    public class ContentViewModel : ViewModelBase
    {
        private readonly IGraphDrawService _graphDrawService;

        public ContentViewModel(IGraphService graphService, IGraphOrganizeService graphOrganizeService, IGraphDrawService graphDrawService)
        {
            _graphDrawService = graphDrawService;
            MyText = "Some of my texts";
            Grid = graphOrganizeService.ProcessGraph(graphService.Graph);
        }
        
        public IGraphDrawService GraphDrawService { get { return _graphDrawService; } }

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

        private IGrid _grid;
        public IGrid Grid
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
