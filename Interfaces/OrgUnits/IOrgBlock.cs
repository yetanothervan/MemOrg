using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces.OrgUnits
{
    public interface IOrgBlock
    {
        Block Block { get; }
        IEnumerable<NESW> ConnectionPoints { get; }
    }
}