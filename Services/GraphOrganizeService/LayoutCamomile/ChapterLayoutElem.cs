using MemOrg.Interfaces;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutElem
    {
        public ChapterLayoutElem()
        {
            Placed = false;
            Row = 0;
            Col = 0;
        }
        public IPage Page
        {
            get { return _page; }
            set
            {
                _page = value;
                _page.Parent = this;
            }
        }

        public int RowSpan;
        private IPage _page;

        public bool Placed;
        public int Row;
        public int Col;
    }
}