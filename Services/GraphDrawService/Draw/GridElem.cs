using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    class GridElem: Component
    {
        private Size? _preferSize;

        public GridElem(IOrgGridElem orgElem)
        {
            Row = orgElem.RowIndex;
            Col = orgElem.ColIndex;
            HorizontalAligment = orgElem.HorizontalContentAligment;
            VerticalAligment = orgElem.VerticalContentAligment;
        }

        public GridElem(int row, int col)
        {
            Row = row;
            Col = col;
            HorizontalAligment = HorizontalAligment.Left;
            VerticalAligment = VerticalAligment.Top;
        }

        public int Col { get; private set; }
        public int Row { get; private set; }

        public HorizontalAligment HorizontalAligment { get; set; }
        public VerticalAligment VerticalAligment { get; set; }

        public override List<DrawingVisual> Render(Point p)
        {
            if (!Childs.Any())
                return null;
            
            if (PreferSize != null)
            {
                double x, y;
                if (HorizontalAligment == HorizontalAligment.Right)
                    x = p.X + PreferSize.Value.Width - GetActualSize().Width;
                else if (HorizontalAligment == HorizontalAligment.Center)
                    x = p.X + (PreferSize.Value.Width - GetActualSize().Width)/2;
                else x = p.X;

                if (VerticalAligment == VerticalAligment.Bottom)
                    y = p.Y + PreferSize.Value.Height - GetActualSize().Height;
                else if (VerticalAligment == VerticalAligment.Center)
                    y = p.Y + (PreferSize.Value.Height - GetActualSize().Height)/2;
                else y = p.Y;

                return Childs.First().Render(new Point(x, y));
            }

            return Childs.First().Render(p);
        }

        public override Size? PreferSize
        {
            get { return _preferSize; }
            set
            {
                _preferSize = value;
                if (Childs.Any())
                    Childs.First().PreferSize = value;
            }
        }

        public override Size GetActualSize()
        {
            if (!Childs.Any())
                return new Size(0, 0);
            return Childs.First().GetActualSize();
        }
    }
}
