using System;

namespace MemOrg.Interfaces
{
    public struct IntIntPair : IComparable
    {
        public bool Equals(IntIntPair other)
        {
            return Row == other.Row && Col == other.Col;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row*397) ^ Col;
            }
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (!(obj is IntIntPair)) return 1;
            var pair = (IntIntPair) obj;
            if (Row > pair.Row) return 1;
            if (Row < pair.Row) return -1;
            return
                Col.CompareTo(pair.Col);
        }

        public readonly int Row;
        public readonly int Col;

        public IntIntPair(int row, int col) : this()
        {
            Row = row;
            Col = col;
        }

        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntIntPair && Equals((IntIntPair) obj);
        }
    }
}