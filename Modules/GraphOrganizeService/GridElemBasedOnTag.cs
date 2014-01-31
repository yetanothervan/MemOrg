using System.Dynamic;
using DAL.Entity;

namespace GraphOrganizeService
{
    public class GridElemBasedOnTag : GridElem
    {
        public GridElemBasedOnTag(Tag tag)
        {
            Tag = tag;
        }

        public Tag Tag { get; private set; }
    }
}