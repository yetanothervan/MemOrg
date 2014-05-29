using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using Microsoft.Practices.Prism.PubSubEvents;

namespace MemOrg.Interfaces
{
    public class PageSelected : PubSubEvent<IPage>
    {
    }

    public class ParticleChanged : PubSubEvent<Particle>
    {
    }

    public class BlockChanged : PubSubEvent<Block>
    {
    }

    public class GraphChanged : PubSubEvent<bool>
    {
    }
}
