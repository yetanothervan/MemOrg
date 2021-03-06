using System;
using MemOrg.Interfaces;

namespace GraphOrganizeService.Chapter
{
    public class PageEdge
    {
        private bool Equals(PageEdge other)
        {
            return (Equals(First, other.First) && Equals(Second, other.Second))
                   || Equals(Second, other.First) && Equals(First, other.Second);
        }

        public IPage GetOther(IPage my)
        {
            return First == my ? Second : First;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (First.Block.BlockId + Second.Block.BlockId).GetHashCode();
            }
        }

        public readonly IPage First;
        public readonly IPage Second;
        public readonly PageLink PageLink;

        public PageEdge(IPage first, IPage second, PageLink pageLink)
        {
            First = first;
            Second = second;
            PageLink = pageLink;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((PageEdge) obj);
        }

        public override string ToString()
        {
            return String.Format("{0} => {1}", First, Second);
        }
    }
}