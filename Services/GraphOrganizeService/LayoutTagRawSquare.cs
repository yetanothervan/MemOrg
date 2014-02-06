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
    public class LayoutTagRawSquare : ITreeLayout
    {
        private readonly IGraph _graph;

        public LayoutTagRawSquare(IGraph graph)
        {
            _graph = graph;
        }

        private Tree CreateTree(Tag root, IGrid grid)
        {
            var tree = new Tree(grid)
            {
                Subtrees = new List<ITree>(), 
                MyElem = new GridElemTag(root, null)
            };

            foreach (var child in root.Childs)
                tree.Subtrees.Add(CreateTree(child, null));

            return tree;
        }

        public IGrid CreateTreesGrid()
        {
            IGrid grid = new Grid();
            var allocator = new RawSquareGridElemAllocator(_graph.GraphService.TagRoots.Count);
            foreach (var tagRoot in _graph.GraphService.TagRoots)
            {
                var tree = CreateTree(tagRoot, grid);
                allocator.PlaceNextGridElem(tree);
            }
            return grid;
        }
    }
}

