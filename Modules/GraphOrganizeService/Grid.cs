using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class Grid : IGrid, IEnumerable<GridElem>
    {
        private readonly List<List<GridElem>> _elems;

        public Grid(int rowLength, int rowsCount)
        {
            _rowLength = rowLength;
            _rowsCount = rowsCount;
            _elems = new List<List<GridElem>>(rowsCount);
            for (int index = 0; index < _elems.Count; ++index)
                _elems[index] = new List<GridElem>(rowLength);
        }

        private readonly int _rowsCount;
        private readonly int _rowLength;
        public int GetRowsCount { get { return _rowsCount; } }
        public int GetRowLength { get { return _rowLength; } }

        public void SetElemOn(int row, int col, GridElem ge)
        {
            ge.PlaceOn(row, col, _elems);
        }

        public IEnumerator<GridElem> GetEnumerator()
        {
            return new GridEnumerator(_elems);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class GridElem
    {
        private int _rowIndex = -1;
        private int _colIndex = -1;

        public void PlaceOn(int row, int col, List<List<GridElem>> elems)
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

    public enum GridElemBasedOnBlockType
    {
        BlockOther, BlockSource, BlockTag, BlockRel
    }
    public class GridElemBasedOnBlock : GridElem
    {
        private readonly Block _block;
        private readonly GridElemBasedOnBlockType _type;
        public GridElemBasedOnBlock(Block block, GridElemBasedOnBlockType type)
        {
            _block = block;
            _type = type;
        }

        public Block Block
        {
            get { return _block; }
        }

        public GridElemBasedOnBlockType Type
        {
            get { return _type; }
        }
    }

    public class GridElemBasedOnTag : GridElem
    {
        private Tag _tag;
        public GridElemBasedOnTag(Tag tag)
        {
            _tag = tag;
        }
    }



    public class GridEnumerator : IEnumerator<GridElem>
    {
        private readonly List<List<GridElem>> _elems;
        public GridEnumerator(List<List<GridElem>> elems)
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

        private IEnumerator<List<GridElem>> _rowsEnumerator;
        private IEnumerator<GridElem> _cellsEnumerator;
        public void Reset()
        {
            _rowsEnumerator = null;
            _cellsEnumerator = null;
        }

        public GridElem Current
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
