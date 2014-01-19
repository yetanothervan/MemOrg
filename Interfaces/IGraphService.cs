using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphService
    {
        /// <summary>
        /// Блоки, являющиеся телом для тага 
        /// </summary>
        List<Block> BlockTags { get; }
        
        /// <summary>
        /// Блоки источники, содержащие SourceTextParticle
        /// </summary>
        List<Block> BlockSources { get; }

        /// <summary>
        /// Блоки, являющиеся телом для реляций
        /// </summary>
        List<Block> BlockRels { get; }
        
        /// <summary>
        /// Любые другие блоки. Все блоки = BlockTags + BlockSources + BlockRels + BlockOthers;
        /// </summary>
        List<Block> BlockOthers { get; }

        /// <summary>
        /// Таги, у которых нет тела-блока. 
        /// </summary>
        List<Tag> TagsNoBlock { get; }

        /// <summary>
        /// Таги с телом-блоком
        /// </summary>
        List<Tag> TagsBlock { get; }
    }
}
