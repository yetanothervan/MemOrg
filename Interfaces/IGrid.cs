using System.Collections.Generic;
using MemOrg.Interfaces.GridElems;

namespace MemOrg.Interfaces
{
    public interface IGrid : IEnumerable<IGridElem>
    {
        int RowCount { get; }
        int RowLength { get; }
        void PlaceElem(int row, int col, IGridElem elem);
    }
}