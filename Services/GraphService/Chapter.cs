using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    public class Chapter : IChapter
    {
        public Chapter()
        {
            PagesBlocks = new List<IPage>();
        }
        public IPage ChapterPage { get; set; }
        public List<IPage> PagesBlocks { get; set; }
        public IChapter NextChapter { get; set; }
        public IChapter PrevChapter { get; set; }
        public IBook MyBookInternal { get; set; }
        public IBook MyBook {
            get { return MyBookInternal; }
        }
    }
}