using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using DAL.Entity.Mapping;

namespace EF
{
    public class MemOrgContext : DbContext, IMemOrgContext
    {

#if testbase
        public MemOrgContext() : base("MemOrgTest")
        {
#else
        public MemOrgContext()
        {
#endif
            Database.SetInitializer(new Configuration());
        }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Particle> Particles { get; set; }
        public DbSet<SourceTextParticle> Texts { get; set; }
        public DbSet<UserTextParticle> Comments { get; set; }
        public DbSet<QuoteSourceParticle> Quotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<RelationType> RelationTypes { get; set; }
        public DbSet<Relation> Relations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new QuoteSourceParticleMap());
            modelBuilder.Configurations.Add(new SourceTextParticleMap());
            modelBuilder.Configurations.Add(new UserTextParticleMap());
            modelBuilder.Configurations.Add(new ParticleMap());
            modelBuilder.Configurations.Add(new BlockMap());
            modelBuilder.Configurations.Add(new TagMap());
            modelBuilder.Configurations.Add(new ReferenceMap());
            modelBuilder.Configurations.Add(new RelationMap());
        }
    }
}
