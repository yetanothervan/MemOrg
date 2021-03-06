﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace GraphDrawService.Draw
{
    class Link : Component 
    {
        private readonly IDrawStyle _style;
        private readonly IReadOnlyList<GridLinkPart> _gridLinkParts;

        public Link(IDrawStyle style, IReadOnlyList<GridLinkPart> gridLinkParts)
        {
            _style = style;
            _gridLinkParts = gridLinkParts;
        }

        private const int halfsize = 7;

        public override List<Visual> Render(Point p)
        {
            var result = new List<Visual>();
            if (_gridLinkParts == null)
                return result;

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                if (PreferSize == null)
                    return result;

                var halfWidth = (PreferSize != null ? PreferSize.Value.Width / 2 : halfsize);
                var halfHeight = (PreferSize != null ? PreferSize.Value.Height / 2 : halfsize);

                var c = new Point(p.X + halfWidth, p.Y + halfHeight);

                var w = new Point(p.X, c.Y);
                var e = new Point(c.X + halfWidth, c.Y);
                
                var n = new Point(c.X, p.Y);
                var s = new Point(c.X, c.Y + halfHeight);

                var cn = new Point(c.X, c.Y - halfsize);
                var cs = new Point(c.X, c.Y + halfsize);
                var cw = new Point(c.X - halfsize, c.Y);
                var ce = new Point(c.X + halfsize, c.Y);
                
                foreach (var part in _gridLinkParts)
                {
                    var partPen = part.Type == GridLinkPartType.Reference 
                        ? _style.ReferenceLinkPen 
                        : _style.RelationBlockPen;
                    /*if (part.Direction == GridLinkPartDirection.NorthEast
                        || part.Direction == GridLinkPartDirection.NorthWest
                        || part.Direction == GridLinkPartDirection.NorthSouth)
                        dc.DrawLine(_pen, n, c);
                    if (part.Direction == GridLinkPartDirection.SouthEast
                        || part.Direction == GridLinkPartDirection.WestSouth
                        || part.Direction == GridLinkPartDirection.NorthSouth)
                        dc.DrawLine(_pen, c, s);
                    if (part.Direction == GridLinkPartDirection.NorthWest
                        || part.Direction == GridLinkPartDirection.WestEast
                        || part.Direction == GridLinkPartDirection.WestSouth)
                        dc.DrawLine(_pen, w, c);
                    if (part.Direction == GridLinkPartDirection.NorthEast
                        || part.Direction == GridLinkPartDirection.SouthEast
                        || part.Direction == GridLinkPartDirection.WestEast)
                        dc.DrawLine(_pen, c, e);*/
                    
                    switch (part.Direction)
                    {
                        case GridLinkPartDirection.NorthSouth:
                            dc.DrawLine(partPen, n, s);
                            break;
                        case GridLinkPartDirection.WestEast:
                            dc.DrawLine(partPen, w, e);
                            break;
                        case GridLinkPartDirection.NorthWest:
                        {
                            dc.DrawGeometry(Brushes.Transparent, partPen,
                                GetArc(cn, cw, SweepDirection.Clockwise));
                            if (n != cn)
                                dc.DrawLine(partPen, n, cn);
                            if (w != cw)
                                dc.DrawLine(partPen, w, cw);
                        }
                            break;
                        case GridLinkPartDirection.NorthEast:
                        {
                            dc.DrawGeometry(Brushes.Transparent, partPen,
                                GetArc(cn, ce, SweepDirection.Counterclockwise));
                            if (n != cn)
                                dc.DrawLine(partPen, n, cn);
                            if (e != ce)
                                dc.DrawLine(partPen, e, ce);
                        }
                            break;
                        case GridLinkPartDirection.SouthEast:
                            dc.DrawGeometry(Brushes.Transparent, partPen,
                                GetArc(cs, ce, SweepDirection.Clockwise));
                            if (s != cs)
                                dc.DrawLine(partPen, s, cs);
                            if (e != ce)
                                dc.DrawLine(partPen, e, ce);
                            break;
                        case GridLinkPartDirection.WestSouth:
                            dc.DrawGeometry(Brushes.Transparent, partPen,
                                GetArc(cw, cs, SweepDirection.Clockwise));
                            if (w != cw)
                                dc.DrawLine(partPen, w, cw);
                            if (s != cs)
                                dc.DrawLine(partPen, s, cs);
                            break;
                    }

                    if (!String.IsNullOrWhiteSpace(part.Caption))
                    {
                        var ft = new FormattedText(part.Caption, CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight, _style.TextTypeface,
                            _style.TextEmSize, _style.TextBrush);
                        var tp = new Point(c.X - ft.Width/2, c.Y - ft.Height/2);
                        dc.DrawText(ft, tp);
                    }
                }
            }
            result.Add(dv);
            return result;
        }

        private Geometry GetArc(Point p1, Point p2, SweepDirection dir)
        {
            return new PathGeometry(new List<PathFigure>
                    {
                        new PathFigure(p1, new List<PathSegment>
                        {
                            new ArcSegment(p2, new Size(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y)), 0,
                                false, dir, true)
                        }, false)
                    });
        }

        public override Size GetActualSize()
        {
            return new Size(halfsize*2, halfsize*2);
        }
    }
}
