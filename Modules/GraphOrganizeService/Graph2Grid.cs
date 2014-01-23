using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class Graph2Grid
    {
        public static Grid ProcessGraph(IGraphService graphService)
        {
            var elemsCount = graphService.BlockOthers.Count
                            + graphService.BlockRels.Count
                            + graphService.BlockSources.Count
                            + graphService.BlockTags.Count
                            + graphService.TagsNoBlock.Count;
            var squareSide = CalculateSquareSideLength(elemsCount);

            var grid = new Grid(squareSide, squareSide);

            int curElem = 0;
            foreach (var blockTag in graphService.BlockTags)
            {
                var gridElem = new GridElemBasedOnBlock(blockTag, GridElemBasedOnBlockType.BlockTag);
                PlaceNextGridElem(grid, gridElem, curElem++);
            }
            foreach (var blockSource in graphService.BlockSources)
            {
                var gridElem = new GridElemBasedOnBlock(blockSource, GridElemBasedOnBlockType.BlockSource);
                PlaceNextGridElem(grid, gridElem, curElem++);
            }
            foreach (var blockRel in graphService.BlockRels)
            {
                var gridElem = new GridElemBasedOnBlock(blockRel, GridElemBasedOnBlockType.BlockRel);
                PlaceNextGridElem(grid, gridElem, curElem++);
            }
            foreach (var block in graphService.BlockOthers)
            {
                var gridElem = new GridElemBasedOnBlock(block, GridElemBasedOnBlockType.BlockOther);
                PlaceNextGridElem(grid, gridElem, curElem++);
            }
            foreach (var tagNoBlock in graphService.TagsNoBlock)
            {
                var gridElem = new GridElemBasedOnTag(tagNoBlock);
                PlaceNextGridElem(grid, gridElem, curElem++);
            }
            return grid;
        }

        private static int PlaceNextGridElem(Grid grid, GridElem gridElem, int gridElemIndex)
        {
            int rowIndex = gridElemIndex / grid.GetRowLength;
            int colIndex = gridElemIndex % grid.GetRowLength;
            grid.SetElemOn(rowIndex, colIndex, gridElem);
        }

        private static int CalculateSquareSideLength(int elemsCount)
        {
            var res = 0;
// ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (elemsCount >= res * res)
                ++res;
            return res;
        }
    }
}
