using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class ParticleMap : EntityTypeConfiguration<Particle>
    {
        public ParticleMap()
        {
            this.HasRequired(t => t.Block).WithMany();
        }
    }
}
