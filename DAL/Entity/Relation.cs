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

        public Int32 RelationTypeId { get; set; }
        public Int32 FirstBlockId { get; set; }
        public Int32 SecondBlockId { get; set; }
        public Int32? RelationBlockId { get; set; }

        public virtual RelationType RelationType { get; set; }
        public virtual Block FirstBlock { get; set; }
        public virtual Block SecondBlock { get; set; }
        public virtual Block RelationBlock { get; set; }
    }
}
