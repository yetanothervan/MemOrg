using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Graph2Plane;

namespace BlocksMapViewerModule
{
    public class DrawingCanvas : FrameworkElement
    {
        private VisualCollection _visuals;

        public static readonly DependencyProperty OffsetProperty;
        
        static DrawingCanvas()
        {
            var metadata = new FrameworkPropertyMetadata(new Vector(), FrameworkPropertyMetadataOptions.AffectsRender);
            OffsetProperty = DependencyProperty.Register("Offset", typeof (Vector), typeof (DrawingCanvas), metadata);
        }


        public DrawingCanvas()
        {
            _visuals = new VisualCollection(this);
            /*var dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var b = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            var p = new Pen(b, 2);
            b.Freeze();
            p.Freeze();

            int count = 10000;
            int width = 800;
            double elemSideCount = Math.Sqrt(count);
            double elemSideSize = width/elemSideCount;


            for (double i = 0; i < elemSideCount; i += 1)
                for (double j = 0; j < elemSideCount; j += 1)
                {
                    double x = elemSideSize * i;
                    double y = elemSideSize * j;
                    dc.DrawLine(p, new Point(x, y), new Point(x + elemSideSize, y + elemSideSize));
                    dc.DrawLine(p, new Point(x + elemSideSize, y), new Point(x, y + elemSideSize));
                }

            dc.Close();
            _visuals.Add(dv);*/
        }
        
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "ActualWidth" || e.Property.Name == "ActualHeight" || e.Property.Name == "Center")
            {
                Refresh();
            }
            if (e.Property.Name == "DataContext" && DataContext is ContentViewModel)
            {
                Refresh();
            }
            if (e.Property.Name == "Offset" && DataContext is ContentViewModel)
            {
                //Refresh();
            }
        }

        public Vector Offset
        {
            set { SetValue(OffsetProperty, value); }
            get { return (Vector) GetValue(OffsetProperty); }
        }

        public void Refresh()
        {
            var dc = DataContext as ContentViewModel;
            if (dc != null)
            {
                _visuals.Clear();
                _visuals = new VisualCollection(this);

                double xmin = 0, xmax = 0, ymin = 0, ymax = 0;
                var blocks = dc.Graph.GetPlainBlocks();
                foreach (var planeBlock in blocks)
                {
                    _visuals.Add(planeBlock.Render(Offset.X, Offset.Y));
                    xmin = Math.Min(xmin, planeBlock.P1.X);
                    ymin = Math.Min(ymin, planeBlock.P1.Y);
                    xmax = Math.Max(xmax, planeBlock.P2.X);
                    ymax = Math.Max(ymax, planeBlock.P2.Y);
                }

                var dvtb = new DrawingVisual();
                using (var dvtbdc = dvtb.RenderOpen())
                    dvtbdc.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Transparent, 0),
                        new Rect(xmin, ymin, xmax, ymax));
                _visuals.Add(dvtb);

                var text = new FormattedText(dc.MyText,
                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Times New Roman"), 10,
                    Brushes.Black);
                var dv = new DrawingVisual();
                using (var drwc = dv.RenderOpen())
                    drwc.DrawText(text, new Point(0, 0));
                _visuals.Add(dv);
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
