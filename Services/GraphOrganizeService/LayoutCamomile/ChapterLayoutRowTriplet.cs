using System.Linq;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutRowTriplet
    {
        private readonly ChapterLayoutRow _row;
        public ChapterLayoutRowTriplet(ChapterLayoutRow row)
        {
            _row = row;
        }

        private IPage Rel
        {
            get { return _row.Pages.First(r => r.IsBlockRel); }
        }

        private IPage BlockInChapter
        {
            get
            {
                return _row.Pages
                    .FirstOrDefault(b => !b.IsBlockRel
                                         && b.MyChapter != null
                                         && b.MyChapter.ChapterBlock.BlockId == _row.MyChapter.ChapterBlock.BlockId);
            }
        }

        private IPage Other(IPage block)
        {
            if (block == null) return null;
            return _row.Pages
                .FirstOrDefault(b => !b.IsBlockRel
                                     && b.Block.BlockId != block.Block.BlockId);
        }

        private bool IsInChapter(IPage block)
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

        private bool IsInBook(IPage block)
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
                    PlaceLayoutElem(new ChapterLayoutElem {Page = BlockInChapter}, 0, -2);
                    PlaceLayoutElem(NewGridLink(), 0, -1);
                    PlaceLayoutElem(new ChapterLayoutElem {Page = Rel}, 0, 0);
                    PlaceLayoutElem(NewGridLink(), 0, 1);
                    PlaceLayoutElem(new ChapterLayoutElem {Page = other}, 0, 2);
                    return;
                }

                PlaceLayoutElem(new ChapterLayoutElem {Page = BlockInChapter}, 0, 0);

                if (IsLeftNeightbor(other))
                {
                    PlaceLayoutElem(NewGridLink(), 0, -1);
                    PlaceLayoutElem(new ChapterLayoutElem {Page = Rel}, 0, -2);
                    return;
                }
                if (IsRightNeightbor(other))
                {
                    PlaceLayoutElem(NewGridLink(), 0, 1);
                    PlaceLayoutElem(new ChapterLayoutElem {Page = Rel}, 0, 2);
                    return;
                }
                if (IsInBook(other))
                {
                    PlaceLayoutElem(NewGridLink(), 0, 1);
                    PlaceLayoutElem(new ChapterLayoutElem {Page = Rel}, 0, 2);
                    _row.Inner = true;
                    return;
                }

                PlaceLayoutElem(NewGridLink(), 0, -1);
                PlaceLayoutElem(new ChapterLayoutElem {Page = Rel}, 0, -2);
                PlaceLayoutElem(NewGridLink(), 0, -3);
                PlaceLayoutElem(new ChapterLayoutElem {Page = other}, 0, -4);

                return;
            }

            PlaceLayoutElem(new ChapterLayoutElem {Page = Rel}, 0, 0);

            if (!IsLeftNeightbor(Rel.RelationFirst) && !IsRightNeightbor(Rel.RelationFirst)
                && !IsInBook(Rel.RelationFirst))
            {
                PlaceLayoutElem(NewGridLink(), 0, -1);
                PlaceLayoutElem(new ChapterLayoutElem {Page = Rel.RelationFirst}, 0, -2);
            }
            if (!IsLeftNeightbor(Rel.RelationSecond) && !IsRightNeightbor(Rel.RelationSecond)
                && !IsInBook(Rel.RelationSecond))
            {
                PlaceLayoutElem(NewGridLink(), 0, 1);
                PlaceLayoutElem(new ChapterLayoutElem {Page = Rel.RelationSecond}, 0, 2);
            }
        }

        private static ChapterLayoutElem NewGridLink()
        {
            return new ChapterLayoutElem
            {
                GridLinkPart = new GridLinkPart
                {Direction = GridLinkPartDirection.WestEast, Type = GridLinkPartType.Relation}
            };
        }

        private void PlaceLayoutElem(ChapterLayoutElem content, int row, int col)
        {
            var gridElem = new GridElem(_row) {Content = content};
            gridElem.PlaceOn(row, col);
        }
    }
}