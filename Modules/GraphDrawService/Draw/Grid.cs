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
    public class Grid : IComponent
    {
        private const double Margin = 5.0;

        private readonly IDrawStyle _style;
        
        public Grid(IDrawStyle style)
        {
            _style = style;
        }

        private List<IComponent> _childs;
        public List<IComponent> Childs
        {
            get
            {
                if (_childs == null) Childs = new List<IComponent>(0);
                return _childs;
            }
            set { _childs = value; }
        }
        
        public List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetSize());
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);

            foreach (var child in _childs)
            {
                var gridElem = child as IGridElem;
                if (gridElem == null) continue;

                var x = _colWidths.Where(o => o.Key < gridElem.ColIndex).Sum(o => o.Value)
                           + (_colWidths.Count(o => o.Key < gridElem.ColIndex) + 1)*Margin;
                
                var y = _rowHeights.Where(o => o.Key < gridElem.RowIndex).Sum(o => o.Value)
                           + (_rowHeights.Count(o => o.Key < gridElem.RowIndex) + 1) * Margin;

                var pC= new Point(x,y);
                result.AddRange(child.Render(pC));
            }

            return result;
        }

        public Size GetSize()
        {
            if (_colWidths == null)
                CalculateGrid();

// ReSharper disable once AssignNullToNotNullAttribute
// ReSharper disable once PossibleNullReferenceException
            return new Size(_colWidths.Sum(o => o.Value) + (_colWidths.Count + 1) * Margin,
                _rowHeights.Sum(o => o.Value) + (_rowHeights.Count + 1) * Margin);
        }

        private Dictionary<int, double> _colWidths; 
        private Dictionary<int, double> _rowHeights;
        
        void CalculateGrid()
        {
            _colWidths = new Dictionary<int, double>();
            _rowHeights = new Dictionary<int, double>();

            foreach (var child in Childs)
            {
                var gridElem = child as IGridElem;
                if (gridElem == null) continue;
                
                var elemSize = child.GetSize();

                if (!_colWidths.ContainsKey(gridElem.ColIndex) 
                    || _colWidths[gridElem.ColIndex] < elemSize.Width)
                    _colWidths[gridElem.ColIndex] = elemSize.Width;
                
                if (!_rowHeights.ContainsKey(gridElem.RowIndex)
                    || _rowHeights[gridElem.RowIndex] < elemSize.Height)
                    _rowHeights[gridElem.RowIndex] = elemSize.Height;
            }
        }
    }
}
