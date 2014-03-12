using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemOrg.Interfaces.GridElems
{
    public class GridLink : IGridLink
    {
        public GridLinkPoint Begin { get; set; }
        public GridLinkPoint End { get; set; }
    }

    public interface IGridLink
    {
        GridLinkPoint Begin { get; }
        GridLinkPoint End { get; }
    }

    public struct GridLinkPoint
    {
        public NESW ConnectionPoint;
        public int Row;
        public int Col;
        public IComponent GridElem;
    }

    public enum NESW
    {
        North, East, South, West
    }
}
