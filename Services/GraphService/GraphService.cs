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
            get { return _blockRepository.BlockTags.ToList(); }
        }

        public List<Block> BlockSources
        {
            get { return _blockRepository.BlockSources.ToList(); }
        }

        public List<Block> BlockRels
        {
            get { return _blockRepository.BlockRels.ToList(); }
        }

        public List<Block> BlockOthers
        {
            get { return _blockRepository.BlockOthers.ToList(); }
        }

        public List<Block> BlockAll
        {
            get { return _blockRepository.All.ToList(); }
        }

        public List<Tag> TagsNoBlock
        {
            get { return _tagRepository.All.Where(tag => tag.TagBlock == null).ToList(); }
        }

        public List<Tag> TagsBlock
        {
            get { return _tagRepository.All.Where(tag => tag.TagBlock != null).ToList(); }
        }

        public List<Tag> TagRoots
        {
            get { return _tagRepository.All.Where(tag => tag.Parent == null).ToList(); }
        }
    }
}
