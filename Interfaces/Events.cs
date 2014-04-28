using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using Microsoft.Practices.Prism.Events;

namespace MemOrg.Interfaces
{
    public class BlockSelected : CompositePresentationEvent<Block>
    {
    }

    public class ParticleChanged : CompositePresentationEvent<int>
    {
    }
}
