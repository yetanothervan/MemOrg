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
        
        public override List<DrawingVisual> Render(Point p)
        {
            var result = new List<DrawingVisual>();
            
            foreach (var child in Childs)
            {
                var gridElem = child as GridElem;
                if (gridElem == null) continue;

                var x = _colWidths.Where(o => o.Key < gridElem.Col).Sum(o => o.Value)
                           + (_colWidths.Count(o => o.Key < gridElem.Col) + 1) * Margin;

                var y = _rowHeights.Where(o => o.Key < gridElem.Row).Sum(o => o.Value)
                           + (_rowHeights.Count(o => o.Key < gridElem.Row) + 1) * Margin;

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
        
        void CalculateGrid()
        {
            _colWidths = new Dictionary<int, double>();
            _rowHeights = new Dictionary<int, double>();

            foreach (var child in Childs)
            {
                var gridElem = child as GridElem;
                if (gridElem != null)
                {
                    var elemSize = child.GetSize();

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
