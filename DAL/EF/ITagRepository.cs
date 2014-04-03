using System.Linq;
using DAL.Entity;

namespace EF
{
    public interface ITagRepository
    {
        IQueryable<Tag> All { get; }
        IQueryable<Tag> Tracking { get; }
        void AddTag(Tag tag);
    }
}