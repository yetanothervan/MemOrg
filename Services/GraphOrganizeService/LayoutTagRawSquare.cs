using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using GraphOrganizeService.OrgUnits;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class LayoutTagRawSquare : ITreeLayout
    {
        private readonly IGraph _graph;

        public LayoutTagRawSquare(IGraph graph)
        {
            _graph = graph;
        }

        private Tree CreateTree(Tag root)
        {
            var tree = new Tree()
            {
                Subtrees = new List<ITree>(), 
                MyElem = new OrgTag(root) 
            };

            foreach (var child in root.Childs)
                tree.Subtrees.Add(CreateTree(child));

            return tree;
        }

        public IGrid CreateTreesGrid()
        {
            IGrid grid = new Grid();
            var allocator = new RawSquareGridElemAllocator(_graph.GraphService.TagRoots.Count());
            foreach (var tagRoot in _graph.GraphService.TagRoots)
            {
                var ge = new GridElem(grid);
                var tree = CreateTree(tagRoot);
                ge.Content = tree;
                allocator.PlaceNextGridElem(ge);
            }
            return grid;
        }
    }
}

