using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphOrganizeService
    {
        IVisualGraph ProcessGraph(IGraph graph);
    }
}
