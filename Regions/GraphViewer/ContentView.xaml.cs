﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphViewer;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;


namespace GraphViewer
{
    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView
    {
        public ContentView(ContentViewModel viewModel)
        {
            InitializeComponent();
            
            
            Loaded += (sender, args) =>
            {
                DataContext = viewModel;
                InnerCanvas.CanvasHeight = ActualHeight;
                InnerCanvas.CanvasWidth = ActualWidth;
            };
            SizeChanged += (sender, args) =>
            {
                InnerCanvas.CanvasHeight = ActualHeight;
                InnerCanvas.CanvasWidth = ActualWidth;
            };

            /*RegionContext.GetObservableContext(this).PropertyChanged += (sender, args) =>
            {
                var vm = (ContentViewModel)this.Resources["ContentViewModel"];
                var context = (ObservableObject<object>) sender;
                vm.Grid = (PlaneGraph)context.Value;
            };*/
        }

        private void Window_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseLbDown)
            {
                if (!ContentGrid.IsMouseCaptured)
                    ContentGrid.CaptureMouse();
                var vm = (ContentViewModel) DataContext;
                if (vm != null)
                {
                    var curPosition = new Vector(e.GetPosition(this).X, e.GetPosition(this).Y);
                    vm.Offset = _startOffset - _startPosition + curPosition;
                    //vm.MyText = String.Format("{0}, {1}", vm.Offset.X, vm.Offset.Y);
                }
            }
        }

        private Vector _startPosition;
        private Vector _startOffset;
        private bool _mouseLbDown;

        private void Window_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = (ContentViewModel) DataContext;
            if (!_mouseLbDown && vm != null)
            {
                _mouseLbDown = true;
                _startPosition = new Vector(e.GetPosition(this).X, e.GetPosition(this).Y);
                _startOffset = vm.Offset;
                
            }
        }

        private void Window_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseLbDown)
            {
                _mouseLbDown = false;
                ContentGrid.ReleaseMouseCapture();
            }
        }
    }
}

