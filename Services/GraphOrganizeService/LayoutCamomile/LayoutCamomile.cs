using System;
using System.Collections.Generic;
using System.Linq;
using GraphOrganizeService.OrgUnits;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.LayoutCamomile
{
    public class LayoutCamomile : IGridLayout
    {
        private readonly IGraph _graph;

        public LayoutCamomile(IGraph graph)
        {
            _graph = graph;
        }
        
        public IOrgGrid CreateGrid()
        {
            var grid = new OrgGrid();
            //page organize
            foreach (var book in _graph.Books)
            {
                int left = 0;
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
                    //rows.ForEach(ProvideReferences);
                    
                    var layout = new ChapterLayout {Rows = rows, ChapterBlock = chapter.ChapterBlock};

                    int chapterColumnLeft = left + (rows.Max(r => r.GridInfo.MaxCol) - rows.Min(r => r.GridInfo.MinCol) + 1);
                    
                    DoChapterLayout(layout, grid, chapterColumnLeft);

                    int chapterLayoutWidth =
                        rows.Max(r => r.GridInfo.MaxCol) -
                        rows.Min(r => r.GridInfo.MinCol);

                    left += chapterLayoutWidth;
                }
            }
            
            return grid;
        }
        
        /*private void CheckColumn(List<ChapterLayoutElem> column, ChapterLayoutRow row)
        {
            var toAdd = new List<ChapterLayoutElem>();
            foreach (var elem in column.Where(c => c.Page.ReferencedBy.Any()))
                foreach (var page in elem.Page.ReferencedBy)
                    if (!row.MyChapter.PagesBlocks.Contains(page))
                        toAdd.Add(new ChapterLayoutElem { Page = page, RowSpan = 1 });
            column.AddRange(toAdd);
        }*/

        private void ApplyLayout(ChapterLayoutRow row)
        {
            if (row.Pages == null || row.Pages.Count == 0) return;

            if (row.Pages.Count == 1)
            {
                PlaceLayoutElem(row,
                    new ChapterLayoutElem
                    {
                        Page = row.Pages[0],
                        HorizontalAligment = HorizontalAligment.Center
                    }, 0, 0);
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
            var cr = new ComplicatedLayoutRow(row);
            if (cr.nestRows.Count(nr => nr.nestRows.Count > 0) > 1)
                throw new Exception("TODO");
            
            LayoutMain(cr, row);
        }

        private void LayoutMain(ComplicatedLayoutRow cr, ChapterLayoutRow row)
        {
            var upper = cr.nestRows.FirstOrDefault(nr => nr.nestRows.Count > 0);
            if (upper != null)
                LayoutUpper(upper, row);

            var lower = cr.nestRows.Where(nr => nr.nestRows.Count == 0).ToList();

            //main
            PlaceLayoutElem(row, NewGridElem(cr.MySelf, HorizontalAligment.Right, NESW.East), 0, -2);
            var mainlinks = new List<GridLinkPartDirection>{GridLinkPartDirection.WestEast};
            if (lower.Count > 1) mainlinks.Add(GridLinkPartDirection.WestSouth);
            PlaceLayoutElem(row, NewGridLink(mainlinks), 0, -1);
            
            var mainSecondLinks = new List<GridLinkPartDirection> { GridLinkPartDirection.WestEast };
            if (upper != null) mainSecondLinks.Add(GridLinkPartDirection.NorthEast);
            
            if (upper == null)
            {
                upper = lower.First();
                lower = lower.Skip(1).ToList();
            }

            PlaceLayoutElem(row, NewGridElem(upper.ParentRel, HorizontalAligment.Center, NESW.East, NESW.West), 0, 0);
            PlaceLayoutElem(row, NewGridLink(mainSecondLinks), 0, 1);
            PlaceLayoutElem(row, NewGridElem(upper.MySelf, HorizontalAligment.Left, NESW.West), 0, 2);

            LayoutLower(lower, row);
        }

        private void LayoutLower(List<ComplicatedLayoutRow> lower, ChapterLayoutRow row)
        {
            var height = 1;
            foreach (var low in lower)
            {
                var links = new List<GridLinkPartDirection> {GridLinkPartDirection.NorthEast};
                if (low != lower.Last()) links.Add(GridLinkPartDirection.NorthSouth);

                PlaceLayoutElem(row, NewGridLink(links), height, -1);
                PlaceLayoutElem(row, NewGridElem(low.ParentRel, HorizontalAligment.Center, NESW.East, NESW.West), height, 0);
                PlaceLayoutElem(row, NewGridLink(new List<GridLinkPartDirection> { GridLinkPartDirection.WestEast }), height, 1);
                PlaceLayoutElem(row, NewGridElem(low.MySelf, HorizontalAligment.Left, NESW.West), height, 2);
                ++height;
            }
        }

        private void LayoutUpper(ComplicatedLayoutRow upper, ChapterLayoutRow row)
        {
            PlaceLayoutElem(row, NewGridLink(new List<GridLinkPartDirection> {GridLinkPartDirection.WestSouth}), -1, 1);
            PlaceLayoutElem(row, NewGridElem(upper.nestRows.First().ParentRel, HorizontalAligment.Center, NESW.East, NESW.West), -1, 0);
            PlaceLayoutElem(row, NewGridLink(new List<GridLinkPartDirection> {GridLinkPartDirection.WestEast}), -1, -1);
            PlaceLayoutElem(row, NewGridElem(upper.nestRows.First().MySelf, HorizontalAligment.Right, NESW.East), -1, -2);
        }

        class ComplicatedLayoutRow
        {
            public readonly IPage MySelf;
            public IPage ParentRel;
            private int Level;
            public List<ComplicatedLayoutRow> nestRows;

            private ComplicatedLayoutRow(IPage myRel, IPage me, int level, ISet<IPage> exceptPages)
            {
                Level = level;
                ParentRel = myRel;
                MySelf = me;
                nestRows = new List<ComplicatedLayoutRow>();

                var relsInMySelf = MySelf.RelatedBy.Where(r => r.MyChapter == myRel.MyChapter).Except(exceptPages).ToList();
                if (!relsInMySelf.Any()) return;

                foreach (var rel in relsInMySelf)
                {
                    exceptPages.Add(rel);
                    var oppose = rel.RelationFirst.Block.BlockId != MySelf.Block.BlockId
                        ? rel.RelationFirst
                        : rel.RelationSecond;
                    var nestRow = new ComplicatedLayoutRow(rel, oppose, Level + 1, exceptPages);
                    nestRows.Add(nestRow);
                }
            }

            public ComplicatedLayoutRow(ChapterLayoutRow row)
            {
                Level = 0;
                ParentRel = null;
                nestRows = new List<ComplicatedLayoutRow>();
                
                //с самым большим кол-вом связей - первый
                MySelf = row.Pages.Where(p => !p.IsBlockRel).OrderByDescending(r => r.RelatedBy.Count).FirstOrDefault();
                if (MySelf == null) return;

                var rels = new HashSet<IPage>();
                var relsInMySelf = MySelf.RelatedBy.Where(r => r.MyChapter == row.MyChapter).Except(rels).ToList();
                if (!relsInMySelf.Any()) return;

                foreach (var rel in relsInMySelf)
                {
                    rels.Add(rel);
                    var oppose = rel.RelationFirst.Block.BlockId != MySelf.Block.BlockId
                        ? rel.RelationFirst
                        : rel.RelationSecond;
                    var nestRow = new ComplicatedLayoutRow(rel, oppose, Level + 1, rels);
                    nestRows.Add(nestRow);
                }
            }
        }

        private void PlaceLayoutElem(ChapterLayoutRow elemsRow, ChapterLayoutElem content, int row, int col)
        {
            var gridElem = new GridElem(elemsRow) { Content = content };
            gridElem.PlaceOn(row, col);
        }

        private static ChapterLayoutElem NewGridLink(IEnumerable<GridLinkPartDirection> directions)
        {
            var res = new ChapterLayoutElem();
            foreach (var gridLinkPartDirection in directions)
            {
                res.AddGridLink(new GridLinkPart
                {
                    Direction = gridLinkPartDirection,
                    Type = GridLinkPartType.Relation
                });
            }
            return res;
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

        private void DoChapterLayout(ChapterLayout layout, OrgGrid orgGrid, int chapterColumnLeft)
        {
            //source elem
            var ge = new OrgGridElem(orgGrid)
            {
                HorizontalContentAligment = HorizontalAligment.Center,
                VerticalContentAligment = VerticalAligment.Center,
                Content = new OrgBlockSource(layout.ChapterBlock, null)
            };
            ge.PlaceOn(0, chapterColumnLeft);

            int whole = 0;
            foreach (var chapterLayoutRow in layout.Rows)
            {
                whole -= chapterLayoutRow.RowCount;
                foreach (var elem in chapterLayoutRow)
                {
                    var cle = elem.Content as ChapterLayoutElem;
                    if (cle != null)
                        PlaceElemInGrid(cle, orgGrid, elem.RowIndex + whole - chapterLayoutRow.GridInfo.MinRow,
                            chapterColumnLeft + elem.ColIndex);
                }
            }
        }

        private static ChapterLayoutElem NewGridElem(IPage content, HorizontalAligment horAligment, params NESW[] conPoints)
        {
            return new ChapterLayoutElem
            {
                Page = content,
                ConnectionPoints = new List<NESW>(conPoints),
                HorizontalAligment = horAligment
            };
        }

        private void PlaceElemInGrid(ChapterLayoutElem page, OrgGrid orgGrid,
            int row, int col)
        {
            var ge = new OrgGridElem(orgGrid)
            {
                VerticalContentAligment = VerticalAligment.Center,
                HorizontalContentAligment = page.HorizontalAligment
            };

            if (page.IsGridLinkPart)
            {
                ge.Content = page.GridLinkParts;
                ge.VerticalContentAligment = VerticalAligment.Top;
            }
            else if (page.Page.IsBlockTag)
                ge.Content = new OrgBlockTag(page.Page.Block, page.Page.Tag, page.ConnectionPoints);
            else if (page.Page.IsBlockRel)
                ge.Content = new OrgBlockRel(page.Page.Block, page.ConnectionPoints);
            else
                ge.Content = new OrgBlockOthers(page.Page.Block, page.ConnectionPoints);

            ge.PlaceOn(row, col);
            page.Placed = true;
            page.Row = row;
            page.Col = col;
        }
    }
}
