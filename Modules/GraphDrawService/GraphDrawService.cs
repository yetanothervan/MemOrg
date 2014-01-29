
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
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
    }
}
