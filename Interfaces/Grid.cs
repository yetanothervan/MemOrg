﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces.OrgUnits;
using Microsoft.Practices.Unity.Utility;

namespace MemOrg.Interfaces
{
    public class Grid : IGrid
    {
        private readonly Dictionary<Pair<int, int>, IGridElem> _elems;
        
        public Grid()
        {
            _elems = new Dictionary<Pair<int, int>, IGridElem>();
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
            var p = new Pair<int, int>(row, col);
            if (_elems.ContainsKey(p))
                throw new ArgumentOutOfRangeException();
            _elems[p] = elem;
            _gridInfo = null;
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
                MaxRow = _elems.Keys.Max(o => o.First),
                MinRow = _elems.Keys.Min(o => o.First),
                MaxCol = _elems.Keys.Max(o => o.Second),
                MinCol = _elems.Keys.Min(o => o.Second)
            };
            return gridInfo;
        }
        
        public int RowCount {
            get
            {
                if (_gridInfo == null)
                    _gridInfo = CalculateSize();
                return _gridInfo.MaxRow - _gridInfo.MinRow;
            }
        }

        public int RowLength
        {
            get
            {
                if (_gridInfo == null)
                    _gridInfo = CalculateSize();
                return _gridInfo.MaxCol - _gridInfo.MinCol;
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
