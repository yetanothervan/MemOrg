using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlockTag 
    {
        private readonly IOrgBlockTag _org;
        public VisualGridElemBlockTag(IOrgBlockTag org)
        {
            _org = org;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawBlockTag();
            var caption = drawer.DrawCaption(_org.Tag.Caption);
            res.Childs.Add(caption);
            if (options.HeadersOnly) return res;

            foreach (var part in _org.Block.Particles.OrderBy(o => o.Order))
            {
                if (part is UserTextParticle)
                    res.Childs.Add(VisualFuncs.UserText(part as UserTextParticle, drawer));
                else if (part is QuoteSourceParticle)
                    res.Childs.Add(VisualFuncs.QuoteSourceText(part as QuoteSourceParticle, drawer));
                else
                    throw new NotImplementedException();
            }
            return res;
        }
    }
}