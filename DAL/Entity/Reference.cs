using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Reference
    {
        public Int32 ReferenceId { get; set; }
        public String Caption { get; set; }
        public Block Block { get; set; }
    }
}
