using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class ReferenceMap : EntityTypeConfiguration<Reference>
    {
        public ReferenceMap()
        {
            this.HasRequired(t => t.Block).WithMany();
        }
    }
}
