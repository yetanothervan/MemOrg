using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridLink : IVisual
    {
        private readonly NESW _direction;

        public VisualGridLink(NESW direction)
        {
            _direction = direction;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            return drawer.DrawLink();
        }
    }
}
