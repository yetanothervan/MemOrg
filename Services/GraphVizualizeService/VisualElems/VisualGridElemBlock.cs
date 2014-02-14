using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlock : VisualGridElem
    {
        private readonly IGridElemBlockOthers _ge;

        public VisualGridElemBlock(IGridElemBlockOthers ge) : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = _ge.Block.Particles.Count > 0 
                ? drawer.DrawBlockOthers(_ge)
                : drawer.DrawBlockOthersNoParticles(_ge);

            var caption = drawer.DrawCaption(_ge.Block.Caption);
            res.Childs.Add(caption);
            if (options.HeadersOnly) return res;

            foreach (var part in _ge.Block.Particles.OrderBy(o => o.Order))
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