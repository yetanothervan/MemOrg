using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlockRel
    {
        private readonly IOrgBlockRel _org;
        public VisualGridElemBlockRel(IOrgBlockRel org)
        {
            _org = org;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawBlockRelation();
            var caption = drawer.DrawCaption(_org.Block.Caption);
            res.AddChild(caption);
            if (options.HeadersOnly) return res;

            foreach (var part in _org.Block.Particles.OrderBy(o => o.Order))
            {
                if (part is UserTextParticle)
                    res.AddChild(VisualFuncs.UserText(part as UserTextParticle, drawer));
                else if (part is QuoteSourceParticle)
                    res.AddChild(VisualFuncs.QuoteSourceText(part as QuoteSourceParticle, drawer));
                else
                {
                    throw new NotImplementedException();
                }
            }
            return res;
        }
    }
}