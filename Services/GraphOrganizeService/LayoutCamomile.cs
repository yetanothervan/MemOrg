using System;
using System.Collections.Generic;
using System.Linq;
using GraphOrganizeService.Chapter;
using GraphOrganizeService.OrgUnits;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService
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

            var bundles = new List<ChapterLayoutBundle>();
            foreach (var graphs in _graph.Books.Select(ChapterLayoutGraph.GetGraphsFromBook))
                bundles.AddRange(graphs.Select(ChapterLayoutBundle.ExtractBundlesFromGraph));

            int left = 0;
            foreach (var book in _graph.Books)
                foreach (var chapter in book.Chapters)
                {
                    var chl = chapter;
                    var chps = bundles.Where(b => b.MyChapter == chl);

                    var result = new List<ChapterLayoutElem>();
                    
                    int resHeight = 0;
                    foreach (var bundle in chps)
                    {
                        var ta = bundle.Render(0, left, true);
                        int min = ta.Min(b => b.Row);
                        int max = ta.Max(b => b.Row);

                        foreach (var el in ta)
                        {
                            el.Row -= (resHeight + max + 1);
                            if (el.Page != null && el.Page.Block != null)
                                el.Page.Block.Caption += " " + el.Row + ", " + el.Col;
                        }
                        result.AddRange(ta);

                        resHeight = -result.Min(e => e.Row);
                    }
                    
                    result.Add(new ChapterLayoutElem
                    {
                        Col = left + 2,
                        Row = 0,
                        HorizontalAligment = HorizontalAligment.Center,
                        Page = chapter.ChapterPage
                    });

                    foreach (var elem in result)
                        PlaceElemInGrid(elem, grid);

                    left += 8;
                }

            return grid;
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
        
        private static ChapterLayoutElem NewGridElem(IPage content, HorizontalAligment horAligment, params NESW[] conPoints)
        {
            return new ChapterLayoutElem
            {
                Page = content,
                ConnectionPoints = new List<NESW>(conPoints),
                HorizontalAligment = horAligment
            };
        }

        private void PlaceElemInGrid(ChapterLayoutElem page, OrgGrid orgGrid)
        {
            var ge = new OrgGridElem(orgGrid)
            {
                VerticalContentAligment = VerticalAligment.Center,
                HorizontalContentAligment = page.HorizontalAligment
            };

            if (page.IsGridLinkPart)
            {
                var cell = orgGrid.GetElem(page.Row, page.Col);
                if (cell != null && cell.Content != null)
                {
                    var list = cell.Content as IReadOnlyList<GridLinkPart>;
                    if (list == null) throw new ArgumentException();
                    var newList = new List<GridLinkPart>();
                    newList.AddRange(list);
                    foreach (var part in page.GridLinkParts.Where(part => !newList.Contains(part)))
                        newList.Add(part);
                    cell.Content = newList;
                    page.Placed = true;
                    return;
                }

                ge.Content = page.GridLinkParts;
                ge.VerticalContentAligment = VerticalAligment.Top;
            }
            else if (page.Page.IsBlockTag)
                ge.Content = new OrgBlockTag(page.Page, page.ConnectionPoints);
            else if (page.Page.IsBlockSource)
                ge.Content = new OrgBlockSource(page.Page, page.ConnectionPoints);
            else if (page.Page.IsBlockUserText)
                ge.Content = new OrgBlockUserText(page.Page, page.ConnectionPoints);
            else if (page.Page.IsBlockRel)
                ge.Content = new OrgBlockRel(page.Page, page.ConnectionPoints);
            else
                ge.Content = new OrgBlockOthers(page.Page, page.ConnectionPoints);

            ge.PlaceOn(page.Row, page.Col);
            page.Placed = true;
        }
    }
}
