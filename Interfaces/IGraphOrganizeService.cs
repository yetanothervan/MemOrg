using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphOrganizeService
    {
        IPlanarGraph MakePlanarGraph(IList<Block> blocks);
    }
}
