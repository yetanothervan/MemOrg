﻿using System;
using System.Windows;
using System.Windows.Documents;
using Graph2Plane;
using Interfaces;
using TmpDal;

namespace BlocksMapViewerModule
{
    public class ContentViewModel : ViewModelBase
    {
        public ContentViewModel()
        {
            MyText = "Some of my texts";
            Graph = new PlaneGraph(new GraphLoader().GetGraph()); //GraphLoader should't be here
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