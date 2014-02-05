using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphVizualizeService.VisualElems
{
    class VisualStackPanel : IVisual
    {
        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            return drawer.DrawStackBox();
        }
    }
}
