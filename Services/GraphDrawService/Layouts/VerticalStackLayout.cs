using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Layouts
{
    public class VerticalStackLayout : StackLayout
    {
        private const bool IsHorizontal = false;
        public VerticalStackLayout(IEnumerable<IComponent> childs, double margin) : base(childs, margin)
        {
        }

        public override Size CalculateSize()
        {
            return CalculateSize(IsHorizontal);
        }

        public override IEnumerable<DrawingVisual> Render(Point p)
        {
            return Render(p, IsHorizontal);
        }
    }
}
