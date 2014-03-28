namespace MemOrg.Interfaces
{
    public struct IntIntPair
    {
        public readonly int Row;
        public readonly int Col;

        public IntIntPair(int row, int col) : this()
        {
            Row = row;
            Col = col;
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IntIntPair)) return false;
            var ob = (IntIntPair)obj;
            return ob.Col == Col && ob.Row == Row;
        }
    }
}