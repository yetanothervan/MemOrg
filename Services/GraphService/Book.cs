using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    class Book : IBook
    {
        public Book()
        {
            ChaptersInternal = new List<IChapter>();
        }

        public string Caption
        {
            get { return CaptionInternal; }
        }
        
        public string CaptionInternal { get; set; }

        public IList<IChapter> Chapters
        {
            get { return ChaptersInternal; }
        }

        public IPage GetPageByBlock(int blockId)
        {
            foreach (var chapter in Chapters)
            {
                var page = chapter.PagesBlocks.FirstOrDefault(p => p.Block.BlockId == blockId);
                if (page != null) return page;
            }
            return null;
        }

        public IList<IChapter> ChaptersInternal { get; set; }
    }
}