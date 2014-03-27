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
                    
                    int chapterColumnLeft = left - rows.Min(r => r.GridInfo.MinCol);
                    
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
            var rels = new HashSet<IPage>();

            var pages = row.Pages.Where(p => !p.IsBlockRel).OrderByDescending(r => r.RelatedBy.Count).ToList();
            int height = 0;
            foreach (var page in pages)
            {
                var relsInPage = page.RelatedBy.Where(r => r.MyChapter == row.MyChapter).Except(rels).ToList();
                
                if (relsInPage.Any())
                {
                    PlaceLayoutElem(row, NewGridElem(page, HorizontalAligment.Right, NESW.East), height, -2);
                    foreach (var p in relsInPage)
                    {
                        PlaceLayoutElem(row,
                            height == 0
                                ? NewGridLink()
                                : NewGridLink(GridLinkPartDirection.NorthEast),
                            height, -1);
                        PlaceLayoutElem(row, NewGridElem(p, HorizontalAligment.Center, NESW.West, NESW.East), height, 0);
                        var oppose = p.RelationFirst.Block.BlockId != page.Block.BlockId 
                            ? p.RelationFirst : p.RelationSecond;
                        PlaceLayoutElem(row, NewGridLink(), height, 1);
                        PlaceLayoutElem(row, NewGridElem(oppose, HorizontalAligment.Left, NESW.West), height, 2);
                        rels.Add(p);
                        height++;
                    }
                }
            }
        }

        private void PlaceLayoutElem(ChapterLayoutRow elemsRow, ChapterLayoutElem content, int row, int col)
        {
            var gridElem = new GridElem(elemsRow) { Content = content };
            gridElem.PlaceOn(row, col);
        }

        private static ChapterLayoutElem NewGridLink(GridLinkPartDirection direction = GridLinkPartDirection.WestEast)
        {
            return new ChapterLayoutElem
            {
                GridLinkPart =
                    new GridLinkPart {Direction = direction, Type = GridLinkPartType.Relation}
            };
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
                whole -= (chapterLayoutRow.RowCount + 1);
                foreach (var elem in chapterLayoutRow)
                {
                    var cle = elem.Content as ChapterLayoutElem;
                    if (cle != null)
                        PlaceElemInGrid(cle, orgGrid, elem.RowIndex + whole, 
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
                ge.Content = page.GridLinkPart;
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
