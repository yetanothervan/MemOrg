using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces.OrgUnits
{
    public interface IOrgBlock : IOrg
    {
        IPage Page { get; }
        IEnumerable<ConnectionPoint> ConnectionPoints { get; }
    }
}