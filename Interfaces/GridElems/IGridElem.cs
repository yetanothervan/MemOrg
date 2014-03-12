namespace MemOrg.Interfaces.GridElems
{
    public interface IGridElem
    {
        int RowIndex { get; }
        int ColIndex { get; }
        IComponent Component { get; set; }
    }
}