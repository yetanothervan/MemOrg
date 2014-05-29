using System;

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

    public class ConnectionPoint : IEquatable<ConnectionPoint>
    {
        public readonly NESW Direction;
        public readonly GridLinkPartType LinkType;

        public ConnectionPoint(NESW direction, GridLinkPartType linkType)
        {
            Direction = direction;
            LinkType = linkType;
        }
        
        public bool Equals(ConnectionPoint other)
        {
            return Direction == other.Direction && LinkType == other.LinkType;
        }
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
