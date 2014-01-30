using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;

namespace DAL.Entity
{
    public class Block
    {
        public Block()
        {
            Particles = new List<Particle>();
            References = new List<Reference>();
            Tags = new List<Tag>();
        }
        
        public Int32 BlockId { get; set; }
        public String Caption { get; set; }
        
        public virtual ICollection<Particle> Particles { get; set; }
        public virtual ICollection<Reference> References { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
