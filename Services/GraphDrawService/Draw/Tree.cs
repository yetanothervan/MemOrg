using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GraphDrawService.Layouts;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    public class Tree : IComponent
    {
        private const double Margin = 10.0;
        public List<IComponent> Childs { get; set; }
        public List<DrawingVisual> Render(Point p)
        {
            return new HorizontalStackLayout(Childs, Margin).Render(p).ToList();
        }

        public Size GetSize()
        {
            double height = 0;
            double width = 0;
            
            if (Childs != null && Childs.Count > 0)
            {
                height += Childs[0].GetSize().Height;
                width += Childs[0].GetSize().Height;
                Size childSize = new HorizontalStackLayout(Childs.Skip(1), Margin).CalculateSize();
                height += childSize.Height;
                width = Math.Max(childSize.Width, width);
            }

            return new Size(width, height);
        }
    }
}