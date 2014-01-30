using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class LayoutRawSquare : IGridLayout
    {
        private List<List<IGridElem>> _elems;
        private int _sideSize;

        public List<List<IGridElem>> DoLayout(IGraph graph)
        {
            var graphService = graph.GraphService;

            var elemsCount = graphService.BlockOthers.Count
                             + graphService.BlockRels.Count
                             + graphService.BlockSources.Count
                             + graphService.BlockTags.Count
                             + graphService.TagsNoBlock.Count;

            _sideSize = CalculateSquareSideLength(elemsCount);

            //create elems
            _elems = new List<List<IGridElem>>(_sideSize);
            for (int index = 0; index < _sideSize; ++index)
            {
                _elems.Add(new List<IGridElem>(_sideSize));
                for (int i = 0; i < _sideSize; i++)
                    _elems[index].Add(null);
            }

            int curElem = 0;
            foreach (var blockTag in graphService.BlockTags)
            {
                var gridElem = new GridElemBasedOnBlock(blockTag, GridElemBasedOnBlockType.BlockTag);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var blockSource in graphService.BlockSources)
            {
                var gridElem = new GridElemBasedOnBlock(blockSource, GridElemBasedOnBlockType.BlockSource);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var blockRel in graphService.BlockRels)
            {
                var gridElem = new GridElemBasedOnBlock(blockRel, GridElemBasedOnBlockType.BlockRel);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var block in graphService.BlockOthers)
            {
                var gridElem = new GridElemBasedOnBlock(block, GridElemBasedOnBlockType.BlockOther);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var tagNoBlock in graphService.TagsNoBlock)
            {
                var gridElem = new GridElemBasedOnTag(tagNoBlock);
                PlaceNextGridElem(gridElem, curElem++);
            }
            return _elems;
        }

        private void PlaceNextGridElem(GridElem gridElem, int gridElemIndex)
        {
            int rowIndex = gridElemIndex/_sideSize;
            int colIndex = gridElemIndex%_sideSize;
            gridElem.PlaceOn(rowIndex, colIndex, _elems);
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

