using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphService
    {
        IList<Block> Blocks { get; }
    }
}
