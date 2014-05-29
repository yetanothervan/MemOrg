﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.Chapter
{
    public class ChapterArrow
    {
        private readonly List<ChapterLayoutElem> _elems;
        private readonly ChapterLayoutElem _first;
        private readonly ChapterLayoutElem _second;
        private readonly PageEdge _link;

        public ChapterArrow(List<ChapterLayoutElem> elems, ChapterLayoutElem first, ChapterLayoutElem second, PageEdge link)
        {
            _elems = elems;
            _first = first;
            _second = second;
            _link = link;
        }

        public void Draw()
        {
            if (_second == null || _first == null) return;
           
            if (_first.Row == _second.Row)
            {
                DrawHorizontal();
                return;
            }
            
            if (_first.Col == _second.Col)
            {
                DrawVertical();
                return;
            }

            if (Math.Abs(_first.Col - _second.Col) == 2)
            {
                DrawNextColumn();
                return;
            }

            DrawElse();
        }

        private void DrawHorizontal()
        {
            _first.AddCon(_first.Col > _second.Col ? NESW.West : NESW.East);
            _second.AddCon(_first.Col > _second.Col ? NESW.East : NESW.West);
            AddLink(_first.Row, Math.Min(_first.Col, _second.Col) + 1, GridLinkPartDirection.WestEast, _link.PageLink.RelName);
        }

        private void DrawElse()
        {
            bool fromWest = false;
            bool toNorth = false;
            if (_first.Col > _second.Col) _first.AddCon(NESW.West);
            else
            {
                _first.AddCon(NESW.East);
                fromWest = true;
            }
            if (_first.Row > _second.Row)
            {
                _second.AddCon(NESW.South);
                toNorth = true;
            }
            else _second.AddCon(NESW.North);

            if (fromWest)
                AddLink(_first.Row, _second.Col,
                    toNorth ? GridLinkPartDirection.NorthWest : GridLinkPartDirection.WestSouth);
            else
                AddLink(_first.Row, _second.Col,
                    toNorth ? GridLinkPartDirection.SouthEast : GridLinkPartDirection.NorthEast);

            for (int i = 1; i < Math.Abs(_first.Col - _second.Col); ++i)
            {
                var x = (fromWest ? _first.Col + i : _first.Col - i);
                AddLink(_first.Row, x, GridLinkPartDirection.WestEast);
            }
            for (int i = 1; i < Math.Abs(_first.Row - _second.Row); ++i)
            {
                var y = (toNorth ? _first.Row - i : _first.Row + i);
                AddLink(y, _second.Col, GridLinkPartDirection.NorthSouth);
            }
        }

        private void DrawNextColumn()
        {
            var c = Math.Min(_first.Col, _second.Col) + 1;
            _first.AddCon(_first.Col < c ? NESW.East : NESW.West);
            _second.AddCon(_second.Col < c ? NESW.East : NESW.West);

            var mindir = _first.Row > _second.Row
                ? (_first.Col > _second.Col ? GridLinkPartDirection.WestSouth : GridLinkPartDirection.SouthEast)
                : (_first.Col > _second.Col ? GridLinkPartDirection.SouthEast : GridLinkPartDirection.WestSouth);

            var maxdir = _first.Row > _second.Row
                ? (_first.Col > _second.Col ? GridLinkPartDirection.NorthEast : GridLinkPartDirection.NorthWest)
                : (_first.Col > _second.Col ? GridLinkPartDirection.NorthWest : GridLinkPartDirection.NorthEast);

            AddLink(Math.Min(_first.Row, _second.Row), c, mindir);
            AddLink(Math.Max(_first.Row, _second.Row), c, maxdir);
            for (int i = Math.Min(_first.Row, _second.Row) + 1; i < Math.Max(_first.Row, _second.Row); ++i)
                AddLink(i, c, GridLinkPartDirection.NorthSouth);
        }

        private void DrawVertical()
        {
            var col = _first.Col == 4 ? _first.Col + 1 : _first.Col - 1;
            var dir = col == 5 ? NESW.East : NESW.West;
            _first.AddCon(dir);
            _second.AddCon(dir);
            AddLink(Math.Min(_first.Row, _second.Row), col,
                col == 5 ? GridLinkPartDirection.WestSouth : GridLinkPartDirection.SouthEast);
            AddLink(Math.Max(_first.Row, _second.Row), col,
                col == 5 ? GridLinkPartDirection.NorthWest : GridLinkPartDirection.NorthEast);
            
            int to = Math.Max(_first.Row, _second.Row);
            for (int i = Math.Min(_first.Row, _second.Row) + 1; i < to; ++i)
                AddLink(i, col, GridLinkPartDirection.NorthSouth, 
                    i + 1 == to ? _link.PageLink.RelName : null);
        }
        
        private void AddLink(int row, int col, GridLinkPartDirection dir, string caption = null)
        {
            var l = new ChapterLayoutElem { Col = col, Row = row, 
                HorizontalAligment = HorizontalAligment.Center};
            GridLinkPartType type = (_link.PageLink.LinkType == PageLinkType.ReferenceTo)
                ? GridLinkPartType.Reference
                : GridLinkPartType.Relation;
            l.AddGridLink(new GridLinkPart {Direction = dir, Type = type, Caption = caption});
            _elems.Add(l);
        }
    }
}
