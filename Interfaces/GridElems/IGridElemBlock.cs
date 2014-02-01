using DAL.Entity;

namespace MemOrg.Interfaces.GridElems
{
    public interface IGridElemBlock : IGridElem
    {
        Block Block { get; }
    }
}