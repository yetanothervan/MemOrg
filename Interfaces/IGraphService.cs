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
        /// Таги корни или таги без родителей
        /// </summary>
        IQueryable<Tag> TagRoots { get; }

        /// <summary>
        /// Собственно, граф
        /// </summary>
        IGraph Graph { get; }
    }
}
