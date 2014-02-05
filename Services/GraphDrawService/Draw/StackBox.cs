using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    public class StackBox : IComponent
    {
        private const double Margin = 10.0;
        public List<IComponent> Childs { get; set; }
        public List<DrawingVisual> Render(Point p)
        {
            return DrawerFuncs.RenderStackLayout(p, Childs, Margin).ToList();
        }

        public Size GetSize()
        {
            return DrawerFuncs.CalculateSizeStackLayout(Childs, Margin);
        }
    }
}
