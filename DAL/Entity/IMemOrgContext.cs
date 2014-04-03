using System.Data.Entity;
using DAL.Entity;

namespace EF
{
    public interface IMemOrgContext
    {
        DbSet<Block> Blocks { get; set; }
        DbSet<Particle> Particles { get; set; }
        DbSet<SourceTextParticle> Texts { get; set; }
        DbSet<UserTextParticle> Comments { get; set; }
        DbSet<QuoteSourceParticle> Quotes { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Reference> References { get; set; }
        DbSet<RelationType> RelationTypes { get; set; }
        DbSet<Relation> Relations { get; set; }
        int SaveChanges();
    }
}