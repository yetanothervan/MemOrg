using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemOrg.Interfaces
{
    public interface IPlanarGraph
    {
        IList<IPlanarGraphBlock> GetBlocks();
        GraphLayout GetGraphLayout();
    }
}
