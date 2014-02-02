﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;
using GraphVizualizeService;
using GraphVizualizeService.VisualElems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.VisualElems
{
    public class VisualGridElemBlockTag : VisualGridElem
    {
        private readonly IGridElemBlockTag _ge;
        public VisualGridElemBlockTag(IGridElemBlockTag ge) : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawBlockTag(_ge);
            var caption = drawer.DrawCaption(_ge.Tag.Caption);
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