using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces.OrgUnits;

namespace MemOrg.Interfaces
{
    public class Grid : IGrid
    {
        private readonly SortedDictionary<IntIntPair, IGridElem> _elems;

        protected Grid()
        {
            _elems = new SortedDictionary<IntIntPair, IGridElem>();
        }
        
        public IEnumerator<IGridElem> GetEnumerator()
        {
            return new GridEnumerator<IGridElem>(_elems);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void PlaceElem(int row, int col, IGridElem elem)
        {
            var p = new IntIntPair(row, col);
            /*if (_elems.ContainsKey(p))
                throw new ArgumentOutOfRangeException();*/
            _elems[p] = elem;
            _gridInfo = null;
        }

        public IGridElem GetElem(int row, int col)
        {
            var p = new IntIntPair(row, col);
            IGridElem res;
            return _elems.TryGetValue(p, out res) ? res : null;
        }


        private GridInfo _gridInfo;

        public GridInfo GridInfo
        {
            get { return _gridInfo ?? (_gridInfo = CalculateSize()); }
        }

        private GridInfo CalculateSize()
        {
            var gridInfo = new GridInfo
            {
                MaxRow = _elems.Keys.Max(o => o.Row),
                MinRow = _elems.Keys.Min(o => o.Row),
                MaxCol = _elems.Keys.Max(o => o.Col),
                MinCol = _elems.Keys.Min(o => o.Col)
            };
            return gridInfo;
        }
        
        public int RowCount {
            get
            {
                if (_gridInfo == null)
                    _gridInfo = CalculateSize();
                return _gridInfo.MaxRow - _gridInfo.MinRow + 1;
            }
        }

        public int RowLength
        {
            get
            {
                if (_gridInfo == null)
                    _gridInfo = CalculateSize();
                return _gridInfo.MaxCol - _gridInfo.MinCol + 1;
            }
        }
    }

    public class GridInfo
    {
        public int MaxCol { get; internal set; }
        public int MinCol { get; internal set; }
        public int MaxRow { get; internal set; }
        public int MinRow { get; internal set; }
    }
}
