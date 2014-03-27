using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MemOrg.Interfaces
{
    public interface IComponent
    {
        IEnumerable<IComponent> Childs { get; }
        void AddChild(IComponent child);
        List<DrawingVisual> Render(Point p);
        Size GetActualSize();
        Size? PreferSize { get; set; }
    }
}
