using System.Collections.Generic;
using System.Linq;
using MemOrg.Interfaces;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutRow : Grid
    {
        public ChapterLayoutRow()
        {
            Inner = false;
        }
        
        public List<IPage> Pages;
        public bool Inner;
        public IChapter MyChapter;
    }
}