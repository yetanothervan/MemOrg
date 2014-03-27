using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlockSource
    {
        private readonly IOrgBlockSource _org;
        public VisualGridElemBlockSource(IOrgBlockSource org)
        {
            _org = org;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawBlockSource();
            var caption = drawer.DrawCaption(_org.Block.Caption);
            res.AddChild(caption);
            if (options.HeadersOnly) return res;

            foreach (var part in _org.Block.Particles.OrderBy(o => o.Order))
            {
                if (part is SourceTextParticle)
                    res.AddChild(VisualFuncs.SourceText(part as SourceTextParticle, drawer));
                else if (part is UserTextParticle)
                    res.AddChild(VisualFuncs.UserText(part as UserTextParticle, drawer));
                else
                    throw new NotImplementedException();
            }
            return res;
        }
    }
}