using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class RelationMap : EntityTypeConfiguration<Relation>
    {
        public RelationMap()
        {
            this.HasRequired(t => t.FirstBlock).WithMany().HasForeignKey(t => t.FirstBlockId);
            this.HasRequired(t => t.SecondBlock)
                .WithMany()
                .HasForeignKey(t => t.SecondBlockId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.RelationType).WithMany().HasForeignKey(t => t.RelationTypeId);
            this.HasOptional(t => t.RelationBlock).WithMany().HasForeignKey(t => t.RelationBlockId);
        }
    }
}
