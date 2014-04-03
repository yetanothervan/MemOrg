using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Tag
    {
        public Tag()
        {
            this.Childs = new List<Tag>();
            this.Blocks = new List<Block>();
        }
        public Int32 TagId { get; set; }
        public string Caption { get; set; }
        public Int32? ParentId { get; set; }
        public Int32? TagBlockId { get; set; }
        
        public virtual Tag Parent { get; set; }
        public virtual ICollection<Tag> Childs { get; set; }
        public virtual Block TagBlock { get; set; }
        public virtual ICollection<Block> Blocks { get; set; }
    }
}
