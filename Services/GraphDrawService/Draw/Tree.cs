using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    public class Tree : Component
    {
        private const double Margin = 10.0;
        private const double RootLineHeight = 10;
        
        private readonly IDrawStyle _style;
        
        public Tree(IDrawStyle style)
        {
            _style = style;
        }

        public override List<DrawingVisual> Render(Point p1, Point? p2)
        {
            var res = new List<DrawingVisual>();
            if (Childs != null && Childs.Count > 0)
            {
                Size rootSize = Childs[0].GetActualSize();
                Size childSize = new HorizontalStackLayout(Childs.Skip(1), Margin).CalculateSize();
                var leftTopRootPoint = new Point(Math.Max(childSize.Width, rootSize.Width)/2 - rootSize.Width/2 + p1.X, p1.Y);
                res.AddRange(Childs[0].Render(leftTopRootPoint, p2)); //draw root elem
                
                //draw lines
                if (Childs.Count > 1)
                {
                    p1.Offset(0.0, rootSize.Height + RootLineHeight);
                    var lines =  new DrawingVisual();
                    using (var dc = lines.RenderOpen())
                    {
                        var bottomStemPoint = new Point(leftTopRootPoint.X + rootSize.Width/2.0, p1.Y);
                        dc.DrawLine(_style.OthersBlockPen,
                            new Point(bottomStemPoint.X, bottomStemPoint.Y - RootLineHeight),
                            bottomStemPoint);

                        var first = Childs.Skip(1).First();
                        var last = Childs.Skip(1).Last();

                        dc.DrawLine(_style.OthersBlockPen,
                            new Point(p1.X + Margin + first.GetActualSize().Width/2.0, p1.Y),
                            new Point(p1.X + childSize.Width - Margin - last.GetActualSize().Width / 2.0, p1.Y));

                        if (childSize.Width < rootSize.Width)
                            p1.Offset((rootSize.Width - childSize.Width) / 2.0, 0.0);

                        var lp = new Point(p1.X, p1.Y);
                        
                        foreach (var elem in Childs.Skip(1))
                        {
                            var w = elem.GetActualSize().Width;
                            lp.Offset(w + Margin, 0.0);
                            dc.DrawLine(_style.OthersBlockPen,
                                new Point(lp.X - w / 2.0, lp.Y),
                                new Point(lp.X - w / 2.0, lp.Y + Margin));
                        }
                    }
                    res.Add(lines);
                    res.AddRange(new HorizontalStackLayout(Childs.Skip(1), Margin).Render(p1));
                }
            }
            return res;
        }

        public override Size GetActualSize()
        {
            double height = 0;
            double width = 0;
            
            if (Childs != null && Childs.Count > 0)
            {
                height += Childs[0].GetActualSize().Height;
                width += Childs[0].GetActualSize().Width;
                Size childSize = new HorizontalStackLayout(Childs.Skip(1), Margin).CalculateSize();
                height += childSize.Height;
                height += RootLineHeight;
                width = Math.Max(childSize.Width, width);
            }

            return new Size(width, height);
        }
    }
}