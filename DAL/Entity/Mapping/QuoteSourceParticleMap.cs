using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class QuoteSourceParticleMap : EntityTypeConfiguration<QuoteSourceParticle>
    {
        public QuoteSourceParticleMap()
        {
            this.ToTable("QuoteSourceParticle");
            this.HasRequired(q => q.SourceTextParticle)
                .WithMany()
                .HasForeignKey(f => f.SourceTextParticleId);
        }
    }
}
