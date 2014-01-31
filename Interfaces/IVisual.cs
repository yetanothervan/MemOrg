using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MemOrg.Interfaces
{
    public interface IVisual
    {
        IComponent Prerender(IDrawer drawer);
    }
}