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

    public static class Grid2VisualGrid
    {
        public static VisualGrid Process(Grid grid, IDrawer drawer)
        {
            var vGrid = new VisualGrid(grid);
            foreach (var gridElem in grid)
            {

                
            }
        }

        public static void ProcessPlanarGraphElem(VisualGridElem gridElem)
        {
            if (gridElem is )
        }
    }
}
