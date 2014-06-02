using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    public class Page : IPage
    {
        public Page()
        {
            LinksBy = new List<PageLink>();
            MyParagraphs = new List<IParagraph>();
            IsBlockUserText = false;
            IsBlockSource = false;
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

        public bool IsBlockUserText { get; set; }
        public bool IsBlockSource { get; set; }

        public BlockQuoteParticleSources MySources { get; set; }
        public List<IParagraph> MyParagraphs { get; set; }

        public IPage RelationFirst { get; set; }
        public IPage RelationSecond { get; set; }
        public IChapter MyChapter {
            get { return MyChapterInternal; }
        }

        public IList<PageLink> LinksBy { get; private set; }
        
        public IChapter MyChapterInternal { get; set; }

        public override string ToString()
        {
            return Block != null ? Block.Caption : base.ToString();
        }
    }
}