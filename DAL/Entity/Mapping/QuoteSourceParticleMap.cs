using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class QuoteSourceParticleMap : EntityTypeConfiguration<QuoteSourceParticle>
    {
        public QuoteSourceParticleMap()
        {
            this.ToTable("QuoteSourceParticle");
        }
    }
}
