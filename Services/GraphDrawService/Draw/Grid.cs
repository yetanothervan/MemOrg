using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphDrawService.Draw
{
    public class Grid : Component
    {
        private const double Margin = 5.0;

        private readonly IDrawStyle _style;
        
        public Grid(IDrawStyle style)
        {
            _style = style;
        }
        
        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var rect = new Rect(p, GetSize());
                dc.DrawRectangle(_style.QuoteBlockBrush, _style.QuoteBlockPen, rect);
            }
            result.Add(dv);

            foreach (var child in Childs)
            {
                int colInd, rowInd;
                if (child is IGridLink)
                {
                    colInd = (child as IGridLink).Begin.Col;
                    rowInd = (child as IGridLink).Begin.Row;
                }
                else
                {
                    var gridElem = child as IGridElem;
                    if (gridElem == null) continue;
                    colInd = gridElem.ColIndex;
                    rowInd = gridElem.RowIndex;
                }


                var x = _colWidths.Where(o => o.Key < colInd).Sum(o => o.Value)
                           + (_colWidths.Count(o => o.Key < colInd) + 1)*Margin;
                
                var y = _rowHeights.Where(o => o.Key < rowInd).Sum(o => o.Value)
                           + (_rowHeights.Count(o => o.Key < rowInd) + 1) * Margin;

                var pC = new Point(x + p.X, y + p.Y);
                
                result.AddRange(child.Render(pC));
            }

            return result;
        }

        public override Size GetSize()
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
        private Dictionary<int, double> _colSpacesWidths;
        private Dictionary<int, double> _rowSpacesHeights;
        
        void CalculateGrid()
        {
            _colWidths = new Dictionary<int, double>();
            _rowHeights = new Dictionary<int, double>();
            _colSpacesWidths = new Dictionary<int, double>();
            _rowSpacesHeights = new Dictionary<int, double>();

            foreach (var child in Childs)
            {
                var gridElem = child as IGridElem;
                if (gridElem != null)
                {
                    var elemSize = child.GetSize();

                    if (!_colWidths.ContainsKey(gridElem.ColIndex)
                        || _colWidths[gridElem.ColIndex] < elemSize.Width)
                        _colWidths[gridElem.ColIndex] = elemSize.Width;

                    if (!_rowHeights.ContainsKey(gridElem.RowIndex)
                        || _rowHeights[gridElem.RowIndex] < elemSize.Height)
                        _rowHeights[gridElem.RowIndex] = elemSize.Height;
                    continue;
                }
                
                var gridLink = child as IGridLink;
                if (gridLink != null)
                {
                    var elemSize = child.GetSize();
                    var col = Math.Max(gridLink.Begin.Col, gridLink.End.Col);
                    var row = Math.Max(gridLink.Begin.Row, gridLink.End.Row);

                    if (!_colWidths.ContainsKey(col)
                        || _colWidths[col] < elemSize.Width)
                        _colWidths[col] = elemSize.Width;

                    if (!_rowHeights.ContainsKey(row)
                        || _rowHeights[row] < elemSize.Height)
                        _rowHeights[row] = elemSize.Height;
                }
            }
        }
    }
}
