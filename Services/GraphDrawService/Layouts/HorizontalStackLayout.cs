using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Layouts
{
    public class HorizontalStackLayout : StackLayout
    {
        private const bool IsHorizontal = true;
        public HorizontalStackLayout(IEnumerable<IComponent> childs, double margin)
            : base(childs, margin)
        {
        }

        public override Size CalculateSize()
        {
            return CalculateSize(IsHorizontal);
        }

        public override IEnumerable<Visual> Render(Point p)
        {
            return Render(p, IsHorizontal);
        }
    }
}
