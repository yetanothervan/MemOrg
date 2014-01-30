using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IGridElem
    {
        void PlaceOn(int row, int col, List<List<IGridElem>> elems);
        int RowIndex { get; }
        int ColIndex { get; }
    }
}