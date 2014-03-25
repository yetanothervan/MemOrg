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

        public override List<DrawingVisual> Render(Point p1, Point? p2)
        {
            if (Childs.Count == 0)
                return null;
            return Childs[0].Render(p1, p2);
        }

        public override Size GetActualSize()
        {
            if (Childs.Count == 0)
                return new Size(0, 0);
            return Childs[0].GetActualSize();
        }
    }
}
