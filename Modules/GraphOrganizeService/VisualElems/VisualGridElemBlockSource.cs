using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;
using GraphOrganizeService.Elems;
using MemOrg.Interfaces;

namespace GraphOrganizeService.VisualElems
{
    public class VisualGridElemBlockSource : VisualGridElem
    {
        private readonly GridElemBlockSource _ge;
        public VisualGridElemBlockSource(GridElemBlockSource ge) : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Prerender(IDrawer drawer)
        {
            var res = drawer.DrawBox(_ge);
            var caption = drawer.DrawCaption(_ge.Block.Caption);
            res.Childs.Add(caption);
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