using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public class BlockRepository : IBlockRepository
    {
        private readonly MemOrgContext _context;

        public BlockRepository()
        {
            if (_context == null) _context = new MemOrgContext();
        }
        
        public IQueryable<Block> All
        {
            get
            {
                return _context.Blocks;
            }
        }

        public IQueryable<Block> BlockOthers
        {
            get
            {
                var res = _context.Blocks.Select(b => b)
                    .Except(_context.Tags.Where(tag => tag.TagBlock != null).Select(t => t.TagBlock))
                    .Except(_context.Relations.Where(rel => rel.RelationBlock != null).Select(r => r.RelationBlock))
                    .Except(_context.Blocks.Where(b => b.Particles.Any(p => p is SourceTextParticle)));
                return res;
            }
        }

        public IQueryable<Block> BlockSources
        {
            get { return _context.Blocks.Where(b => b.Particles.Any(p => p is SourceTextParticle)); }
        }

        public IQueryable<Block> BlockTags
        {
            get { return _context.Tags.Where(tag => tag.TagBlock != null).Select(t => t.TagBlock); }
        }

        public IQueryable<Block> BlockRels
        {
            get { return _context.Relations.Where(rel => rel.RelationBlock != null).Select(r => r.RelationBlock); }
        }
    }
}
