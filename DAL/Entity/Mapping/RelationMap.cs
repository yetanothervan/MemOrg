using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class RelationMap : EntityTypeConfiguration<Relation>
    {
        public RelationMap()
        {
            this.HasRequired(t => t.FirstBlock).WithRequiredDependent();
            this.HasRequired(t => t.SecondBlock).WithRequiredDependent();
            this.HasOptional(t => t.RelationBlock).WithOptionalDependent();
            this.HasRequired(t => t.RelationType).WithRequiredDependent();
        }
    }
}
