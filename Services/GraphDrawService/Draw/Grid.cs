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
        
        public Grid(IDrawStyle style)
        {
            _style = style;
        }
        
        public override List<DrawingVisual> Render(Point p1, Point? p2)
        {
            var result = new List<DrawingVisual>();
            var size = GetActualSize();
            Point pS;
            if (p2 == null) pS = p1;
            else
            {
                switch (HorizontalAligment)
                {
                    case HorizontalAligment.Left:
                        pS = p1;
                        break;
                        case HorizontalAligment.Right:
                        pS = new Point(p2.Value.X - size.Width, p1.Y);
                        break;
                        case HorizontalAligment.Center:
                        pS = new Point((p2.Value.X - p1.X - size.Width) / 2 + p1.X, p1.Y);
                        break;
                        case HorizontalAligment.Stretch:
                    default:
                        throw new NotImplementedException();
                }
            }
            

            foreach (var child in Childs)
            {
                var gridElem = child as GridElem;
                if (gridElem == null) continue;

                var x1 = pS.X + _colWidths.Where(o => o.Key < gridElem.Col).Sum(o => o.Value)
                           + (_colWidths.Count(o => o.Key < gridElem.Col) + 1) * Margin;

                var y1 = pS.Y + _rowHeights.Where(o => o.Key < gridElem.Row).Sum(o => o.Value)
                           + (_rowHeights.Count(o => o.Key < gridElem.Row) + 1) * Margin;

                var x2 = x1 + _colWidths.First(c => c.Key == gridElem.Col).Value;

                var y2 = y1 + _rowHeights.First(c => c.Key == gridElem.Row).Value;

                result.AddRange(child.Render(
                    new Point(x1, y1),
                    new Point(x2, y2)
                    ));
            }

            return result;
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
    }
}
