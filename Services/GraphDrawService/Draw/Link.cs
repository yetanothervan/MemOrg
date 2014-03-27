﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphDrawService.Draw
{
    class Link : Component 
    {
        private readonly Pen _pen;

        public Link(Pen pen)
        {
            _pen = pen;
        }

        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                if (PreferSize == null)
                    return result;
                dc.DrawLine(_pen,
                    new Point(
                        p.X,
                        p.Y + (PreferSize != null ? PreferSize.Value.Height / 2 : 15)),
                    new Point(
                        p.X + (PreferSize != null ? PreferSize.Value.Width : 10),
                        p.Y + (PreferSize != null ? PreferSize.Value.Height / 2 : 15)
                        ));
            }
            result.Add(dv);
            return result;
        }
        
        public override Size GetActualSize()
        {
            return new Size(10, 1);
        }
    }
}
