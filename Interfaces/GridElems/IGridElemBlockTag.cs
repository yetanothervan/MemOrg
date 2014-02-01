using DAL.Entity;

namespace MemOrg.Interfaces.GridElems
{
    public interface IGridElemBlockTag : IGridElemBlock
    {
        Tag Tag { get; }
    }
}