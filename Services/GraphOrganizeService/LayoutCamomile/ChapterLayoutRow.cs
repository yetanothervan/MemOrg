using System.Collections.Generic;
using System.Linq;
using MemOrg.Interfaces;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutRow
    {
        public ChapterLayoutRow()
        {
            Inner = false;
            FirstColumn = new List<ChapterLayoutElem>();
            SecondColumn = new List<ChapterLayoutElem>();
            ThirdColumn = new List<ChapterLayoutElem>();
            FourthColumn = new List<ChapterLayoutElem>();
            
            FirstSecond = false;
            SecondThird = false;
            ThirdFourth = false;
        }

        public bool FirstSecond;
        public bool SecondThird;
        public bool ThirdFourth;

        public int Height {
            get
            {
                return new List<int>
                {
                    FirstColumn.Sum(r => r.RowSpan),
                    SecondColumn.Sum(r => r.RowSpan),
                    ThirdColumn.Sum(r => r.RowSpan),
                    FourthColumn.Sum(r => r.RowSpan)
                }.Max();
            }
        }

        public List<ChapterLayoutElem> FirstColumn;
        public List<ChapterLayoutElem> SecondColumn;
        public List<ChapterLayoutElem> ThirdColumn;
        public List<ChapterLayoutElem> FourthColumn;

        public List<IPage> Pages;
        public bool Inner;
        public IChapter MyChapter;
    }
}