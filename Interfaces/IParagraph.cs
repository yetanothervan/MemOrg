using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IParagraph
    {
        int ParticleId { get; set; }
        List<Block> UsedInBlocks { get; set; }
        ParagraphType ParagraphType { get; set; }
    }

    public class BlockParagraph : IParagraph
    {
        public BlockParagraph()
        {
            UsedInBlocks = new List<Block>();
            ParagraphType = ParagraphType.SourceNoQuotes;
        }
        public int ParticleId { get; set; }
        public List<Block> UsedInBlocks { get; set; }
        public ParagraphType ParagraphType { get; set; }
    }
}