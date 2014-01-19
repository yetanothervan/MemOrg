using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MemOrg.Interfaces
{
    public interface IComponent
    {
        List<IComponent> Childs { get; set; }
        List<DrawingVisual> Render();
        Size GetSize();
    }
}
