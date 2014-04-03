using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            this.HasOptional(t => t.Parent)
                .WithMany(t => t.Childs)
                .HasForeignKey(t => t.ParentId);

            this.HasOptional(t => t.TagBlock)
                .WithMany()
                .HasForeignKey(t => t.TagBlockId);

            this.HasMany(t => t.Blocks).WithMany(d => d.Tags);
        }
    }
}
