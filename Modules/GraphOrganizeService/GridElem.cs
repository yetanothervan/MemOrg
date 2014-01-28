using System.Collections.Generic;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public abstract class GridElem : IGridElem
    {
        private int _rowIndex = -1;
        private int _colIndex = -1;

        public void PlaceOn(int row, int col, List<List<IGridElem>> elems)
        {
            _rowIndex = row;
            _colIndex = col;
            elems[row][col] = this;
        }

        public int RowIndex
        {
            get { return _rowIndex; }
        }

        public int ColIndex
        {
            get { return _colIndex; }
        }
    }
}