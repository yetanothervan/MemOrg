using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Relation
    {
        public Int32 RelationId { get; set; }
        public virtual RelationType RelationType { get; set; }
        public virtual Block FirstBlock { get; set; }
        public virtual Block SecondBlock { get; set; }
        public virtual Block RelationBlock { get; set; }
    }
}
