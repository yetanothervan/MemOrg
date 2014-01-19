using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class Grid : IGrid, IEnumerable<VisualGraphElem>
    {
        private List<List<VisualGraphElem>> Elems;

        public IEnumerator<VisualGraphElem> GetEnumerator()
        {
            return new GridEnumerator(Elems);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class GridElem
    {
        
    }
    
    public class GridEnumerator : IEnumerator<VisualGraphElem>
    {
        private readonly List<List<VisualGraphElem>> _elems;
        public GridEnumerator(List<List<VisualGraphElem>> elems)
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

        private IEnumerator<List<VisualGraphElem>> _rowsEnumerator;
        private IEnumerator<VisualGraphElem> _cellsEnumerator;
        public void Reset()
        {
            _rowsEnumerator = null;
            _cellsEnumerator = null;
        }

        public VisualGraphElem Current
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
