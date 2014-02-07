using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public class RelationRepository : IRelationRepository
    {
        private MemOrgContext _context;
        public IQueryable<Relation> All
        {
            get
            {
                if (_context == null) _context = new MemOrgContext();
                return _context.Relations.AsNoTracking();
            }
        }
    }
}
