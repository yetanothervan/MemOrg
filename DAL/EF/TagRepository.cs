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
        private readonly MemOrgContext _context;

        public TagRepository()
        {
            if (_context == null) _context = new MemOrgContext();
        }

        public IQueryable<Tag> All
        {
            get
            {
                return _context.Tags.AsNoTracking();
            }
        }
    }
}
