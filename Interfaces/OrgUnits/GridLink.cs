namespace MemOrg.Interfaces.OrgUnits
{
    public enum NESW
    {
        North, East, South, West
    }

    public class GridLinkPart
    {
        public GridLinkPartType Type;
        public GridLinkPartDirection Direction;
        public string Caption;
    }

    public enum GridLinkPartDirection
    {
        NorthSouth,
        NorthWest,
        NorthEast,
        WestEast,
        WestSouth,
        SouthEast
    }

    public enum GridLinkPartType
    {
        Source,
        Relation,
        Reference
    }
}
