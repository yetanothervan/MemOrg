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
        private int _sideSize;

        private readonly IGraph _graph;

        public LayoutRawSquare(IGraph graph)
        {
            _graph = graph;
        }

        public void DoLayout(IGrid grid)
        {
            var graphService = _graph.GraphService;

            var elemsCount = graphService.BlockOthers.Count
                             + graphService.BlockRels.Count
                             + graphService.BlockSources.Count
                             + graphService.BlockTags.Count
                             + graphService.TagsNoBlock.Count;

            _sideSize = CalculateSquareSideLength(elemsCount);
            
            int curElem = 0;
            foreach (var blockTag in graphService.BlockTags)
            {
// ReSharper disable once AccessToForEachVariableInClosure
                var tag = graphService.TagsBlock.First(o => o.TagBlock.BlockId == blockTag.BlockId);
                var gridElem = new GridElemBlockTag(blockTag, tag, grid);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var blockSource in graphService.BlockSources)
            {
                var gridElem = new GridElemBlockSource(blockSource, grid);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var blockRel in graphService.BlockRels)
            {
                var gridElem = new GridElemBlockRel(blockRel, grid);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var block in graphService.BlockOthers)
            {
                if (block.Particles.Count == block.Particles.OfType<UserTextParticle>().Count()
                    && block.Particles.Count != 0)
                    PlaceNextGridElem(new GridElemBlockUserText(block, grid), curElem++);
                else
                    PlaceNextGridElem(new GridElemBlockOthers(block, grid), curElem++);
            }
            foreach (var tagNoBlock in graphService.TagsNoBlock)
            {
                var gridElem = new GridElemTag(tagNoBlock, grid);
                PlaceNextGridElem(gridElem, curElem++);
            }
        }

        private void PlaceNextGridElem(GridElem gridElem, int gridElemIndex)
        {
            int rowIndex = gridElemIndex/_sideSize;
            int colIndex = gridElemIndex%_sideSize;
            gridElem.PlaceOn(rowIndex, colIndex);
        }

        private static int CalculateSquareSideLength(int elemsCount)
        {
            var res = 0;
// ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (elemsCount >= res*res)
                ++res;
            return res;
        }
    }
}

