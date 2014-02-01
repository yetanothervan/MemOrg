using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService.Elems
{
    public abstract class GridElem : IGridElem
    {
        private int _rowIndex = -1;
        private int _colIndex = -1;

        private readonly IGrid _myGrid;

        protected GridElem(IGrid myGrid)
        {
            this._myGrid = myGrid;
        }

        public void PlaceOn(int row, int col)
        {
            _rowIndex = row;
            _colIndex = col;
            _myGrid.PlaceElem(row, col, this);
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