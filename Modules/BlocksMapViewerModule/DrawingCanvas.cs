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
            SourceProperty = DependencyProperty.Register("Source", typeof(IGrid), typeof(DrawingCanvas), graphSourceMetadata);
        }


        public DrawingCanvas()
        {
            _visuals = new VisualCollection(this);            
        }
        
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if ((e.Property.Name == "DataContext" || e.Property.Name == "Source") && DataContext is ContentViewModel)
                Refresh();
        }

        public Vector Offset
        {
            set { SetValue(OffsetProperty, value); }
            get { return (Vector) GetValue(OffsetProperty); }
        }

        public IGrid Source
        {
            set { SetValue(SourceProperty, value); }
            get { return (IGrid)GetValue(SourceProperty); }
        }

        public void Refresh()
        {
            var dc = DataContext as ContentViewModel;
            if (dc != null && dc.Grid != null)
            {
                _visuals.Clear();
                _visuals = new VisualCollection(this);
                
                var dvtb = new DrawingVisual();
                using (var dvtbdc = dvtb.RenderOpen())
                    dvtbdc.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Transparent, 0),
                        new Rect(g2R.Layout.X, g2R.Layout.Y, g2R.Layout.Width, g2R.Layout.Height));
                _visuals.Add(dvtb);

                var elems = g2R.Render();
                foreach (var elem in elems)
                    _visuals.Add(elem);
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
