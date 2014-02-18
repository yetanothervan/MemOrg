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
                    
                    rows.ForEach(ApplyLayout);
                    
                    var layout = new ChapterLayout {Rows = rows, ChapterBlock = chapter.ChapterBlock};

                    DoChapterLayout(layout, grid, chapCount++);
                }
            }
            return grid;
        }

        private void ApplyLayout(ChapterLayoutRow chapterLayoutRow)
        {
            if (chapterLayoutRow.Pages == null || chapterLayoutRow.Pages.Count == 0) return;

            if (chapterLayoutRow.Pages.Count == 1)
            {
                chapterLayoutRow.ThirdColumn.Add(
                    new ChapterLayoutElem {Page = chapterLayoutRow.Pages[0], RowSpan = 1});
                return;
            }

            if (chapterLayoutRow.Pages.Count == 3)
            {
                var rel = chapterLayoutRow.Pages.FirstOrDefault(r => r.IsBlockRel);
                if (rel != null)
                {
                    chapterLayoutRow.ThirdColumn.Add(
                        new ChapterLayoutElem {Page = rel, RowSpan = 1});
                    chapterLayoutRow.SecondColumn.Add(
                        new ChapterLayoutElem {Page = rel.RelationFirst, RowSpan = 1});
                    chapterLayoutRow.FourthColumn.Add(
                        new ChapterLayoutElem { Page = rel.RelationSecond, RowSpan = 1 });
                }
                return;
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
                int c = 0;
                foreach (var elem in chapterLayoutRow.FirstColumn)
                    PlaceElemInGrid(elem, grid, c -= elem.RowSpan - whole, chapCount*4);

                c = 0;
                foreach (var elem in chapterLayoutRow.SecondColumn)
                    PlaceElemInGrid(elem, grid, c -= elem.RowSpan - whole, chapCount*4 + 1);

                c = 0;
                foreach (var elem in chapterLayoutRow.ThirdColumn)
                    PlaceElemInGrid(elem, grid, c -= elem.RowSpan - whole, chapCount*4 + 2);

                c = 0;
                foreach (var elem in chapterLayoutRow.FourthColumn)
                    PlaceElemInGrid(elem, grid, c -= elem.RowSpan - whole, chapCount*4 + 3);

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
    }
}
