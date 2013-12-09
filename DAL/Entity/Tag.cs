using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Tag
    {
        public Int32 TagId { get; set; }
        public virtual Block Block { get; set; }
    }
}
