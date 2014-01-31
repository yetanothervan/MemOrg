﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public static class VisualFuncs
    {
        public static IComponent UserText(UserTextParticle userTextParticle, IDrawer drawer)
        {
            return drawer.DrawQuoteText(userTextParticle.Content);
        }

        public static IComponent QuoteSourceText(QuoteSourceParticle quoteSourceParticle, IDrawer drawer)
        {
            var t = drawer.DrawQuoteText(quoteSourceParticle.SourceTextParticle.Content);
            var qb = drawer.DrawQuoteBox();
            qb.Childs.Add(t);
            return qb;
        }

        public static IComponent SourceText(SourceTextParticle sourceTextParticle, IDrawer drawer)
        {
            return drawer.DrawQuoteText(sourceTextParticle.Content);
        }
    }
}
