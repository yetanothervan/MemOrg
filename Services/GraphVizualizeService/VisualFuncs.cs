using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphVizualizeService
{
    public static class VisualFuncs
    {
        public static IComponent UserText(UserTextParticle userTextParticle, IDrawer drawer)
        {
            return drawer.DrawQuoteText(userTextParticle.Content);
        }

        public static IComponent QuoteSourceText(QuoteSourceParticle quoteSourceParticle, IDrawer drawer)
        {
            var t = drawer.DrawQuoteText(quoteSourceParticle.SourceTextParticle.Content);
            var qb = drawer.DrawQuoteBox();
            qb.AddChild(t);
            return qb;
        }

        public static IComponent SourceText(SourceTextParticle sourceTextParticle, IDrawer drawer)
        {
            return drawer.DrawQuoteText(sourceTextParticle.Content);
        }
    }
}
