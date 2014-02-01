using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService
{
    public static class DrawerFuncs
    {
        public static Size CalculateSizeStackLayout(List<IComponent> childs, double margin)
        {
            double height = childs.Sum(child => (child.GetSize().Height + margin)) + margin;
            double width = childs.Max(child => (child.GetSize().Width)) + margin * 2;
            return new Size { Height = height, Width = width };
        }

        public static IEnumerable<DrawingVisual> 
            RenderStackLayout(Point p, IEnumerable<IComponent> childs, double margin)
        {
            var result = new List<DrawingVisual>();
            
            var curPt = p;
            curPt.Offset(margin, margin);
            foreach (var child in childs)
            {
                result.AddRange(child.Render(curPt));
                curPt.Offset(0.0, child.GetSize().Height + margin);
            }
            return result;
        }

    }
}
