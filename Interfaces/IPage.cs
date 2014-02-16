using System.Collections;
using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IPage
    {
        Block Block { get; set; }
        Tag Tag { get; set; }
        Relation Relation { get; set; }
        bool IsBlockTag { get; }
        bool IsBlockRel { get; }
        
        BlockQuoteParticleSources MySources { get; set; }
        IPage RelationFirst { get; set; }
        IPage RelationSecond { get; set; }
        IChapter MyChapter { get; }
        IList<IPage> RelatedBy { get; }
    }
}