using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;
using GraphOrganizeService.Elems;
using MemOrg.Interfaces;

namespace GraphOrganizeService.VisualElems
{
    public class VisualGridElemTag : VisualGridElem
    {
        private readonly GridElemTag _ge;
        public VisualGridElemTag(GridElemTag ge) : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Prerender(IDrawer drawer)
        {
            var res = drawer.DrawBox(_ge);
            var caption = drawer.DrawCaption(_ge.Tag.Caption);
            res.Childs.Add(caption);
            return res;
        }
    }
}