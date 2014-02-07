using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class RelationMap : EntityTypeConfiguration<Relation>
    {
        public RelationMap()
        {
            this.HasRequired(t => t.FirstBlock).WithMany();
            this.HasRequired(t => t.SecondBlock).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(t => t.RelationType).WithMany();
        }
    }
}
