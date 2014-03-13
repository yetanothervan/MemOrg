using System;

namespace MemOrg.Interfaces
{
    public interface IGridElem
    {
        int RowIndex { get; }
        int ColIndex { get; }
        Object Content { get; set; }
    }
}