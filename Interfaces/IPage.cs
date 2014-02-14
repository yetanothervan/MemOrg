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
        BlockQuoteParticleSources RelFirstSources { get; set; }
        BlockQuoteParticleSources RelSecondSources { get; set; }
    }
}