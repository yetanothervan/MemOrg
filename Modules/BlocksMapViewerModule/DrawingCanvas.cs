using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BlocksMapViewerModule
{
    public class DrawingCanvas : FrameworkElement
    {
        private readonly VisualCollection _visuals;

        public DrawingCanvas()
        {
            _visuals = new VisualCollection(this);
            var dv = new DrawingVisual();
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
            _visuals.Add(dv);
        }
        
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "Width" || e.Property.Name == "Height" || e.Property.Name == "Center")
            {
                //UpdateVisualChildren();
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
