using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class RawSquareGridElemAllocator
    {
        private readonly int _elemsCount;
        private int _curIndex;
        private readonly int _sideSize;

        public RawSquareGridElemAllocator(int elemsCount)
        {
            if (elemsCount < 0)
                throw new ArgumentOutOfRangeException();
            _curIndex = 0;
            
            _elemsCount = elemsCount;
            _sideSize = CalculateSquareSideLength(_elemsCount);
        }

        public void PlaceNextGridElem(GridElem gridElem)
        {
            if (_curIndex == _elemsCount)
                throw new ArgumentOutOfRangeException();
            
            int rowIndex = _curIndex/_sideSize;
            int colIndex = _curIndex%_sideSize;
            gridElem.PlaceOn(rowIndex, colIndex);
            ++_curIndex;
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
