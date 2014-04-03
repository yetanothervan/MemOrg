using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class QuoteSourceParticle : Particle
    {
        public Int32 SourceTextParticleId { get; set; }
        public virtual SourceTextParticle SourceTextParticle { get; set; }
    }
}
