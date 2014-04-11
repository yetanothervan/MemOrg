using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity.Utility;

namespace MemOrg.Interfaces
{
    public class GridEnumerator<T> : IEnumerator<T> where T : IGridElem
    {
        private readonly SortedDictionary<IntIntPair, T> _elems;
        private readonly List<IntIntPair> _pairs;

        public GridEnumerator(SortedDictionary<IntIntPair, T> elems)
        {
            _elems = elems;
            _pairs = _elems.Keys.OrderBy(o => o.Row).ThenBy(o => o.Col).ToList();
            Reset();
        }

        public void Dispose()
        {
            if (_iter != null) _iter.Dispose();
        }
        
        public bool MoveNext()
        {
            if (_iter == null)
                _iter = _pairs.GetEnumerator();

            return _iter.MoveNext();
        }
        
        public void Reset()
        {
            _iter = null;
        }

        private IEnumerator<IntIntPair> _iter;
        public T Current
        {
            get
            {
                if (_iter == null)
                    throw new IndexOutOfRangeException();
                return _elems[_iter.Current];
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}