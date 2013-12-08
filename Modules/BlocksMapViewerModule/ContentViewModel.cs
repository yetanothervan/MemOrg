using System;
using System.Windows;
using System.Windows.Documents;
using BL.Graph2Plane;
using Interfaces;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;


namespace BlocksMapViewer
{
    public class ContentViewModel : ViewModelBase
    {
        public ContentViewModel(IGraphService graphService)
        {
            MyText = "Some of my texts";
            Graph = new PlaneGraph(graphService.Blocks);
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

        private PlaneGraph _graph;
        public PlaneGraph Graph
        {
            get
            {
                return _graph;
            }
            set
            {
                if (_graph != value)
                {
                    _graph = value;
                    RaisePropertyChangedEvent("Graph");
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
