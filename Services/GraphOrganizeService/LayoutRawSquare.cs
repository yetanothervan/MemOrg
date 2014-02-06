using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using GraphOrganizeService.Elems;
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

        public IGrid CreateGrid()
        {
            var grid = new Grid();
            var graphService = _graph.GraphService;

            var elemsCount = graphService.BlockOthers.Count
                             + graphService.BlockRels.Count
                             + graphService.BlockSources.Count
                             + graphService.BlockTags.Count;

            _allocator = new RawSquareGridElemAllocator(elemsCount);
            
          
            foreach (var blockTag in graphService.BlockTags)
            {
// ReSharper disable once AccessToForEachVariableInClosure
                var tag = graphService.TagsBlock.First(o => o.TagBlock.BlockId == blockTag.BlockId);
                var gridElem = new GridElemBlockTag(blockTag, tag, grid);
                _allocator.PlaceNextGridElem(gridElem);
            }
            foreach (var blockSource in graphService.BlockSources)
            {
                var gridElem = new GridElemBlockSource(blockSource, grid);
                _allocator.PlaceNextGridElem(gridElem);
            }
            foreach (var blockRel in graphService.BlockRels)
            {
                var gridElem = new GridElemBlockRel(blockRel, grid);
                _allocator.PlaceNextGridElem(gridElem);
            }
            foreach (var block in graphService.BlockOthers)
            {
                if (block.Particles.Count == block.Particles.OfType<UserTextParticle>().Count()
                    && block.Particles.Count != 0)
                    _allocator.PlaceNextGridElem(new GridElemBlockUserText(block, grid));
                else
                    _allocator.PlaceNextGridElem(new GridElemBlockOthers(block, grid));
            }

            return grid;
        }
    }
}

