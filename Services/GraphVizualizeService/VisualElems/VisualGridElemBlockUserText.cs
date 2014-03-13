using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlockUserText
    {
        private readonly IOrgBlockUserText _org;
        public VisualGridElemBlockUserText(IOrgBlockUserText org)
        {
            _org = org;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawBlockUserText();
            var caption = drawer.DrawCaption(_org.Block.Caption);
            res.Childs.Add(caption);
            if (options.HeadersOnly) return res;

            foreach (var part in _org.Block.Particles.OrderBy(o => o.Order))
            {
                if (part is UserTextParticle)
                    res.Childs.Add(VisualFuncs.UserText(part as UserTextParticle, drawer));
                else
                {
                    throw new NotImplementedException();
                }
            }
            return res;
        }
    }
}