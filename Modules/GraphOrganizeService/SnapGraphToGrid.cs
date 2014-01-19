using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace GraphOrganizeService
{
    /// <summary>
    /// Идея класса состоит в следующем:
    /// Дан граф с помеченными блоками. Теперь его необходимо привязать к сетке таким образом.
    /// Все SeveralQuoteBlock и TagBlock образуют квадрат.
    /// По сторонам этого квадрата располагаются SourceBlock.
    /// SingleQuoteBlock и ReferenceBlock распологаются вовне.
    /// 
    /// Дальше идея должна эволюционировать в направлении к множеству таких квадратов.
    /// </summary>
    public static class SnapGraphToGrid
    {
        public static Grid SnapGraph(VisualGraph ngGraph)
        {
            var ringLayout = new GraphRingLayout(ngGraph);
            
            return new Grid();
        }
    }

    public class GridRingCell
    {
        public VisualGraphElem Elem;
    }

    public class GraphRingLayout
    {
        private readonly VisualGraph _graph;
        private readonly int _squareSide;
        private readonly int _coilCount;
        private readonly List<List<GridRingCell>> _coils;
        private readonly List<GridRingCell> _inner;
        private readonly List<GridRingCell> _tmpUpper;
        private int _currentCoil = 0;
        private int _currentCoilCell = 0;
        
        public GraphRingLayout(VisualGraph visualGraph)
        {
            _graph = visualGraph;
            int innerItemsCount = _graph.Stats.SeveralSourcesQuoteBlockCount + _graph.Stats.TagBlockCount;

            _squareSide = CalculateSquareSideLength(innerItemsCount);
            _coilCount = CalculateCoilCount(_squareSide, _graph.Stats.SourceBlockCount);

            //initialize coils
            _coils = new List<List<GridRingCell>>(_coilCount);
            for (int i = 0; i < _coilCount; ++i)
                _coils[i] = new List<GridRingCell>(CoilCapacity(i, _squareSide));

            _inner = new List<GridRingCell>();
            _tmpUpper = new List<GridRingCell>();
        }

        public VisualGraph DoLayout()
        {
            var blocks = _graph.GetGraphElems();
            foreach (var block in blocks)
                PlaceBlock(block as VisualGraphElem);
            return _graph;
        }

        private void PlaceBlock(VisualGraphElem block)
        {
            switch (block.BlockType)
            {
                case VisualGraphBlockType.SourceBlock:
                {
                    if (_currentCoilCell == _coils[_currentCoil].Count)
                    {
                        ++_currentCoil;
                        _currentCoilCell = 0;
                    }
                    _coils[_currentCoil][_currentCoilCell++] = new GridRingCell() {Elem = block};
                }
                    break;
                case VisualGraphBlockType.OneSourceQuoteBlock:
                    _tmpUpper.Add(new GridRingCell() {Elem = block});
                    break;
                case VisualGraphBlockType.ReferenceBlock:
                    _tmpUpper.Add(new GridRingCell() {Elem = block});
                    break;
                case VisualGraphBlockType.SeveralSourcesQuoteBlock:
                    _inner.Add(new GridRingCell() {Elem = block});
                    break;
                default:
                    throw new NotImplementedException();
            }
            
        }

        private static int CalculateSquareSideLength(int itemsCount)
        {
            var res = 0;
            while (itemsCount >= res * res)
                ++res;
            return res;
        }

        private static int CalculateCoilCount(int squareSide, int edgeItemsCount)
        {
            var coilCount = 1;
            var sumCoilLengths = CoilCapacity(coilCount, squareSide);
            while (edgeItemsCount > sumCoilLengths)
            {
                ++coilCount;
                sumCoilLengths += CoilCapacity(coilCount, squareSide);
            }
            return coilCount;
        }

        private static int CoilCapacity(int coil, int squareSide)
        {
            return (squareSide + 2 * coil) * 4 + 4;
        }
    }
}
