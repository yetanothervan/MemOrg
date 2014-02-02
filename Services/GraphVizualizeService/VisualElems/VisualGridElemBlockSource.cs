using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlockSource : VisualGridElem
    {
        private readonly IGridElemBlockSource _ge;
        public VisualGridElemBlockSource(IGridElemBlockSource ge) : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawBlockSource(_ge);
            var caption = drawer.DrawCaption(_ge.Block.Caption);
            res.Childs.Add(caption);
            if (options.HeadersOnly) return res;

            foreach (var part in _ge.Block.Particles.OrderBy(o => o.Order))
            {
                if (part is SourceTextParticle)
                    res.Childs.Add(VisualFuncs.SourceText(part as SourceTextParticle, drawer));
                else if (part is UserTextParticle)
                    res.Childs.Add(VisualFuncs.UserText(part as UserTextParticle, drawer));
                else
                    throw new NotImplementedException();
            }
            return res;
        }
    }
}