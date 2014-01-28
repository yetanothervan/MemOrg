using System;
using System.Collections;
using System.Collections.Generic;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class GridEnumerator : IEnumerator<IGridElem>
    {
        private readonly List<List<IGridElem>> _elems;
        public GridEnumerator(List<List<IGridElem>> elems)
        {
            _elems = elems;
            Reset();
        }

        public void Dispose()
        {
            if (_rowsEnumerator != null) _rowsEnumerator.Dispose();
            if (_cellsEnumerator != null) _cellsEnumerator.Dispose();
        }
        
        public bool MoveNext()
        {
            if (_rowsEnumerator == null)
            {
                _rowsEnumerator = _elems.GetEnumerator();
                if (_rowsEnumerator.MoveNext() == false)
                    return false;
                //else
                _cellsEnumerator = _rowsEnumerator.Current.GetEnumerator();
            }

            if (_cellsEnumerator.MoveNext() == false)
            {
                if (_rowsEnumerator.MoveNext() == false)
                    return false;
                _cellsEnumerator = _rowsEnumerator.Current.GetEnumerator();
                MoveNext();
            }
            return true;
        }

        private IEnumerator<List<IGridElem>> _rowsEnumerator;
        private IEnumerator<IGridElem> _cellsEnumerator;
        public void Reset()
        {
            _rowsEnumerator = null;
            _cellsEnumerator = null;
        }

        public IGridElem Current
        {
            get
            {
                if (_cellsEnumerator == null)
                    throw new IndexOutOfRangeException();
                return _cellsEnumerator.Current;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}