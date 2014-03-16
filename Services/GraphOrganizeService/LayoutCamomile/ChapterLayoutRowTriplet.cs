using System.Linq;
using MemOrg.Interfaces;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutRowTriplet
    {
        private readonly ChapterLayoutRow _row;
        public ChapterLayoutRowTriplet(ChapterLayoutRow row)
        {
            _row = row;
        }

        public IPage Rel
        {
            get { return _row.Pages.First(r => r.IsBlockRel); }
        }

        public IPage BlockInChapter
        {
            get
            {
                return _row.Pages
                    .FirstOrDefault(b => !b.IsBlockRel
                                         && b.MyChapter != null
                                         && b.MyChapter.ChapterBlock.BlockId == _row.MyChapter.ChapterBlock.BlockId);
            }
        }

        public IPage Other(IPage block)
        {
            if (block == null) return null;
            return _row.Pages
                .FirstOrDefault(b => !b.IsBlockRel
                                     && b.Block.BlockId != block.Block.BlockId);
        }

        public bool IsInChapter(IPage block)
        {
            if (block == null || block.MyChapter == null ||
                block.MyChapter != _row.MyChapter) return false;
            return true;
        }

        private bool IsLeftNeightbor(IPage block)
        {
            if (block != null && block.MyChapter != null
                && block.MyChapter.NextChapter == _row.MyChapter) return true;
            return false;
        }

        private bool IsRightNeightbor(IPage block)
        {
            if (block != null && block.MyChapter != null 
                && block.MyChapter.PrevChapter == _row.MyChapter) return true;
            return false;
        }

        public bool IsInBook(IPage block)
        {
            if (block != null && block.MyChapter != null 
                && block.MyChapter.MyBook == _row.MyChapter.MyBook) return true;
            return false;
        }
        
        public void DoLayout()
        {
            if (BlockInChapter != null)
            {
                var other = Other(BlockInChapter);
                if (IsInChapter(other))
                {
                    _row.SecondColumn.Add(new ChapterLayoutElem {Page = BlockInChapter, RowSpan = 1});
                    _row.ThirdColumn.Add(new ChapterLayoutElem {Page = Rel, RowSpan = 1});
                    _row.FourthColumn.Add(new ChapterLayoutElem {Page = other, RowSpan = 1});
                    _row.SecondThird = true;
                    _row.ThirdFourth = true;
                    return;
                }

                _row.ThirdColumn.Add(new ChapterLayoutElem {Page = BlockInChapter, RowSpan = 1});

                if (IsLeftNeightbor(other))
                {
                    _row.SecondColumn.Add(new ChapterLayoutElem {Page = Rel, RowSpan = 1});
                    return;
                }
                if (IsRightNeightbor(other))
                {
                    _row.FourthColumn.Add(new ChapterLayoutElem {Page = Rel, RowSpan = 1});
                    return;
                }
                if (IsInBook(other))
                {
                    _row.FourthColumn.Add(new ChapterLayoutElem {Page = Rel, RowSpan = 1});
                    _row.Inner = true;
                    return;
                }

                _row.FirstColumn.Add(new ChapterLayoutElem {Page = other, RowSpan = 1});
                _row.SecondColumn.Add(new ChapterLayoutElem {Page = Rel, RowSpan = 1});
                _row.FirstSecond = true;
                _row.SecondThird = true;
                
                return;
            }

            _row.ThirdColumn.Add(new ChapterLayoutElem {Page = Rel, RowSpan = 1});
            if (!IsLeftNeightbor(Rel.RelationFirst) && !IsRightNeightbor(Rel.RelationFirst)
                && !IsInBook(Rel.RelationFirst))
                _row.SecondColumn.Add(new ChapterLayoutElem {Page = Rel.RelationFirst, RowSpan = 1});
            if (!IsLeftNeightbor(Rel.RelationSecond) && !IsRightNeightbor(Rel.RelationSecond)
                && !IsInBook(Rel.RelationSecond))
                _row.FourthColumn.Add(new ChapterLayoutElem { Page = Rel.RelationSecond, RowSpan = 1 });
        }
    }
}