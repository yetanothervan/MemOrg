using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    public class Page : IPage
    {
        public Block Block { get; set; }
        public Tag Tag { get; set; }
        public Relation Relation { get; set; }

        public bool IsBlockTag
        {
            get { return Tag != null; }
        }

        public bool IsBlockRel
        {
            get { return Relation != null; }
        }

        public BlockQuoteParticleSources MySources { get; set; }
        public BlockQuoteParticleSources RelFirstSources { get; set; }
        public BlockQuoteParticleSources RelSecondSources { get; set; }
    }
}