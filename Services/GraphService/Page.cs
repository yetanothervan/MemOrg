using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    public class Page : IPage
    {
        public Page()
        {
            RelatedBy = new List<IPage>();
            ReferencedBy = new List<IPage>();
        }
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

        public IPage RelationFirst { get; set; }
        public IPage RelationSecond { get; set; }
        public IChapter MyChapter {
            get { return MyChapterInternal; }
        }

        public IList<IPage> ReferencedBy { get; private set; }
        public object Parent { get; set; }
        public IList<IPage> RelatedBy { get; private set; }
        public IChapter MyChapterInternal { get; set; }

        public override string ToString()
        {
            return Block != null ? Block.Caption : base.ToString();
        }
    }
}