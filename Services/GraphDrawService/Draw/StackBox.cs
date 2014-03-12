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
        
        public override List<DrawingVisual> Render(Point p)
        {
            return new VerticalStackLayout(Childs, Margin).Render(p).ToList();
        }

        public override Size GetSize()
        {
            return new VerticalStackLayout(Childs, Margin).CalculateSize();
        }
    }
}
