using DAL.Entity;

namespace MemOrg.Interfaces.OrgUnits
{
    public interface IOrgTag : IOrg
    {
        Tag Tag { get; }
    }
}