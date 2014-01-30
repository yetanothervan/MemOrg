using System.Data.Entity.ModelConfiguration;

namespace DAL.Entity.Mapping
{
    public class UserTextParticleMap : EntityTypeConfiguration<UserTextParticle>
    {
        public UserTextParticleMap()
        {
            this.ToTable("UserTextParticle");
        }
    }
}
