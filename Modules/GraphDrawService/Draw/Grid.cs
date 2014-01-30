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
            CalculateGrid();
            throw new NotImplementedException();
        }

        public Size GetSize()
        {
            throw new NotImplementedException();
        }

        void CalculateGrid()
        {
            var colWidths = new Dictionary<int, double>();
            var rowHeights = new Dictionary<int, double>();
            foreach (var child in Childs)
            {
// ReSharper disable once SuspiciousTypeConversion.Global
                var gridElem = child as IGridElem;
                if (gridElem == null) continue;
                
                var elemSize = child.GetSize();
                if (colWidths.ContainsKey(gridElem.ColIndex)
                    && colWidths[gridElem.ColIndex] < elemSize.Width)
                    colWidths[gridElem.ColIndex] = elemSize.Width;
                if (rowHeights.ContainsKey(gridElem.RowIndex)
                    && rowHeights[gridElem.RowIndex] < elemSize.Height)
                    rowHeights[gridElem.RowIndex] = elemSize.Height;
            }
        }
    }
}
