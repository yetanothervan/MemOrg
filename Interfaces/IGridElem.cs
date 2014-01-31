using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IGridElem
    {
        int RowIndex { get; }
        int ColIndex { get; }
    }
}