using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphOrganizeService
{
    public interface IDrawer
    {
        IDrawText CreateDrawText();
    }

    public interface IDrawText
    {
    }

    public static class Grid2Render
    {
        public static void Process(Grid grid, IDrawer drawer)
        {
            foreach (var gridElem in grid)
            {
                
            }
        }

        public static void ProcessPlanarGraphElem(VisualGraphElem graphElem)
        {
            if (graphElem is )
        }
    }

    
}
