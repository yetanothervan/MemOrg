using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemOrg.Interfaces
{
    public interface IVisualGraph
    {
        IList<IVisualGraphElem> GetGraphElems();
    }
}
