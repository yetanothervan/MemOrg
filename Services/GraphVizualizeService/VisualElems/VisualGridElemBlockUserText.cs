using System;
using System.Linq;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemBlockUserText : VisualGridElem
    {
        private readonly IGridElemBlockUserText _ge;
        public VisualGridElemBlockUserText(IGridElemBlockUserText ge)
            : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawBlockUserText(_ge);
            var caption = drawer.DrawCaption(_ge.Block.Caption);
            res.Childs.Add(caption);
            if (options.HeadersOnly) return res;

            foreach (var part in _ge.Block.Particles.OrderBy(o => o.Order))
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