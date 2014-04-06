using System.Collections.Generic;
using System.Linq;
using GraphOrganizeService.Chapter;
using GraphOrganizeService.OrgUnits;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService
{
    public class LayoutChaptersTree : ITreeLayout
    {
        private readonly IGraph _graph;

        public LayoutChaptersTree(IGraph graph)
        {
            _graph = graph;
        }

        private Tree CreateTree(ChapterLayoutBundle root)
        {
            string pre;
            switch (root.Direction)
            {
                    case BundleDirection.Lower:
                    pre = "low";
                    break;
                    case BundleDirection.Middle:
                    pre = "mid";
                    break;
                    case BundleDirection.OuterRoot:
                    pre = "out";
                    break;
                    case BundleDirection.Root:
                    pre = "root";
                    break;
                   case BundleDirection.Upper:
                    pre = "up";
                    break;
                default:
                    pre = "o_0";
                    break;
            }

            var tree = new Tree
            {
                Subtrees = new List<ITree>(),
                MyElem = GetElem(root.MyElem, pre)
            };

            foreach (var child in root.Ones)
            {
                var other = child.First == root.MyElem ? child.Second : child.First;
                tree.Subtrees.Add(CreateTree(other));
            }

            foreach (var child in root.Bundles)
                tree.Subtrees.Add(CreateTree(child));

            return tree;
        }

        private Tree CreateTree(IPage one)
        {
            var tree = new Tree()
            {
                Subtrees = new List<ITree>(),
                MyElem = GetElem(one, "one")
            };
            return tree;
        }

        private static IOrg GetElem(IPage page, string dir)
        {
            page.Block.Caption = dir + " " + page.Block.Caption;

            if (page.IsBlockTag)
                return new OrgBlockTag(page.Block, page.Tag, null);
            
            if (page.IsBlockRel)
                return new OrgBlockRel(page.Block, null);

            if (page.IsBlockUserText)
                return new OrgBlockUserText(page.Block, null);
            
            return new OrgBlockOthers(page.Block, null);
        }

        public IOrgGrid CreateTreesGrid()
        {
            IOrgGrid grid = new OrgGrid();
            
            var bundles = new List<ChapterLayoutBundle>();
            foreach (var graphs in _graph.Books.Select(ChapterLayoutGraph.GetGraphsFromBook))
                bundles.AddRange(graphs.Select(ChapterLayoutBundle.ExtractBundlesFromGraph));

            var allocator = new RawSquareGridElemAllocator(bundles.Count);

            foreach (var bundle in bundles)
            {
                var ge = new OrgGridElem(grid);
                var tree = CreateTree(bundle);
                ge.Content = tree;
                allocator.PlaceNextGridElem(ge);
            }
            return grid;
        }
    }
}