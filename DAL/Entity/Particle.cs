using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public abstract class Particle
    {
        public Int32 ParticleId { get; set; }
        public Int32 Order { get; set; }
        public virtual Block Block { get; set; }
    }
}
