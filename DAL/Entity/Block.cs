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
        
        //!!! выпилить после реализации параметров тега
        public String ParamName { get; set; }
        public Int32 ParamValue { get; set; }

        public virtual ICollection<Particle> Particles { get; set; }
        public virtual ICollection<Reference> References { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public override string ToString()
        {
            return Caption;
        }
    }
}
