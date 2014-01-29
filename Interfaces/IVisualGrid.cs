using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MemOrg.Interfaces
{
    public interface IVisualGrid : IGrid
    {
        void Prerender(IDrawer drawer);
        List<DrawingVisual> Render(Point p);
    }
}