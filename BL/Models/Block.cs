using System.Collections.Generic;

namespace Models
{
    public class Block
    {
        public Block()
        {
            Paragraphs = new List<Paragraph>();
        }

        public string Caption { get; set; }
        public IList<Paragraph> Paragraphs { get; set; }
    }
}
