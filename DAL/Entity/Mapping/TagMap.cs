using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            this.HasOptional(t => t.Parent)
                .WithMany(t => t.Childs);
            
            this.HasMany(t => t.Blocks).WithMany(d => d.Tags);
        }
    }
}
