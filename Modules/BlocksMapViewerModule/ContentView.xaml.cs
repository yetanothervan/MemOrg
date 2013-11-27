using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace BlocksMapViewerModule
{
    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView
    {
        public ContentView()
        {
            InitializeComponent();
        }

        private void Window_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseLbDown)
            {
                var vm = (ContentViewModel)this.Resources["ContentViewModel"];
                if (vm != null)
                    vm.MyText = String.Format("{0}, {1}", e.GetPosition(this).X, e.GetPosition(this).Y);
            }
        }

        private bool _mouseLbDown;
        private void Window_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_mouseLbDown)
            {
                _mouseLbDown = true;
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

