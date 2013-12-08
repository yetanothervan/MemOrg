using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class ParagraphRef : Particle
    {
        public Paragraph Paragraph { get; set; }
    }
}
