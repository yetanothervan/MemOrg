using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlockRel : VisualGridElem
    {
        private readonly IGridElemBlockRel _ge;
        public VisualGridElemBlockRel(IGridElemBlockRel ge) : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Visualize(IDrawer drawer)
        {
            var res = drawer.DrawBox(_ge);
            var caption = drawer.DrawCaption(_ge.Block.Caption);
            res.Childs.Add(caption);
            foreach (var part in _ge.Block.Particles.OrderBy(o => o.Order))
            {
                if (part is UserTextParticle)
                    res.Childs.Add(VisualFuncs.UserText(part as UserTextParticle, drawer));
                else if (part is QuoteSourceParticle)
                    res.Childs.Add(VisualFuncs.QuoteSourceText(part as QuoteSourceParticle, drawer));
                else
                {
                    throw new NotImplementedException();
                }
            }
            return res;
        }
    }
}