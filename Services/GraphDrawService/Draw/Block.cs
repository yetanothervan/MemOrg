using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    abstract class Block : Component
    {
        protected readonly Brush Brush;
        protected readonly Pen Pen;

        protected const double Margin = 5.0;

        protected Block(Brush brush, Pen pen)
        {
            Brush = brush;
            Pen = pen;
        }
        
        public abstract override List<DrawingVisual> Render(Point p);

        public override Size GetSize()
        {
            return new VerticalStackLayout(Childs, Margin).CalculateSize();
        }
    }
}
