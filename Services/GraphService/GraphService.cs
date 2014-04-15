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

        public IQueryable<Tag> TagsAll
        {
            get { return _tagRepository.All; }
        }

        public IQueryable<Tag> TagRoots
        {
            get { return _tagRepository.All.Where(tag => tag.Parent == null); }
        }

        public IQueryable<Relation> RelationsBlock
        {
            get { return _relationRepository.All.Where(rel => rel.RelationBlock != null); }
        }

        public IQueryable<Relation> RelationsNoBlock
        {
            get { return _relationRepository.All.Where(rel => rel.RelationBlock == null); }
        }

        public IGraph GetGraph()
        {
            return new Graph(this);
        }

        public IQueryable<RelationType> RelationTypes
        {
            get { return _relationRepository.RelationTypes; }
        }


        public void AddRelationType(RelationType relationType)
        {
            _relationRepository.AddRelationType(relationType);
        }

        public void AddRelation(Relation relation)
        {
            _relationRepository.AddRelation(relation);
        }

        public void AddBlock(Block block)
        {
            _blockRepository.AddBlock(block);
        }

        public IQueryable<Block> TrackingBlocks { get { return _blockRepository.Tracking; } }

        public void AddTag(Tag tag)
        {
            _tagRepository.AddTag(tag);
        }

        public IQueryable<Tag> TrackingTags {
            get { return _tagRepository.Tracking; }
        }

        public void SaveChanges()
        {
            _blockRepository.SaveChanges();
        }

        public void ClearGraph()
        {
            _blockRepository.RemoveAllBlocks();
        }
    }
}
