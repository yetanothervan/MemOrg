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
        private readonly IMemOrgContext _context;

        public RelationRepository(IMemOrgContext context)
        {
            _context = context;
        }

        public IQueryable<Relation> All
        {
            get { return _context.Relations.AsNoTracking(); }
        }

        public IQueryable<RelationType> RelationTypes
        {
            get { return _context.RelationTypes.AsNoTracking(); }
        }

        public void AddRelationType(RelationType relationType)
        {
            _context.RelationTypes.Add(relationType);
            _context.SaveChanges();
        }
        
        public void AddRelation(Relation relation)
        {
            _context.Relations.Add(relation);
            _context.SaveChanges();
        }
    }
}
