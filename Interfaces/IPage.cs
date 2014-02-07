using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IPage
    {
        Block Block { get; set; }
        Tag Tag { get; set; }
        bool IsBlockTag { get; set; }
    }
}