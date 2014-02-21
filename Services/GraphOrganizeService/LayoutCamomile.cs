using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using GraphOrganizeService.Elems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;
using MoreLinq;

namespace GraphOrganizeService
{
    public class LayoutCamomile : IGridLayout
    {
        private readonly IGraph _graph;

        public LayoutCamomile(IGraph graph)
        {
            _graph = graph;
        }
        
        public IGrid CreateGrid()
        {
            var grid = new Grid();
            foreach (var book in _graph.Books)
            {
                int chapCount = 0;
                foreach (var chapter in book.Chapters)
                {
                    //let's fill rows
                    var rows = new List<ChapterLayoutRow>();
                    
                    //and pour out pagesSet
                    var pagesSet = new HashSet<IPage>(chapter.PagesBlocks);
                    
                    while (pagesSet.Count > 0)
                    {
                        var page = pagesSet.FirstOrDefault(r => !r.IsBlockRel);
                        if (page == null) throw new ArgumentException();

                        rows.Add(
                            YeildChapterLayoutRow(pagesSet,
                                ExtractRowForBlockInChapter(pagesSet, chapter.ChapterBlock.BlockId, page, null)));
                    }

                    rows.ForEach(r => r.MyChapter = chapter);
                    rows.ForEach(ApplyLayout);
                    rows.ForEach(ProvideReferences);
                    
                    var layout = new ChapterLayout {Rows = rows, ChapterBlock = chapter.ChapterBlock};

                    DoChapterLayout(layout, grid, chapCount++);
                }
            }
            return grid;
        }

        private void ProvideReferences(ChapterLayoutRow row)
        {
            CheckColumn(row.FirstColumn, row);
            CheckColumn(row.SecondColumn, row);
            CheckColumn(row.ThirdColumn, row);
            CheckColumn(row.FourthColumn, row);
        }

        private void CheckColumn(List<ChapterLayoutElem> column, ChapterLayoutRow row)
        {
            var toAdd = new List<ChapterLayoutElem>();
            foreach (var elem in column.Where(c => c.Page.ReferencedBy.Any()))
                foreach (var page in elem.Page.ReferencedBy)
                    if (!row.MyChapter.PagesBlocks.Contains(page))
                        toAdd.Add(new ChapterLayoutElem { Page = page, RowSpan = 1 });
            column.AddRange(toAdd);
        }

        private void ApplyLayout(ChapterLayoutRow row)
        {
            if (row.Pages == null || row.Pages.Count == 0) return;

            if (row.Pages.Count == 1)
            {
                row.ThirdColumn.Add(
                    new ChapterLayoutElem {Page = row.Pages[0], RowSpan = 1});
                if (row.Pages[0].MySources == BlockQuoteParticleSources.MyBook)
                    row.Inner = true;
                return;
            }

            if (row.Pages.Count == 3)
            {
                var trip = new ChapterLayoutRowTriplet(row);
                trip.DoLayout();
                return;
            }

            //complicated case
            var rels = new HashSet<IPage>();

            var pages = row.Pages.Where(p => !p.IsBlockRel).OrderByDescending(r => r.RelatedBy.Count).ToList();
            foreach (var page in pages)
            {
                var relsInPage = page.RelatedBy.Where(r => r.MyChapter == row.MyChapter).Except(rels).ToList();
                
                if (relsInPage.Any())
                {
                    row.SecondColumn.Add(new ChapterLayoutElem { Page = page, RowSpan = relsInPage.Count });

                    foreach (var p in relsInPage)
                    {
                        row.ThirdColumn.Add(new ChapterLayoutElem { Page = p, RowSpan = 1 });
                        var oppose = p.RelationFirst.Block.BlockId != page.Block.BlockId ? p.RelationFirst : p.RelationSecond;
                        row.FourthColumn.Add(new ChapterLayoutElem { Page = oppose, RowSpan = 1 });
                        rels.Add(p);
                    }
                }
            }
        }


        private IEnumerable<IPage> ExtractRowForBlockInChapter(HashSet<IPage> @from, int chapterId, IPage blockPage, List<IPage> res)
        {
            if (res == null)
                res = new List<IPage> { blockPage };
            else
                if (!res.Contains(blockPage)) res.Add(blockPage);
            
            if (blockPage.RelatedBy.Count == 0) return res;

            var toAdd = blockPage.RelatedBy.Where(r => !res.Contains(r)
                && r.MyChapter.ChapterBlock.BlockId == chapterId).ToList();
            res.AddRange(toAdd);

            foreach (var rel in toAdd)
            {
                var next = rel.RelationFirst.Block.BlockId == blockPage.Block.BlockId
                    ? rel.RelationSecond
                    : rel.RelationFirst;
                ExtractRowForBlockInChapter(@from, chapterId, next, res);
            }

            return res;
        }


        private ChapterLayoutRow YeildChapterLayoutRow(HashSet<IPage> @from, IEnumerable<IPage> whatList)
        {
            var enumerable = whatList as IList<IPage> ?? whatList.ToList();
            @from.RemoveWhere(enumerable.Contains);
            return new ChapterLayoutRow { Pages = enumerable.ToList() };
        }

        private void DoChapterLayout(ChapterLayout layout, Grid grid, int chapCount)
        {
            //source elem
            var selem = new GridElemBlockSource(layout.ChapterBlock, grid);
            selem.PlaceOn(0, chapCount * 4 + 2);

            int whole = 0;
            foreach (var chapterLayoutRow in layout.Rows)
            {
                int c = whole;
                foreach (var elem in chapterLayoutRow.FirstColumn)
                {
                    c -= elem.RowSpan;
                    PlaceElemInGrid(elem, grid, c, chapCount*4);
                }

                c = whole;
                foreach (var elem in chapterLayoutRow.SecondColumn)
                {
                    c -= elem.RowSpan;
                    PlaceElemInGrid(elem, grid, c, chapCount*4 + 1);
                }

                c = whole;
                foreach (var elem in chapterLayoutRow.ThirdColumn)
                {
                    c -= elem.RowSpan;
                    PlaceElemInGrid(elem, grid, c, chapCount*4 + 2);
                }

                c = whole;
                foreach (var elem in chapterLayoutRow.FourthColumn)
                {
                    c -= elem.RowSpan;
                    PlaceElemInGrid(elem, grid, c, chapCount*4 + 3);
                }

                whole -= chapterLayoutRow.Height;
            }
        }

        private void PlaceElemInGrid(ChapterLayoutElem page, Grid grid, int row, int col)
        {
            GridElemBlock elem;
            if (page.Page.IsBlockTag)
                elem = new GridElemBlockTag(page.Page.Block, page.Page.Tag, grid);
            else if (page.Page.IsBlockRel)
                elem = new GridElemBlockRel(page.Page.Block, grid);
            else
                elem = new GridElemBlockOthers(page.Page.Block, grid);
            elem.PlaceOn(row, col);
        }
    }


    public class ChapterLayout
    {
        public Block ChapterBlock;
        public List<ChapterLayoutRow> Rows;
    }

    public class ChapterLayoutElem
    {
        public IPage Page;
        public int RowSpan;
    }

    public class ChapterLayoutRow
    {
        public ChapterLayoutRow()
        {
            Inner = false;
            FirstColumn = new List<ChapterLayoutElem>();
            SecondColumn = new List<ChapterLayoutElem>();
            ThirdColumn = new List<ChapterLayoutElem>();
            FourthColumn = new List<ChapterLayoutElem>();
        }

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
