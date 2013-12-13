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
        public static readonly DependencyProperty GraphSourceProperty;
        
        static DrawingCanvas()
        {
            var offsetMetadata = new FrameworkPropertyMetadata(new Vector(), FrameworkPropertyMetadataOptions.AffectsRender);
            OffsetProperty = DependencyProperty.Register("Offset", typeof (Vector), typeof (DrawingCanvas), offsetMetadata);

            var graphSourceMetadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender);
            GraphSourceProperty = DependencyProperty.Register("GraphSource", typeof(IPlanarGraph), typeof(DrawingCanvas), graphSourceMetadata);
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
            if ((e.Property.Name == "DataContext" || e.Property.Name == "GraphSource") && DataContext is ContentViewModel)
            {
                Refresh();
            }
        }

        public Vector Offset
        {
            set { SetValue(OffsetProperty, value); }
            get { return (Vector) GetValue(OffsetProperty); }
        }

        public IPlanarGraph GraphSource
        {
            set { SetValue(GraphSourceProperty, value); }
            get { return (IPlanarGraph)GetValue(GraphSourceProperty); }
        }

        public void Refresh()
        {
            var dc = DataContext as ContentViewModel;
            if (dc != null && dc.Graph != null)
            {
                _visuals.Clear();
                _visuals = new VisualCollection(this);
                
                var blocks = dc.Graph.GetBlocks();
                foreach (var planeBlock in blocks)
                    _visuals.Add(planeBlock.Render(Offset.X, Offset.Y));

                GraphLayout gl = dc.Graph.GetGraphLayout();
                var dvtb = new DrawingVisual();
                using (var dvtbdc = dvtb.RenderOpen())
                    dvtbdc.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Transparent, 0),
                        new Rect(gl.X1, gl.Y1, gl.X2, gl.Y2));
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
