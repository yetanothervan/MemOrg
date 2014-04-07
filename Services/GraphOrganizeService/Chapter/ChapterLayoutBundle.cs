using System.Collections.Generic;
using System.Linq;
using GraphOrganizeService.LayoutCamomile;
using MemOrg.Interfaces;

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
        
        public IPage MyElem { get { return _myElem; }}
        public IEnumerable<ChapterLayoutBundle> Bundles { get { return _bundles; }}
        public IEnumerable<PageEdge> Ones { get { return _ones; } }
        public BundleDirection Direction { get { return _direction; } }

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

        public IGrid Render()
        {
            return null;

            /* Отрендерить корень
             * Найти в корне верхний бандл
             * 
             * 
             * 
             * 
             */
        }
    }
}