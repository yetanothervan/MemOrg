namespace MemOrg.Interfaces
{
    public class GridElem : IGridElem
    {
        private int _rowIndex = -1;
        private int _colIndex = -1;

        private readonly IGrid _myGrid;

        public GridElem(IGrid myGrid)
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

        public object Content { get; set; }
    }
}