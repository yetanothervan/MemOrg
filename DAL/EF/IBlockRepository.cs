using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public interface IBlockRepository
    {
        IQueryable<Block> All { get; }
        IQueryable<Block> BlockOthers { get; }
        IQueryable<Block> BlockSources { get; }
        IQueryable<Block> BlockTags { get; }
        IQueryable<Block> BlockRels { get; }
        IQueryable<Block> Tracking { get; }
        void AddBlock(Block block);
        void SaveChanges();
    }
}
