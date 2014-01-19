using System.Linq;
using DAL.Entity;

namespace EF
{
    public interface ITagRepository
    {
        IQueryable<Tag> All { get; }
    }
}