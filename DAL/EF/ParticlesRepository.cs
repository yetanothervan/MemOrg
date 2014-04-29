using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public class ParticlesRepository : IParticlesRepository
    {
         private readonly IMemOrgContext _context;

        public ParticlesRepository(IMemOrgContext context)
        {
            _context = context;
        }

        public IQueryable<Particle> Tracking
        {
            get { return _context.Particles; }
        }

        public void RemoveParticle(Particle particle)
        {
            if (particle == null) return;
            var exist = _context.Particles.FirstOrDefault(p => p.ParticleId == particle.ParticleId);
            if (exist == null) return;
            _context.Particles.Remove(exist);
        }
    }
}
