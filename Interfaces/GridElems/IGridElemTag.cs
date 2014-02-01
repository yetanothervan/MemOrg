using DAL.Entity;

namespace MemOrg.Interfaces.GridElems
{
    public interface IGridElemTag : IGridElem
    {
        Tag Tag { get; }
    }
}