using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphDrawService.Draw
{
    class GridElem: Component
    {
        public GridElem(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Col { get; private set; }
        public int Row { get; private set; }

        public override List<DrawingVisual> Render(Point p)
        {
            if (Childs.Count == 0)
                return null;
            return Childs[0].Render(p);
        }

        public override Size GetSize()
        {
            if (Childs.Count == 0)
                return new Size(0, 0);
            return Childs[0].GetSize();
        }
    }
}
