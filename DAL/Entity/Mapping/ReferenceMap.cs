using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class ReferenceMap : EntityTypeConfiguration<Reference>
    {
        public ReferenceMap()
        {
            this.HasRequired(t => t.Block).WithMany();
            this.HasRequired(t => t.ReferencedBlock)
                .WithMany()
                .HasForeignKey(t => t.ReferencedBlockId)
                .WillCascadeOnDelete(false);
        }
    }
}
