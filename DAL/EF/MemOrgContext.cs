using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public class MemOrgContext : DbContext
    {
        public MemOrgContext()
        {
            Database.SetInitializer(new Configuration());
        }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Particle> Particles { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<ParagraphRef> ParagraphRefs { get; set; }
    }
}
