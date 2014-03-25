using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MemOrg.Interfaces
{
    public interface IComponent
    {
        List<IComponent> Childs { get; set; }
        List<DrawingVisual> Render(Point p1, Point? p2);
        Size GetActualSize();
        HorizontalAligment HorizontalAligment { get; set; }
        VerticalAligment VerticalAligment{ get; set; }
    }

    public enum HorizontalAligment
    {
        Left, Right, Center, Stretch
    }

    public enum VerticalAligment
    {
        Top, Bottom, Center, Stretch
    }
}
