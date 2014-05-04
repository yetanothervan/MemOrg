using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GraphViewer;
using MemOrg.Interfaces;

namespace GraphViewer
{
    public class DrawingCanvas : FrameworkElement
    {
        private VisualCollection _visuals;

        public static readonly DependencyProperty OffsetProperty;
        public static readonly DependencyProperty SourceProperty;
        
        static DrawingCanvas()
        {
            var offsetMetadata = new FrameworkPropertyMetadata(new Vector(), FrameworkPropertyMetadataOptions.AffectsRender);
            OffsetProperty = DependencyProperty.Register("Offset", typeof (Vector), typeof (DrawingCanvas), offsetMetadata);

            var graphSourceMetadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender);
            SourceProperty = DependencyProperty.Register("Source", typeof(IList<Visual>), typeof(DrawingCanvas), graphSourceMetadata);
        }


        public DrawingCanvas()
        {
            _visuals = new VisualCollection(this);
            MouseUp += (sender, args) =>
            {
                var dc = DataContext as ContentViewModel;
                if (dc != null)
                {
                    Point pt = args.GetPosition((UIElement) sender);
                    HitTestResult result = VisualTreeHelper.HitTest(this, pt);

                    if (result != null)
                    {
                        var a = (result.VisualHit as Visual);
                        if (a != null)
                            dc.VisualMouseDown(a);
                    }
                }
            };
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if ((/*e.Property.Name == "DataContext" || */e.Property.Name == "Source") && DataContext is ContentViewModel)
                Refresh();
        }

        public Vector Offset
        {
            set { SetValue(OffsetProperty, value); }
            get { return (Vector) GetValue(OffsetProperty); }
        }

        public IList<Visual> Source
        {
            set { SetValue(SourceProperty, value); }
            get { return (IList<Visual>)GetValue(SourceProperty); }
        }

        public void Refresh()
        {
            var dc = DataContext as ContentViewModel;
            if (dc != null && dc.Visuals != null)
            {
                _visuals.Clear();
                _visuals = new VisualCollection(this);
                foreach (var elem in dc.Visuals)
                {
                    _visuals.Add(elem);
                }
            }
        }
        
        protected override int VisualChildrenCount
        {
            get
            {
                return _visuals.Count;
            }
        }
        
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _visuals.Count)
                throw new ArgumentOutOfRangeException("index");
            return _visuals[index];
        }
    }
}
