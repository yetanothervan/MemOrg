using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.Chapter
{
    public class ChapterLayoutBundle
    {
        private readonly List<ChapterLayoutBundle> _bundles;
        private readonly List<PageEdge> _ones;
        private IPage _myElem;
        private ChapterLayoutBundle _parent;
        private BundleDirection _direction;
        public IChapter MyChapter;

        public IPage MyElem
        {
            get { return _myElem; }
        }

        public IEnumerable<ChapterLayoutBundle> Bundles
        {
            get { return _bundles; }
        }

        public IEnumerable<PageEdge> Ones
        {
            get { return _ones; }
        }

        public BundleDirection Direction
        {
            get { return _direction; }
        }

        private ChapterLayoutBundle()
        {
            _direction = BundleDirection.Root;
            _myElem = null;
            _ones = new List<PageEdge>();
            _bundles = new List<ChapterLayoutBundle>();
            _parent = null;
        }

        public static ChapterLayoutBundle ExtractBundlesFromGraph(ChapterLayoutGraph graph)
        {
            //let's find vertex with the most count of edges
            var root = graph.GetMostLargestNumberOfEdgeVertex();
            var result = ExtractBundlesByEdge(graph, null, root, BundleDirection.Root);
            result.MyChapter = graph.MyChapter;

            //mark up direction
            MarkUpDirections(result);
            return result;
        }

        private static void MarkUpDirections(ChapterLayoutBundle bundle)
        {
            var bundleOrderedByPower
                = bundle._bundles.OrderByDescending(b => b.GetBundlePower()).ToList();

            if (bundleOrderedByPower.Count == 0) return;

            if (bundleOrderedByPower.Count > 0)
            {
                var upper = bundleOrderedByPower[0];
                if (upper._parent._direction == BundleDirection.Root
                    || upper._parent._direction == BundleDirection.OuterRoot
                    || upper._parent._direction == BundleDirection.Upper)
                    upper._direction = BundleDirection.Upper;
                else
                    upper._direction = BundleDirection.OuterRoot;

                if (bundleOrderedByPower.Count > 1)
                {
                    var lower = bundleOrderedByPower[1];
                    if (lower._parent._direction == BundleDirection.Root
                        || lower._parent._direction == BundleDirection.OuterRoot
                        || lower._parent._direction == BundleDirection.Lower)
                        lower._direction = BundleDirection.Lower;
                    else if (lower._parent._direction == BundleDirection.Upper)
                        lower._direction = BundleDirection.Middle;
                    else
                        lower._direction = BundleDirection.OuterRoot;
                }
            }
            foreach (var ordBundle in bundleOrderedByPower)
                MarkUpDirections(ordBundle);
        }

        private static ChapterLayoutBundle ExtractBundlesByEdge(ChapterLayoutGraph graph,
            PageEdge myEdge, IPage my, BundleDirection direction)
        {
            var result = new ChapterLayoutBundle
            {
                _myElem = my,
                _direction = direction
            };
            foreach (var edge in graph.GetEdgesForVertex(my))
            {
                if (Equals(edge, myEdge)) continue;

                var other = edge.First == my ? edge.Second : edge.First;
                if (graph.GetEdgesForVertex(other).All(e => Equals(e, edge)))
                {
                    result._ones.Add(edge);
                    continue;
                }
                var bundle = ExtractBundlesByEdge(graph, edge, other, BundleDirection.Middle);
                bundle._parent = result;
                result._bundles.Add(bundle);
            }
            return result;
        }

        private int GetBundlePower()
        {
            return _bundles.Count + _bundles.Sum(bundle => bundle.GetBundlePower());
        }

        public List<ChapterLayoutElem> Render()
        {
            var elems = RenderRoot(0, 0, true);
            return elems;
        }

        private List<ChapterLayoutElem> RenderRoot(int row, int col, bool right)
        {
            var result = new List<ChapterLayoutElem>();
            result.AddRange(RenderElem(row, col, false));

            col = right ? col + 1 : col - 1;

            if (this._direction == BundleDirection.Root)
            {
                var upper = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Upper);
                if (upper != null)
                {
                    var upperElems = upper.RenderUpper(row, col, right).ToList();
                    result.AddRange(upperElems);
                    row = GetLowerRow(upperElems, row);
                }

                foreach (var bundle in _bundles.Where(b => b.Direction == BundleDirection.Middle))
                {
                    var midElems = bundle.RenderElem(row + 1, col, false).ToList();
                    result.AddRange(midElems);
                    row = GetLowerRow(midElems, row + 2);
                }

                var lower = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Lower);
                if (lower != null)
                {
                    var lowerElems = lower.RenderLower(row + 1, col, right).ToList();
                    result.AddRange(lowerElems);
                }
            }

            if (this._direction == BundleDirection.Upper)
            {
                foreach (var bundle in _bundles.Where(b => b.Direction == BundleDirection.Middle))
                {
                    var midElems = bundle.RenderElem(row - 1, col, true).ToList();
                    result.AddRange(midElems);
                    row = GetUpperRow(midElems, row - 2);
                }

                var upper = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Upper);
                if (upper != null)
                {
                    var upperElems = upper.RenderUpper(row, col, right).ToList();
                    result.AddRange(upperElems);
                    row = GetLowerRow(upperElems, row);
                }
            }

            if (this._direction == BundleDirection.Lower)
            {
                foreach (var bundle in _bundles.Where(b => b.Direction == BundleDirection.Middle))
                {
                    var midElems = bundle.RenderElem(row + 1, col, true).ToList();
                    result.AddRange(midElems);
                    row = GetLowerRow(midElems, row + 2);
                }

                var lower = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Lower);
                if (lower != null)
                {
                    var lowerElems = lower.RenderUpper(row, col, right).ToList();
                    result.AddRange(lowerElems);
                }
            }

            return result;
        }

        private IEnumerable<ChapterLayoutElem> RenderUpper(int row, int col, bool right)
        {
            var result = new List<ChapterLayoutElem>();

            result.AddRange(RenderElem(row, col, false));
            
            var upperRoot = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Upper);
            if (upperRoot != null)
            {
                var upperRootElem = upperRoot.RenderRoot(row - 1, right ? col + 1 : col - 1, !right);
                result.AddRange(upperRootElem);
            }

            //TODO outer
            return result;
        }

        private IEnumerable<ChapterLayoutElem> RenderLower(int row, int col, bool right)
        {
            var result = new List<ChapterLayoutElem>();
            var lems = RenderElem(row, col, false);
            result.AddRange(lems);
            
            var lowerRoot = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Lower);
            if (lowerRoot != null)
            {
                var lowerRootElem = lowerRoot.RenderRoot(GetLowerRow(result, row + 1), 
                    right ? col + 1 : col - 1, !right);
                result.AddRange(lowerRootElem);
            }

            //TODO outer
            return result;
        }
        
        private IEnumerable<ChapterLayoutElem> RenderOnes(int row, int col, bool up)
        {
            var result = new List<ChapterLayoutElem>();
            if (!_ones.Any()) return result;
            int onesIndex = 0;
            foreach (var pageEdge in _ones)
            {
                var other = pageEdge.First == MyElem ? pageEdge.Second : pageEdge.First;
                var otherElem = other.MakeElem(row + onesIndex, col);
                result.Add(otherElem);
                onesIndex = up ? onesIndex - 1 : onesIndex + 1;
            }
            return result;
        }

        private int GetLowerRow(IEnumerable<ChapterLayoutElem> elems, int row)
        {
            var rowLow = elems.OrderByDescending(e => e.Row).FirstOrDefault();
            if (rowLow == null) return row;
            if (row < rowLow.Row) return row;
            return rowLow.Row;
        }

        private int GetUpperRow(IEnumerable<ChapterLayoutElem> elems, int row)
        {
            var rowUp = elems.OrderBy(e => e.Row).FirstOrDefault();
            if (rowUp == null) return row;
            if (row > rowUp.Row) return row;
            return rowUp.Row;
        }

        private IEnumerable<ChapterLayoutElem> RenderElem(int row, int col, bool up)
        {
            var result = new List<ChapterLayoutElem>();
            var rel = MakeElem(row, col);
            result.Add(rel);
            result.AddRange(RenderOnes(up ? row - 1 : row + 1, col, up));
            return result;
        }

        private ChapterLayoutElem MakeElem(int row, int col)
        {
            var rel = new ChapterLayoutElem
            {
                Col = col,
                Row = row,
                Page = MyElem
            };
            return rel;
        }
    }

    public static class PageExtension
    {
        public static ChapterLayoutElem MakeElem(this IPage page, int row, int col)
        {
            var rel = new ChapterLayoutElem
            {
                Col = col,
                Row = row,
                Page = page
            };
            return rel;
        }
    }
}