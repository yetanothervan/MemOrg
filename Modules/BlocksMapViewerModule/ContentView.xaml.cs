using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BL.Graph2Plane;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;


namespace BlocksMapViewer
{
    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView
    {
        public ContentView(ContentViewModel viewModel)
        {
            InitializeComponent();
            Loaded += (sender, args) => { DataContext = viewModel; };

            /*RegionContext.GetObservableContext(this).PropertyChanged += (sender, args) =>
            {
                var vm = (ContentViewModel)this.Resources["ContentViewModel"];
                var context = (ObservableObject<object>) sender;
                vm.Graph = (PlaneGraph)context.Value;
            };*/
        }

        private void Window_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseLbDown)
            {
                var vm = (ContentViewModel) DataContext;
                if (vm != null)
                {
                    var curPosition = new Vector(e.GetPosition(this).X, e.GetPosition(this).Y);
                    vm.Offset = _startOffset - _startPosition + curPosition;
                    vm.MyText = String.Format("{0}, {1}", vm.Offset.X, vm.Offset.Y);
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
                ContentGrid.CaptureMouse();
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

