using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService.VisualElems
{
    public class VisualGridElemTag
    {
        private readonly IOrgTag _org;
        public VisualGridElemTag(IOrgTag org)
        {
            _org = org;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            var res = drawer.DrawTag();
            var caption = drawer.DrawCaption(_org.Tag.Caption);
            res.Childs.Add(caption);
            return res;
        }
    }
}