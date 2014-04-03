using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public class TagRepository : ITagRepository
    {
        private readonly IMemOrgContext _context;

        public TagRepository(IMemOrgContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> All
        {
            get
            {
                return _context.Tags.AsNoTracking();
            }
        }

        public IQueryable<Tag> Tracking
        {
            get { return _context.Tags; }
        }

        public void AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }
    }
}
