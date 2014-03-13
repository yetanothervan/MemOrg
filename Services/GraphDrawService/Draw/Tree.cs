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

        public override List<DrawingVisual> Render(Point p)
        {
            var res = new List<DrawingVisual>();
            if (Childs != null && Childs.Count > 0)
            {
                Size rootSize = Childs[0].GetSize();
                Size childSize = new HorizontalStackLayout(Childs.Skip(1), Margin).CalculateSize();
                var leftTopRootPoint = new Point(Math.Max(childSize.Width, rootSize.Width)/2 - rootSize.Width/2 + p.X, p.Y);
                res.AddRange(Childs[0].Render(leftTopRootPoint)); //draw root elem
                
                //draw lines
                if (Childs.Count > 1)
                {
                    p.Offset(0.0, rootSize.Height + RootLineHeight);
                    var lines =  new DrawingVisual();
                    using (var dc = lines.RenderOpen())
                    {
                        var bottomStemPoint = new Point(leftTopRootPoint.X + rootSize.Width/2.0, p.Y);
                        dc.DrawLine(_style.OthersBlockPen,
                            new Point(bottomStemPoint.X, bottomStemPoint.Y - RootLineHeight),
                            bottomStemPoint);

                        var first = Childs.Skip(1).First();
                        var last = Childs.Skip(1).Last();

                        dc.DrawLine(_style.OthersBlockPen,
                            new Point(p.X + Margin + first.GetSize().Width/2.0, p.Y),
                            new Point(p.X + childSize.Width - Margin - last.GetSize().Width / 2.0, p.Y));

                        if (childSize.Width < rootSize.Width)
                            p.Offset((rootSize.Width - childSize.Width) / 2.0, 0.0);

                        var lp = new Point(p.X, p.Y);
                        
                        foreach (var elem in Childs.Skip(1))
                        {
                            var w = elem.GetSize().Width;
                            lp.Offset(w + Margin, 0.0);
                            dc.DrawLine(_style.OthersBlockPen,
                                new Point(lp.X - w / 2.0, lp.Y),
                                new Point(lp.X - w / 2.0, lp.Y + Margin));
                        }
                    }
                    res.Add(lines);
                    res.AddRange(new HorizontalStackLayout(Childs.Skip(1), Margin).Render(p));
                }
            }
            return res;
        }

        public override Size GetSize()
        {
            double height = 0;
            double width = 0;
            
            if (Childs != null && Childs.Count > 0)
            {
                height += Childs[0].GetSize().Height;
                width += Childs[0].GetSize().Width;
                Size childSize = new HorizontalStackLayout(Childs.Skip(1), Margin).CalculateSize();
                height += childSize.Height;
                height += RootLineHeight;
                width = Math.Max(childSize.Width, width);
            }

            return new Size(width, height);
        }
    }
}