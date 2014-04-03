using System.Collections.Generic;
using System.Linq;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphService
    {
        /// <summary>
        /// Блоки, являющиеся телом для тага 
        /// </summary>
        IQueryable<Block> BlockTags { get; }
        
        /// <summary>
        /// Блоки источники, содержащие SourceTextParticle
        /// </summary>
        IQueryable<Block> BlockSources { get; }

        /// <summary>
        /// Блоки, являющиеся телом для реляций
        /// </summary>
        IQueryable<Block> BlockRels { get; }
        
        /// <summary>
        /// Любые другие блоки.
        /// </summary>
        IQueryable<Block> BlockOthers { get; }

        /// <summary>
        /// Все блоки = BlockTags + BlockSources + BlockRels + BlockOthers;
        /// </summary>
        IQueryable<Block> BlockAll { get; }


        /// <summary>
        /// Таги, у которых нет тела-блока. 
        /// </summary>
        IQueryable<Tag> TagsNoBlock { get; }

        /// <summary>
        /// Таги с телом-блоком
        /// </summary>
        IQueryable<Tag> TagsBlock { get; }

        /// <summary>
        /// Все таги
        /// </summary>
        IQueryable<Tag> TagsAll { get; }

        /// <summary>
        /// Таги корни или таги без родителей
        /// </summary>
        IQueryable<Tag> TagRoots { get; }

        /// <summary>
        /// Реляции с блоком
        /// </summary>
        IQueryable<Relation> RelationsBlock { get; }

        /// <summary>
        /// Реляции без блока
        /// </summary>
        IQueryable<Relation> RelationsNoBlock { get; }

        /// <summary>
        /// Типы реляций
        /// </summary>
        IQueryable<RelationType> RelationTypes { get; }
        
        /// <summary>
        /// Собственно, граф
        /// </summary>
        IGraph Graph { get; }
        
        void AddRelationType(RelationType relationType);
        void AddRelation(Relation relation);
        void AddBlock(Block block);
        IQueryable<Block> TrackingBlocks { get; }
        void AddTag(Tag tag);
        IQueryable<Tag> TrackingTags { get; }
        void SaveChanges();
    }
}
