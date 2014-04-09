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

            var order = (from book in _graph.Books 
                         from chapter in book.Chapters 
                         from bundle in bundles.Where(b => b.MyChapter == chapter)
                         select bundle).ToList();

            var result = new List<ChapterLayoutElem>();
            int resHeight = 0;
            foreach (var bundle in order)
            {
                var ta = bundle.Render();
                int bundleIce = 0 - ta.Min(b => b.Row);

                foreach (var el in ta)
                {
                    el.Row += (resHeight + bundleIce);
                    if (el.Page != null && el.Page.Block != null)
                        el.Page.Block.Caption += " " + el.Row + ", " + el.Col;
                }
                result.AddRange(ta);
                resHeight = result.Max(e => e.Row) + 1;
            }

            foreach (var elem in result)
                PlaceElemInGrid(elem, grid);

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
                ge.Content = page.GridLinkParts;
                ge.VerticalContentAligment = VerticalAligment.Top;
            }
            else if (page.Page.IsBlockTag)
                ge.Content = new OrgBlockTag(page.Page.Block, page.Page.Tag, page.ConnectionPoints);
            else if (page.Page.IsBlockRel)
                ge.Content = new OrgBlockRel(page.Page.Block, page.ConnectionPoints);
            else
                ge.Content = new OrgBlockOthers(page.Page.Block, page.ConnectionPoints);

            ge.PlaceOn(page.Row, page.Col);
            page.Placed = true;
        }
    }
}
