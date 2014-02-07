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

        public IQueryable<Block> BlockTags
        {
            get { return _blockRepository.BlockTags; }
        }

        public IQueryable<Block> BlockSources
        {
            get { return _blockRepository.BlockSources; }
        }

        public IQueryable<Block> BlockRels
        {
            get { return _blockRepository.BlockRels; }
        }

        public IQueryable<Block> BlockOthers
        {
            get { return _blockRepository.BlockOthers; }
        }

        public IQueryable<Block> BlockAll
        {
            get { return _blockRepository.All; }
        }

        public IQueryable<Tag> TagsNoBlock
        {
            get { return _tagRepository.All.Where(tag => tag.TagBlock == null); }
        }

        public IQueryable<Tag> TagsBlock
        {
            get { return _tagRepository.All.Where(tag => tag.TagBlock != null); }
        }

        public IQueryable<Tag> TagRoots
        {
            get { return _tagRepository.All.Where(tag => tag.Parent == null); }
        }

        private IGraph _graph;
        public IGraph Graph
        {
            get { return _graph ?? (_graph = new Graph(this)); }
        }
    }
}
