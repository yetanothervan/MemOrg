using System.Collections.Generic;
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

        public IList<IChapter> ChaptersInternal { get; set; }
    }
}