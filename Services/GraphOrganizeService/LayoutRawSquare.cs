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
    public class LayoutRawSquare : IGridLayout
    {
        private readonly IGraph _graph;
        private RawSquareGridElemAllocator _allocator;

        public LayoutRawSquare(IGraph graph)
        {
            _graph = graph;
        }

        public IOrgGrid CreateGrid()
        {
            var grid = new OrgGrid();
            var graphService = _graph.GraphService;

            var elemsCount = graphService.BlockOthers.Count()
                             + graphService.BlockRels.Count()
                             + graphService.BlockTags.Count();
                

            _allocator = new RawSquareGridElemAllocator(elemsCount);
          
            foreach (var blockTag in graphService.BlockTags)
            {
// ReSharper disable once AccessToForEachVariableInClosure
                var tag = graphService.TagsBlock.First(o => o.TagBlock.BlockId == blockTag.BlockId);
                var ge = new GridElem(grid) {Content = new OrgBlockTag(blockTag, tag)};
                _allocator.PlaceNextGridElem(ge);
            }
            foreach (var blockRel in graphService.BlockRels)
            {
                var ge = new GridElem(grid) {Content = new OrgBlockRel(blockRel)};
                _allocator.PlaceNextGridElem(ge);
            }
            foreach (var block in graphService.BlockOthers)
            {
                var ge = new GridElem(grid);
                if (block.Particles.Count == block.Particles.OfType<UserTextParticle>().Count()
                    && block.Particles.Count != 0)
                    ge.Content = new OrgBlockUserText(block);
                else
                    ge.Content = new OrgBlockOthers(block);
                _allocator.PlaceNextGridElem(ge);
            }

            return grid;
        }
    }
}

