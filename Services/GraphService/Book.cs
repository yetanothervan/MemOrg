using System.Collections.Generic;
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

        public IEnumerable<IChapter> Chapters
        {
            get { return ChaptersInternal; }
        }

        public List<IChapter> ChaptersInternal { get; set; }
    }
}