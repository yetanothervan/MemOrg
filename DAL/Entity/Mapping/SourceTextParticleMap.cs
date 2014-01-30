using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class SourceTextParticleMap : EntityTypeConfiguration<SourceTextParticle>
    {
        public SourceTextParticleMap()
        {
            this.ToTable("SourceTextParticle");
        }
    }
}
