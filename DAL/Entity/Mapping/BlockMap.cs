using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class BlockMap : EntityTypeConfiguration<Block>
    {
        public BlockMap()
        {
            this.HasMany(t => t.Tags)
                .WithMany(t => t.Blocks)
                .Map(m =>
                {
                    m.ToTable("BlockTag");
                    m.MapLeftKey("Blocks_BlockId");
                    m.MapRightKey("Tags_TagId");
                });

            this.HasMany(t => t.Particles).WithRequired(d => d.Block);
            this.HasMany(t => t.References).WithRequired(d => d.Block);
        }
    }
}
