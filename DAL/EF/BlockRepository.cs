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
        private readonly IMemOrgContext _context;

        public BlockRepository(IMemOrgContext context)
        {
            _context = context;
        }

        public IQueryable<Block> All
        {
            get
            {
                return _context.Blocks.AsNoTracking();
            }
        }

        public IQueryable<Block> BlockOthers
        {
            get
            {
                /*var res = _context.Blocks.Select(b => b)
                    .Except(_context.Tags.Where(tag => tag.TagBlock != null).Select(t => t.TagBlock))
                    .Except(_context.Relations.Where(rel => rel.RelationBlock != null).Select(r => r.RelationBlock))
                    .Except(_context.Blocks.Where(b => b.Particles.Any(p => p is SourceTextParticle)));*/
               /* var a = @"select BlockId from Blocks 
left join Tags on BlockId = TagBlock_BlockId
left join Relations on BlockId = RelationBlock_BlockId
left join (Particles t1 inner join SourceTextParticle t2 on t1.ParticleId = t2.ParticleId) on BlockId = t1.Block_BlockId
where TagId is null and RelationId is null and t1.ParticleId is null";*/
               
                var res =
                    _context.Blocks.AsNoTracking()

                    .GroupJoin(_context.Tags, block => block, tag => tag.TagBlock,
                        (block, tg) => new { block, tg })
                        .SelectMany(@t => @t.tg.DefaultIfEmpty(), (@t, tag) => new { @t, tag })
                        .Where(@t => @t.tag == null)
                        .Select(@t => @t.@t.block)

                        .GroupJoin(_context.Relations, block => block, rel => rel.RelationBlock,
                            (block, rel) => new { block, rel })
                        .SelectMany(@t => @t.rel.DefaultIfEmpty(), (@t, rel) => new { @t, rel })
                        .Where(@t => @t.rel == null)
                        .Select(@t => @t.@t.block)

                        .GroupJoin(
                            (from pb in _context.Particles where pb is SourceTextParticle select pb),
                            b1 => b1, b2 => b2.Block,
                            (b1, b2) => new { b1, b2 })
                        .SelectMany(@t => @t.b2.DefaultIfEmpty(), (@t, b2) => new { @t, b2 })
                        .Where(@t => @t.b2 == null)
                        .Select(@t => @t.@t.b1);
                
                return res;
            }
        }

        public IQueryable<Block> BlockSources
        {
            get
            {
                return
                    _context.Blocks.AsNoTracking()
                        .Where(b => !String.IsNullOrEmpty(b.ParamName) || b.Particles.Any(p => p is SourceTextParticle));
            }
        }

        public IQueryable<Block> BlockTags
        {
            get { return _context.Tags.AsNoTracking().Where(tag => tag.TagBlock != null).Select(t => t.TagBlock); }
        }

        public IQueryable<Block> BlockRels
        {
            get { return _context.Relations.AsNoTracking().Where(rel => rel.RelationBlock != null).Select(r => r.RelationBlock); }
        }

        public IQueryable<Block> Tracking
        {
            get { return _context.Blocks; }
        }


        public void AddBlock(Block block)
        {
            _context.Blocks.Add(block);
            _context.SaveChanges();
        }

        public void RemoveAllBlocks()
        {
            foreach (var b in  _context.Blocks)
                b.Tags.Clear();
            _context.SaveChanges();
            _context.References.RemoveRange(_context.References);
            _context.SaveChanges();
            foreach (var b in _context.Blocks)
                b.References.Clear();
            _context.SaveChanges();
            _context.Relations.RemoveRange(_context.Relations);
            _context.SaveChanges();
            _context.RelationTypes.RemoveRange(_context.RelationTypes);
            _context.SaveChanges();
            _context.Particles.RemoveRange(_context.Particles);
            _context.SaveChanges();
            _context.Tags.RemoveRange(_context.Tags);
            _context.SaveChanges();
            _context.Blocks.RemoveRange(_context.Blocks);
            _context.SaveChanges();

            //_context.Blocks.RemoveRange(Tracking);
            //_context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IMemOrgContext GetContext()
        {
            return _context;
        }
    }
}
