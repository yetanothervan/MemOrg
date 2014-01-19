using System.Collections.Generic;
using System.Linq;
using DAL.Entity;
using EF;
using MemOrg.Interfaces;

namespace GraphService
{
    public class GraphService : IGraphService
    {
        readonly IBlockRepository _blockRepository;
        readonly ITagRepository _tagRepository;
        readonly IRelationRepository _relationRepository;
        
        public GraphService(IBlockRepository blockRepository, ITagRepository tagRepository, IRelationRepository relationRepository)
        {
            _blockRepository = blockRepository;
            _tagRepository = tagRepository;
            _relationRepository = relationRepository;
        }

        public List<Block> BlockTags
        {
            get { return _tagRepository.All.Where(tag => tag.TagBlock != null).Select(t => t.TagBlock).ToList(); }
        }

        public List<Block> BlockSources
        {
            get { return _blockRepository.All.Where(b => b.Particles.Any(p => p is SourceTextParticle)).ToList(); }
        }

        public List<Block> BlockRels
        {
            get { return _relationRepository.All.Where(rel => rel.RelationBlock != null).Select(r => r.RelationBlock).ToList(); }
        }

        public List<Block> BlockOthers
        {
            get
            {
                var res = _blockRepository.All.Select(b => b)
                    .Except(_tagRepository.All.Where(tag => tag.TagBlock != null).Select(t => t.TagBlock))
                    .Except(_relationRepository.All.Where(rel => rel.RelationBlock != null).Select(r => r.RelationBlock))
                    .Except(_blockRepository.All.Where(b => b.Particles.Any(p => p is SourceTextParticle)));

                return res.ToList();
            }
        }

        public List<Tag> TagsNoBlock
        {
            get { return _tagRepository.All.Where(tag => tag.TagBlock == null).ToList(); }
        }

        public List<Tag> TagsBlock
        {
            get { return _tagRepository.All.Where(tag => tag.TagBlock != null).ToList(); }
        }
    }
}
