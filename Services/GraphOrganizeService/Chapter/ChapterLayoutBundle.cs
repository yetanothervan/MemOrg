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
                if (upper.Direction == BundleDirection.EndRel)
                    ;
                else if (upper._parent._direction == BundleDirection.Root
                    || upper._parent._direction == BundleDirection.OuterRoot
                    || upper._parent._direction == BundleDirection.Upper)
                    upper._direction = BundleDirection.Upper;
                else
                    upper._direction = BundleDirection.OuterRoot;

                if (bundleOrderedByPower.Count > 1)
                {
                    var lower = bundleOrderedByPower[1];
                    if (lower.Direction == BundleDirection.EndRel)
                        ;
                    else if (lower._parent._direction == BundleDirection.Root
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

                var other = edge.GetOther(my);
                if (my.IsBlockRel && my.RelationFirst == other || my.RelationSecond == other)
                {
                    if (graph.GetChildCountForVertex(other) == 0
                        || graph.GetEdgesForVertex(other)
                            .All(e => !e.Equals(edge) && graph.GetChildCountForVertex(e.GetOther(other)) == 0))
                    {
                        var relEnd = ExtractBundlesByEdge(graph, edge, other, BundleDirection.EndRel);
                        relEnd._parent = result;
                        result._bundles.Add(relEnd);
                        continue;
                    }
                }
                
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


        struct OuterRoot
        {
            public ChapterLayoutBundle Body;
            public bool Direction;
        }

        private List<OuterRoot> _outerRoots;
        public List<ChapterLayoutElem> Render(int row, int col, bool right)
        {
            _outerRoots = new List<OuterRoot>();
            var elems = RenderRoot(0, 0, true);
            foreach (var outerRoot in _outerRoots)
            {
                int resHeight = elems.Max(e => e.Row) + 1;
                var outElems = outerRoot.Body.Render(0, outerRoot.Direction ? 2 : 0, !outerRoot.Direction);
                int outIce = 0 - outElems.Min(b => b.Row);
                foreach (var elem in outElems)
                    elem.Row += (resHeight + outIce);
                elems.AddRange(outElems);
            }
            return elems;
        }

        private List<ChapterLayoutElem> RenderRoot(int row, int col, bool right)
        {
            var result = new List<ChapterLayoutElem>();

            result.AddRange(RenderElem(row, col, _direction == BundleDirection.Upper, right));
            col = right ? col + 1 : col - 1;

            if (this._direction == BundleDirection.Root)
            {
                row = RenderUpperElems(row, col, right, result);
                row = RenderMiddleElems(row, col, result, false, right);
                RenderLowerElems(row, col, right, result);
            }

            if (this._direction == BundleDirection.Upper)
            {
                row = RenderMiddleElems(row, col, result, true, right);
                RenderUpperElems(row, col, right, result);
            }

            if (this._direction == BundleDirection.Lower)
            {
                row = RenderMiddleElems(row, col, result, false, right);
                RenderLowerElems(row, col, right, result);
            }

            return result;
        }

        private void RenderLowerElems(int row, int col, bool right, List<ChapterLayoutElem> result)
        {
            var lower = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Lower);
            if (lower != null)
            {
                var lowerElems = lower.RenderLower(row + 1, col, right).ToList();
                result.AddRange(lowerElems);
            }
        }

        private int RenderUpperElems(int row, int col, bool right, List<ChapterLayoutElem> result)
        {
            var upper = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Upper);
            if (upper != null)
            {
                var upperElems = upper.RenderUpper(row, col, right).ToList();
                result.AddRange(upperElems);
                row = GetLowerRow(upperElems, row);
            }
            return row;
        }

        private int RenderMiddleElems(int row, int col, List<ChapterLayoutElem> result, bool onesToUp, bool right)
        {
            foreach (var bundle in _bundles.Where(b => b.Direction == BundleDirection.Middle))
            {
                var midElems = bundle.RenderElem(onesToUp ? row - 1: row + 1, col, onesToUp, right).ToList();
                result.AddRange(midElems);

                row = onesToUp
                    ? GetUpperRow(midElems, row - 2)
                    : GetLowerRow(midElems, row + 2);
            }

            return row;
        }

        private IEnumerable<ChapterLayoutElem> RenderUpper(int row, int col, bool right)
        {
            var result = new List<ChapterLayoutElem>();

            if (this.MyElem.IsBlockRel)
                result.AddRange(RenderElem(row, col, true, right));
            else
            {
                var upperRootElem = RenderRoot(row - 1, right ? col + 1 : col - 1, !right);
                result.AddRange(upperRootElem);
                return result;
            }
            
            var upperRoot = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Upper);
            if (upperRoot != null)
            {
                var upperRootElem = upperRoot.RenderRoot(row - 1, right ? col + 1 : col - 1, !right);
                result.AddRange(upperRootElem);
            }
            
            return result;
        }

        private IEnumerable<ChapterLayoutElem> RenderLower(int row, int col, bool right)
        {
            var result = new List<ChapterLayoutElem>();

            if (this.MyElem.IsBlockRel)
                result.AddRange(RenderElem(row, col, false, right));
            else
            {
                var lowerRootElem = RenderRoot(GetLowerRow(result, row + 1),
                    right ? col + 1 : col - 1, !right);
                result.AddRange(lowerRootElem);
                return result;
            }
            
            var lowerRoot = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Lower);
            if (lowerRoot != null)
            {
                var lowerRootElem = lowerRoot.RenderRoot(GetLowerRow(result, row + 1), 
                    right ? col + 1 : col - 1, !right);
                result.AddRange(lowerRootElem);
            }

            return result;
        }
        
        private IEnumerable<ChapterLayoutElem> RenderOnes(int row, int col, bool up)
        {
            var result = new List<ChapterLayoutElem>();
            if (!_ones.Any()) return result;
            int onesIndex = 0;
            foreach (var pageEdge in _ones)
            {
                var other = pageEdge.GetOther(MyElem);
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
            if (row < rowUp.Row) return row;
            return rowUp.Row;
        }

        private IEnumerable<ChapterLayoutElem> RenderElem(int row, int col, bool up, bool right)
        {
            var result = new List<ChapterLayoutElem>();
            var rel = MakeElem(row, col);

            result.Add(rel);
            result.AddRange(RenderOnes(up ? row - 1 : row + 1, col, up));

            var endRel = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.EndRel);
            if (endRel != null)
                result.AddRange(endRel.RenderElem(row, right ? col + 1 : col - 1, up, right));

            foreach (var bundle in _bundles.Where(b => b.Direction == BundleDirection.OuterRoot))
                GetRoot()._outerRoots.Add(new OuterRoot {Body = bundle, Direction = right});

            return result;
        }

        ChapterLayoutBundle GetRoot()
        {
            if (Direction == BundleDirection.Root || this.Direction == BundleDirection.OuterRoot || _parent == null)
                return this;
            return _parent.GetRoot();
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