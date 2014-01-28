using DAL.Entity;

namespace GraphOrganizeService
{
    public class GridElemBasedOnTag : GridElem
    {
        private Tag _tag;
        public GridElemBasedOnTag(Tag tag)
        {
            _tag = tag;
        }
    }
}