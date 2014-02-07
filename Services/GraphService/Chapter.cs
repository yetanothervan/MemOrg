using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    class Chapter : IChapter
    {
        public Chapter()
        {
            PagesBlocks = new List<Block>();
        }
        public Block ChapterBlock { get; set; }
        public List<Block> PagesBlocks { get; set; }
    }
}