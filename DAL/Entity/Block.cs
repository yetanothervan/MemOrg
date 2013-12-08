using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;

namespace DAL.Entity
{
    public class Block
    {
        public Int32 BlockId { get; set; }
        public String Caption { get; set; }
        
        public virtual List<Particle> Particles { get; set; }
        public virtual List<Reference> References { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}
