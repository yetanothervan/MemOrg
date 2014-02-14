using System;
using System.Collections.Generic;
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
                    var layout = new ChapterLayout {ChapterBlock = chapter.ChapterBlock};

                    var pagesRelsSet = new HashSet<IPage>(chapter.PagesBlocks.Where(p => p.IsBlockRel));
                    layout.FirstColumn.AddRange(pagesRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 1}));

                    var pagesNoRelsSet = new HashSet<IPage>(chapter.PagesBlocks
                        .Where(p => !p.IsBlockRel && pagesRelsSet
                            .All(p2 => p2.Relation.FirstBlock.BlockId != p.Block.BlockId
                                       && p2.Relation.SecondBlock.BlockId != p.Block.BlockId)));
                    layout.SecondColumn.AddRange(
                        pagesNoRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 1}));

                    var pagesOneRelsSet = new HashSet<IPage>(chapter.PagesBlocks.Where(p =>
                        pagesRelsSet
                            .Count(p2 => p2.Relation.FirstBlock.BlockId == p.Block.BlockId
                                         || p2.Relation.SecondBlock.BlockId == p.Block.BlockId) == 1));
                    layout.ThirdColumn.AddRange(
                        pagesOneRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 1}));

                    var pagesMultiRelsSet = new HashSet<IPage>(chapter.PagesBlocks.Where(p =>
                        !(pagesRelsSet.Contains(p) || pagesNoRelsSet.Contains(p) || pagesOneRelsSet.Contains(p))));
                    layout.FourthColumn.AddRange(
                        pagesMultiRelsSet.Select(p => new ChapterLayoutElem {Page = p, RowSpan = 2}));
                    
                    DoChapterLayout(layout, grid, chapCount++);
                }

            }

            return grid;
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
        public readonly List<ChapterLayoutElem> FirstColumn;
        public readonly List<ChapterLayoutElem> SecondColumn;
        public readonly List<ChapterLayoutElem> ThirdColumn;
        public readonly List<ChapterLayoutElem> FourthColumn;
        public Block ChapterBlock;

        public ChapterLayout()
        {
            FirstColumn = new List<ChapterLayoutElem>();
            SecondColumn = new List<ChapterLayoutElem>();
            ThirdColumn = new List<ChapterLayoutElem>();
            FourthColumn = new List<ChapterLayoutElem>();
        }
    }

    public class ChapterLayoutElem
    {
        public IPage Page;
        public int RowSpan;
    }
}
