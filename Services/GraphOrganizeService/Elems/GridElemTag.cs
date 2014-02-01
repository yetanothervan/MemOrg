using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public class GridElemTag : GridElem, IGridElemTag
    {
        public GridElemTag(Tag tag, IGrid myGrid) : base(myGrid)
        {
            _tag = tag;
        }

        private readonly Tag _tag;
        public Tag Tag
        {
            get { return _tag; }
        }
    }
}