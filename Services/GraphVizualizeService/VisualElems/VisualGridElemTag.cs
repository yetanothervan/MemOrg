using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;
using GraphVizualizeService.VisualElems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.VisualElems
{
    public class VisualGridElemTag : VisualGridElem
    {
        private readonly IGridElemTag _ge;
        public VisualGridElemTag(IGridElemTag ge) : base(ge)
        {
            _ge = ge;
        }

        public override IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawTag(_ge);
            var caption = drawer.DrawCaption(_ge.Tag.Caption);
            res.Childs.Add(caption);
            return res;
        }
    }
}