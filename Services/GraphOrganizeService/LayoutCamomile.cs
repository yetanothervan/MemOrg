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

                    var pagesRelsSet = chapter.PagesBlocks.Where(p => p.IsBlockRel).ToList();

                    var pagesNoRelsSet = chapter.PagesBlocks
                        .Where(p => !p.IsBlockRel && pagesRelsSet
                            .All(p2 => p2.Relation.FirstBlock.BlockId != p.Block.BlockId
                                       && p2.Relation.SecondBlock.BlockId != p.Block.BlockId)).ToList();

                    var pagesOneRelsSet = chapter.PagesBlocks.Where(p =>
                      pagesRelsSet
                          .Count(p2 => p2.Relation.FirstBlock.BlockId == p.Block.BlockId
                              || p2.Relation.SecondBlock.BlockId == p.Block.BlockId) == 1).ToList();

                    var pagesMultiRelsSet = chapter.PagesBlocks.Where(p =>
                        !(pagesRelsSet.Contains(p) || pagesNoRelsSet.Contains(p) || pagesOneRelsSet.Contains(p))).ToList();

                    var layout = new ChapterLayout
                    {
                        ChapterBlock = chapter.ChapterBlock,
                        FirstColumn = new List<ChapterLayoutElem>(
                            pagesRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 1})),
                        SecondColumn = new List<ChapterLayoutElem>(
                            pagesNoRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 1})),
                        ThirdColumn = new List<ChapterLayoutElem>(
                            pagesOneRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 1})),
                        FourthColumn = new List<ChapterLayoutElem>(
                            pagesMultiRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 2}))
                    };

                    DoChapterLayout(layout, grid, chapCount++);
                }
            }
            return grid;
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

            int c = 0;
            foreach (var elem in layout.FirstColumn)
                PlaceElemInGrid(elem, grid, c -= elem.RowSpan, chapCount*4);

            c = 0;
            foreach (var elem in layout.SecondColumn)
                PlaceElemInGrid(elem, grid, c -= elem.RowSpan, chapCount*4 + 1);

            c = 0;
            foreach (var elem in layout.ThirdColumn)
                PlaceElemInGrid(elem, grid, c -= elem.RowSpan, chapCount*4 + 2);

            c = 0;
            foreach (var elem in layout.FourthColumn)
                PlaceElemInGrid(elem, grid, c -= elem.RowSpan, chapCount*4 + 3);
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
        public List<ChapterLayoutElem> FirstColumn;
        public List<ChapterLayoutElem> SecondColumn;
        public List<ChapterLayoutElem> ThirdColumn;
        public List<ChapterLayoutElem> FourthColumn;
        public Block ChapterBlock;
    }

    public class ChapterLayoutElem
    {
        public IPage Page;
        public int RowSpan;
    }

    public class ChapterLayoutRow
    {
        public List<IPage> Pages;
    }
}
