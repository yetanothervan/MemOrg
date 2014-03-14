using System;
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
            _cachedSize = null;
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
}
