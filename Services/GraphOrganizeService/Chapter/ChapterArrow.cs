using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.Chapter
{
    public static class ChapterArrow
    {
        public static void Draw(List<ChapterLayoutElem> elems, ChapterLayoutElem f, ChapterLayoutElem s)
        {
            if (s != null && f != null)
            {
                if (f.Row == s.Row)
                {
                    f.AddCon(f.Col > s.Col ? NESW.West : NESW.East);
                    s.AddCon(f.Col > s.Col ? NESW.East : NESW.West);
                    AddLink(elems, f.Row, Math.Min(f.Col, s.Col) + 1, GridLinkPartDirection.WestEast);
                    return;
                }
                if (f.Col == s.Col)
                {
                    var col = f.Col == 4 ? f.Col + 1 : f.Col - 1;
                    var dir = col == 5 ? NESW.East : NESW.West;
                    f.AddCon(dir);
                    s.AddCon(dir);
                    AddLink(elems, Math.Min(f.Row, s.Row), col,
                        col == 5 ? GridLinkPartDirection.WestSouth : GridLinkPartDirection.SouthEast);
                    AddLink(elems, Math.Max(f.Row, s.Row), col,
                        col == 5 ? GridLinkPartDirection.NorthWest : GridLinkPartDirection.NorthEast);
                    for (int i = Math.Min(f.Row, s.Row) + 1; i < Math.Max(f.Row, s.Row); ++i)
                        AddLink(elems, i, col, GridLinkPartDirection.NorthSouth);
                    return;
                }

                if (Math.Abs(f.Col - s.Col) == 2)
                {
                    var c = Math.Min(f.Col, s.Col) + 1;
                    f.AddCon(f.Col < c ? NESW.East : NESW.West);
                    s.AddCon(s.Col < c ? NESW.East : NESW.West);

                    var mindir = f.Row > s.Row
                        ? (f.Col > s.Col ? GridLinkPartDirection.WestSouth : GridLinkPartDirection.SouthEast)
                        : (f.Col > s.Col ? GridLinkPartDirection.SouthEast : GridLinkPartDirection.WestSouth);

                    var maxdir = f.Row > s.Row
                        ? (f.Col > s.Col ? GridLinkPartDirection.NorthEast : GridLinkPartDirection.NorthWest)
                        : (f.Col > s.Col ? GridLinkPartDirection.NorthWest : GridLinkPartDirection.NorthEast);

                    AddLink(elems, Math.Min(f.Row, s.Row), c, mindir);
                    AddLink(elems, Math.Max(f.Row, s.Row), c, maxdir);
                    for (int i = Math.Min(f.Row, s.Row) + 1; i < Math.Max(f.Row, s.Row); ++i)
                        AddLink(elems, i, c, GridLinkPartDirection.NorthSouth);
                    return;
                }
                
                bool fromWest = false;
                bool toNorth = false;
                if (f.Col > s.Col) f.AddCon(NESW.West);
                else {f.AddCon(NESW.East);
                    fromWest = true;
                }
                if (f.Row > s.Row) {s.AddCon(NESW.South);
                    toNorth = true;
                }
                else s.AddCon(NESW.North);

                if (fromWest)
                    AddLink(elems, f.Row, s.Col,
                        toNorth ? GridLinkPartDirection.NorthWest : GridLinkPartDirection.WestSouth);
                else
                    AddLink(elems, f.Row, s.Col,
                    toNorth ? GridLinkPartDirection.SouthEast : GridLinkPartDirection.NorthEast);

                for (int i = 1; i < Math.Abs(f.Col - s.Col); ++i)
                {
                    var x = (fromWest ? f.Col + i : f.Col - i);
                    AddLink(elems, f.Row, x, GridLinkPartDirection.WestEast);
                }
                for (int i = 1; i < Math.Abs(f.Row - s.Row); ++i)
                {
                    var y = (toNorth ? f.Row - i : f.Row + i);
                    AddLink(elems, y, s.Col, GridLinkPartDirection.NorthSouth);
                }

            }
        }

        private static void AddLink(List<ChapterLayoutElem> elems, int row, int col, GridLinkPartDirection dir)
        {
            var l = new ChapterLayoutElem { Col = col, Row = row , 
                HorizontalAligment = HorizontalAligment.Center};
            l.AddGridLink(new GridLinkPart { Direction = dir });
            elems.Add(l);
        }
    }
}
