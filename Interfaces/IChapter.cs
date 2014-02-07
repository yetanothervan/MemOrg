using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IChapter
    {
        Block ChapterBlock { get; set; }
        List<Block> PagesBlocks { get; set; }
    }
}