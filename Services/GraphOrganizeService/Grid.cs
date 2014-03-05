using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;
using Microsoft.Practices.Unity.Utility;

namespace GraphOrganizeService
{
    public class Grid : IGrid
    {
        private readonly Dictionary<Pair<int, int>, IGridElem> _elems;
        private readonly List<GridLink> _links;
        
        public Grid()
        {
            _elems = new Dictionary<Pair<int, int>, IGridElem>();
            _links = new List<GridLink>();
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
            _cachedSize = null;
        }

        public void AddLink(int fromRow, int fromCol, NESW cpb, int toRow, int toCol, NESW cpe)
        {
            var link = new GridLink
            {
                Begin = new GridLinkPoint {Col = fromCol, ConnectionPoint = cpb, Row = fromRow},
                End = new GridLinkPoint {Col = toCol, ConnectionPoint = cpe, Row = toRow}
            };
            _links.Add(link);
        }

        private Pair<int,int> _cachedSize;

        private void CalculateSize()
        {
            _cachedSize = new Pair<int, int>
                (_elems.Keys.Max(o => o.First) - _elems.Keys.Min(o => o.First),
                    _elems.Keys.Max(o => o.Second) - _elems.Keys.Min(o => o.Second)
                );
        }

        public int RowCount {
            get
            {
                if (_cachedSize == null)
                    CalculateSize();
// ReSharper disable once PossibleNullReferenceException
                return _cachedSize.First;
            } 
        }

        public int RowLength
        {
            get
            {
                if (_cachedSize == null)
                    CalculateSize();
// ReSharper disable once PossibleNullReferenceException
                return _cachedSize.Second;
            }
        }
    }

    public class GridLink
    {
        public GridLinkPoint Begin;
        public GridLinkPoint End;
    }

    public struct GridLinkPoint
    {
        public NESW ConnectionPoint;
        public int Row;
        public int Col;
    }

    public enum NESW
    {
        North, East, South, West
    }
}
