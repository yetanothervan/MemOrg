using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<ChapterLayoutElem> Render()
        {
            var elems = GetElemsForBundle();
            return elems;
        }

        private List<ChapterLayoutElem> GetElemsForBundle()
        {
            /* Отрендерить корень
             * Найти в корне верхний бандл
             * Он может быть:
             *   бендл с релблоком
             *     рендер релблока, затем конечный блок
             *   бендл без рел блока
             *     рендер стрелки с названием рела, затем конечный блок
             *       рендер бандлов аппера
             *          рендер мидлов 
             *             если миддл релблок с релом 
             *               - рел вверх-влево, конечный блок - влево
             *             если миддл без релблока, тоже, но вместо релблока 
             *               - стрелка с названием рела
             *             аутеры - пропустить.
             *          рендер аппера
             *             аналогично в другую стророну.
             *      рендер миддлов рута
             *      рендер лоуэра
             */

            var result = new List<ChapterLayoutElem>();
            
            var root = new ChapterLayoutElem()
            {
                Col = 0,
                Row = 0,
                HorizontalAligment = HorizontalAligment.Right,
                Page = MyElem
            };
            result.Add(root);

            var upper = Bundles.FirstOrDefault(b => b.Direction == BundleDirection.Upper);
            
            if (upper != null)
            {
                root.AddCon(NESW.East);
                result.AddRange(upper.RenderUpper(0, 1));
            }

            return result;
        }

        private IEnumerable<ChapterLayoutElem> RenderUpper(int row, int col)
        {
            var result = new List<ChapterLayoutElem>();
            
            if (MyElem.IsBlockRel)
            {
                var midRel = RenderMidRel(row, col);
                result.AddRange(midRel);
                
                var other = MyElem.RelationFirst == _parent.MyElem
                    ? MyElem.RelationSecond
                    : MyElem.RelationFirst;
                var endRel = Bundles.FirstOrDefault(b => b.MyElem == other);

                if (endRel != null || Ones.Contains(new PageEdge(_parent.MyElem, other)))
                {
                    var end = new ChapterLayoutElem
                    {
                        Col = col + 1,
                        Row = row,
                        HorizontalAligment = HorizontalAligment.Left,
                        ConnectionPoints = new List<NESW> { NESW.West },
                        Page = other
                    };
                    result.Add(end);
                }
            }
            else
            {
                var arrow = new ChapterLayoutElem
                {
                    Row = row,
                    Col = col
                };
                arrow.AddGridLink(new GridLinkPart
                {
                    Direction = GridLinkPartDirection.WestEast,
                    Type = GridLinkPartType.Relation
                });
                result.Add(arrow);
                var end = new ChapterLayoutElem
                {
                    Col = col + 1,
                    Row = row,
                    HorizontalAligment = HorizontalAligment.Left,
                    ConnectionPoints = new List<NESW> { NESW.West },
                    Page = MyElem
                };
                result.Add(end);
            }
            return result;
        }

        private IEnumerable<ChapterLayoutElem> RenderMidRel(int row, int col)
        {
            var result = new List<ChapterLayoutElem>();

            var cps = _ones.Any()
                ? new List<NESW> {NESW.West, NESW.East, NESW.South}
                : new List<NESW> {NESW.West, NESW.East};
            
            var rel = new ChapterLayoutElem
            {
                Col = _ones.Any() ? col + 1 : col,
                Row = row,
                HorizontalAligment = HorizontalAligment.Center,
                ConnectionPoints = cps,
                Page = MyElem
            };
            result.Add(rel);

            if (!_ones.Any()) return result;

            var left = new ChapterLayoutElem
            {
                HorizontalAligment = HorizontalAligment.Center,
                Col = col,
                Row = row 
            };
            left.AddGridLink(new GridLinkPart() { Direction = GridLinkPartDirection.WestEast });
            result.Add(left);

            var down = new ChapterLayoutElem
            {
                HorizontalAligment = HorizontalAligment.Center,
                Col = col + 1,
                Row = row + 1
            };
            down.AddGridLink(new GridLinkPart() {Direction = GridLinkPartDirection.NorthWest});
            result.Add(down);

            var seconddown = new ChapterLayoutElem
            {
                HorizontalAligment = HorizontalAligment.Center,
                Col = col,
                Row = row + 1
            };
            seconddown.AddGridLink(new GridLinkPart() {Direction = GridLinkPartDirection.SouthEast});
            result.Add(seconddown);

            int onesIndex = 2;
            foreach (var pageEdge in _ones)
            {
                var other = pageEdge.First == MyElem ? pageEdge.Second : pageEdge.First;
                var otherElem = new ChapterLayoutElem
                {
                    Col = col + 1,
                    Row = row + onesIndex,
                    HorizontalAligment = HorizontalAligment.Center,
                    ConnectionPoints = new List<NESW> {NESW.West},
                    Page = other
                };
                result.Add(otherElem);

                var leftArrow = new ChapterLayoutElem
                {
                    HorizontalAligment = HorizontalAligment.Center,
                    Col = col,
                    Row = row + onesIndex
                };
                leftArrow.AddGridLink(new GridLinkPart() {Direction = GridLinkPartDirection.NorthEast});

                if (!Equals(pageEdge, _ones.Last()))
                    leftArrow.AddGridLink(new GridLinkPart() {Direction = GridLinkPartDirection.NorthSouth});
                result.Add(leftArrow);

                ++onesIndex;
            }
            return result;
        }
    }
}