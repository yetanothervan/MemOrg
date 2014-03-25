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
    public class StackBox : Component
    {
        private const double Margin = 10.0;
        
        public override List<DrawingVisual> Render(Point p1, Point? p2)
        {
            return new VerticalStackLayout(Childs, Margin).Render(p1).ToList();
        }

        public override Size GetActualSize()
        {
            return new VerticalStackLayout(Childs, Margin).CalculateSize();
        }
    }
}
