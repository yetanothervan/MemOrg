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
        private MemOrgContext _context;
        public IQueryable<Tag> All
        {
            get
            {
                if (_context == null) _context = new MemOrgContext();
                return _context.Tags;
            }
        }
    }
}
