using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphDrawService.Draw
{
    public class Grid : Component
    {
        private const double Margin = 0.0;

        private readonly IDrawStyle _style;

        public IDictionary<int, double> ColStarWidths;
        public IDictionary<int, double> RowStarHeights;
        
        public Grid(IDrawStyle style)
        {
            _style = style;
        }

        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();
            
            foreach (var child in Childs)
            {
                var gridElem = child as GridElem;
                if (gridElem == null) 
                    continue;

                gridElem.PreferSize = new Size(_colWidths[gridElem.Col], _rowHeights[gridElem.Row]);

                var x = p.X + _colWidths.Where(o => o.Key < gridElem.Col).Sum(o => o.Value)
                        + (_colWidths.Count(o => o.Key < gridElem.Col) + 1)*Margin;

                var y = p.Y + _rowHeights.Where(o => o.Key < gridElem.Row).Sum(o => o.Value)
                        + (_rowHeights.Count(o => o.Key < gridElem.Row) + 1)*Margin;

                result.AddRange(child.Render(
                    new Point(x, y)));
            }

            return result;
        }

        public override Size? PreferSize
        {
            get { return _preferSize ?? (_preferSize = new Size()); }
            set
            {
                _preferSize = value;
                RecalculateSize();
            }
        }

        private void RecalculateSize()
        {
            var size = GetActualSize();
            //set star sizes
            if (PreferSize == null)
                PreferSize = GetActualSize();

            if (ColStarWidths != null && ColStarWidths.Count > 0)
            {
                var starwidthprice = (PreferSize.Value.Width - size.Width) / ColStarWidths.Sum(c => c.Value);

                var newWidths = new Dictionary<int, double>();
                foreach (var colWidth in _colWidths.Keys)
                    if (ColStarWidths.ContainsKey(colWidth))
                        newWidths[colWidth] =
                            (_colWidths[colWidth] > starwidthprice * ColStarWidths[colWidth]
                                ? _colWidths[colWidth]
                                : starwidthprice * ColStarWidths[colWidth]);
                    else newWidths[colWidth] = _colWidths[colWidth];
                _colWidths = newWidths;
            }

            if (RowStarHeights != null && RowStarHeights.Count > 0)
            {
                var starheigthprice = (PreferSize.Value.Height - size.Height) / RowStarHeights.Sum(c => c.Value);

                var newHeights = new Dictionary<int, double>();
                foreach (var rowHeight in _rowHeights.Keys)
                    if (RowStarHeights.ContainsKey(rowHeight))
                        newHeights[rowHeight] =
                            (_rowHeights[rowHeight] > starheigthprice * RowStarHeights[rowHeight]
                                ? _rowHeights[rowHeight]
                                : starheigthprice * RowStarHeights[rowHeight]);
                    else newHeights[rowHeight] = _rowHeights[rowHeight];
                _rowHeights = newHeights;
            }
        }

        public override Size GetActualSize()
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
        private Size? _preferSize;

        void CalculateGrid()
        {
            _colWidths = new Dictionary<int, double>();
            _rowHeights = new Dictionary<int, double>();

            foreach (var child in Childs)
            {
                var gridElem = child as GridElem;
                if (gridElem != null)
                {
                    var elemSize = child.GetActualSize();

                    if (!_colWidths.ContainsKey(gridElem.Col)
                        || _colWidths[gridElem.Col] < elemSize.Width)
                        _colWidths[gridElem.Col] = elemSize.Width;

                    if (!_rowHeights.ContainsKey(gridElem.Row)
                        || _rowHeights[gridElem.Row] < elemSize.Height)
                        _rowHeights[gridElem.Row] = elemSize.Height;
                }
            }
        }

        public override void AddChild(IComponent child)
        {
            base.AddChild(child);
            _colWidths = null;
            _rowHeights = null;
        }
    }
}
