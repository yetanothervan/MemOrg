﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Layouts
{
    public abstract class StackLayout
    {
        private readonly IEnumerable<IComponent> _childs;
        private readonly double _margin;

        protected StackLayout(IEnumerable<IComponent> childs, double margin)
        {
            _childs = childs;
            _margin = margin;
        }

        protected Size CalculateSize(bool isHorizontal)
        {
            if (!_childs.Any()) return new Size(0, 0);
            double length =
                isHorizontal
                    ? _childs.Sum(child => (child.GetActualSize().Width + _margin)) + _margin
                    : _childs.Sum(child => (child.GetActualSize().Height + _margin)) + _margin;
            double width =
                isHorizontal
                    ? _childs.Max(child => (child.GetActualSize().Height)) + _margin * 2
                    : _childs.Max(child => (child.GetActualSize().Width)) + _margin*2;

            return isHorizontal
                ? new Size { Height = width, Width = length }
                : new Size { Height = length, Width = width };
        }

        public abstract Size CalculateSize();

        protected IEnumerable<Visual> Render(Point p, bool isHorizontal)
        {
            var result = new List<Visual>();
            if (!_childs.Any()) return result;

            var curPt = p;
            curPt.Offset(_margin, _margin);
            foreach (var child in _childs)
            {
                result.AddRange(child.Render(curPt));
                if (isHorizontal)
                    curPt.Offset(child.GetActualSize().Width +_margin, 0.0);
                else
                    curPt.Offset(0.0, child.GetActualSize().Height + _margin);
            }
            return result;
        }

        public abstract IEnumerable<Visual> Render(Point p);
    }
}