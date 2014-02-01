using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphVizualizeService.VisualElems
{
    public abstract class VisualGridElem : IVisual, IGridElem
    {
        private readonly IGridElem _ge;

        protected VisualGridElem(IGridElem ge)
        {
            this._ge = ge;
        }

        public int RowIndex
        {
            get { return _ge.RowIndex; }
        }

        public int ColIndex
        {
            get { return _ge.ColIndex; }
        }

        public abstract IComponent Visualize(IDrawer drawer);
    }
}