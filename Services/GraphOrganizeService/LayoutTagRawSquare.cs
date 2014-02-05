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
    public class LayoutTagRawSquare : IGridLayout
    {
        private int _sideSize;

        private readonly IGraph _graph;

        public LayoutTagRawSquare(IGraph graph)
        {
            _graph = graph;
        }

        public void DoLayout(IGrid grid)
        {
            var graphService = _graph.GraphService;

            var elemsCount = graphService.TagsBlock.Count 
                             + graphService.TagsNoBlock.Count;

            _sideSize = CalculateSquareSideLength(elemsCount);
            
            int curElem = 0;
            foreach (var tagNoBlock in graphService.TagsNoBlock)
            {
                var gridElem = new GridElemTag(tagNoBlock, grid);
                PlaceNextGridElem(gridElem, curElem++);
            }
            foreach (var tagBlock in graphService.TagsBlock)
            {
                var gridElem = new GridElemTag(tagBlock, grid);
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

