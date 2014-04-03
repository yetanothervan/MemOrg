
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService
{
    public class GraphDrawService : IGraphDrawService
    {
        public IComponent PrepareGrid(IGrid grid)
        {
            return null;
        }

        public IDrawer GetDrawer(IDrawStyle style)
        {
            return new Drawer(style);
        }

        public IDrawStyle GetStyle()
        {
            return new DrawStyle();
        }


        public object GetByVisual(System.Windows.Media.Visual vis)
        {
            return GetByVisual(vis as object);
        }

        private object GetByVisual(object vis)
        {
            if (vis == null) return null;

            if (vis is LogicalBlock)
                return (vis as LogicalBlock).Data;
            
            if (vis is ContainerVisual)
                return GetByVisual((vis as ContainerVisual).Parent);

            return null;
        }
    }
}
