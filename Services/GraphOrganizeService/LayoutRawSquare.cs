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
                var page = graphService.CreateStumpPage(blockTag, tag);
                var ge = new OrgGridElem(grid) { Content = new OrgBlockTag(page, null) };
                _allocator.PlaceNextGridElem(ge);
            }
            foreach (var blockRel in graphService.BlockRels)
            {
                var page = graphService.CreateStumpPage(blockRel, null);
                var ge = new OrgGridElem(grid) { Content = new OrgBlockRel(page, null) };
                _allocator.PlaceNextGridElem(ge);
            }
            foreach (var block in graphService.BlockOthers)
            {
                var page = graphService.CreateStumpPage(block, null);
                var ge = new OrgGridElem(grid);
                if (block.Particles.Count == block.Particles.OfType<UserTextParticle>().Count()
                    && block.Particles.Count != 0)
                    ge.Content = new OrgBlockUserText(page, null);
                else
                    ge.Content = new OrgBlockOthers(page, null);
                _allocator.PlaceNextGridElem(ge);
            }

            return grid;
        }
    }
}

