using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using Microsoft.Practices.Prism.Events;

namespace MemOrg.Interfaces
{
    public class PageSelected : CompositePresentationEvent<IPage>
    {
    }

    public class ParticleChanged : CompositePresentationEvent<Particle>
    {
    }

    public class BlockChanged : CompositePresentationEvent<Block>
    {
    }
    
    public class GraphChanged : CompositePresentationEvent<bool>
    {
    }
}
