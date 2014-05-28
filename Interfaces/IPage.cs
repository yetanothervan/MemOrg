using System;
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
        bool IsBlockUserText { get; set; }
        bool IsBlockSource { get; set; }
        
        BlockQuoteParticleSources MySources { get; set; }
        IPage RelationFirst { get; set; }
        IPage RelationSecond { get; set; }
        IChapter MyChapter { get; }
        IList<IPageLink> LinksBy { get; }
    }
}