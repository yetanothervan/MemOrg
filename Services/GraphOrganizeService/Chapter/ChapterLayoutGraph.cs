using System;
using System.Collections.Generic;
using System.Linq;
using GraphOrganizeService.LayoutCamomile;
using MemOrg.Interfaces;

namespace GraphOrganizeService.Chapter
{
    public class ChapterLayoutGraph
    {
        private readonly HashSet<PageEdge> _edges;
        private readonly HashSet<IPage> _vertexes;
        private readonly HashSet<PageEdge> _removedEdge;

        private ChapterLayoutGraph()
        {
            _edges = new HashSet<PageEdge>();
            _vertexes = new HashSet<IPage>();
            _removedEdge = new HashSet<PageEdge>();
        }

        public IEnumerable<PageEdge> GetEdgesForVertex(IPage vertex)
        {
            return _edges.Where(e => e.First == vertex || e.Second == vertex);
        }

        public IPage GetMostLargestNumberOfEdgeVertex()
        {
            var res =
                _vertexes.SelectMany(v => _edges, (v, e) => new {v, e})
                    .Where(@t => @t.e.First == @t.v || @t.e.Second == @t.v).GroupBy(g => g.v)
                    .Select(group => new {group.Key, Count = group.Count()})
                    .OrderByDescending(o => o.Count).First(v => !v.Key.IsBlockRel);

            return res.Key;
        }

        private static void Uncycle(ChapterLayoutGraph graph)
        {
            if (graph._vertexes.Count == 0) return;
            var cycPars = new CyclingParams(graph);

            IPage c;
            while ((c = UncycleProceed(cycPars, graph._vertexes.First())) != null)
            {
                bool refDeleted = false;
                //let's delete reference (if we find it)
                foreach (var edge in cycPars.Path)
                {
                    if (!edge.Equals(cycPars.Path.First()) && (edge.First == c || edge.Second == c))
                        break;

                    if (!edge.First.RelatedBy.Contains(edge.Second))
                    {
                        graph._removedEdge.Add(edge);
                        graph._edges.Remove(edge);
                        refDeleted = true;
                        break;
                    }
                }

                if (!refDeleted) //delete first
                {
                    var first = cycPars.Path.Pop();
                    graph._removedEdge.Add(first);
                    graph._edges.Remove(first);
                }

                cycPars = new CyclingParams(graph);
            }
        }

        struct CyclingParams
        {
            public CyclingParams(ChapterLayoutGraph graph)
            {
                Graph = graph;
                VertexVisited = new HashSet<IPage>();
                EdgesVisited = new HashSet<PageEdge>();
                Path = new Stack<PageEdge>();
            }

            public readonly HashSet<IPage> VertexVisited;
            public readonly HashSet<PageEdge> EdgesVisited;
            public readonly ChapterLayoutGraph Graph;
            public readonly Stack<PageEdge> Path;
        }


        private static IPage UncycleProceed(CyclingParams cycPars, IPage vertex)
        {
            if (cycPars.VertexVisited.Contains(vertex)) return vertex;
            cycPars.VertexVisited.Add(vertex);

            foreach (var edge in cycPars.Graph._edges
                .Where(e => (!cycPars.EdgesVisited.Contains(e) && (e.First == vertex || e.Second == vertex))))
            {
                var other = (edge.First == vertex) ? edge.Second : edge.First;
                cycPars.EdgesVisited.Add(edge);
                cycPars.Path.Push(edge);
                var c = UncycleProceed(cycPars, other);
                if (c != null) return c;
                cycPars.Path.Pop();
            }
            return null;
        }

        private static ChapterLayoutGraph ExtractGraph(HashSet<IPage> fromArray)
        {
            var byPage = fromArray.FirstOrDefault();
            if (byPage == null) throw new ArgumentNullException();

            var result = new ChapterLayoutGraph();
            Proceed(result, byPage);

            foreach (var vertex in result._vertexes)
                fromArray.Remove(vertex);
            
            return result;
        }

        private static void Proceed(ChapterLayoutGraph result, IPage byPage)
        {
            result._vertexes.Add(byPage);

            if (byPage.IsBlockRel)
            {
                var edgeFirst = new PageEdge(byPage, byPage.RelationFirst);
                if (!result._edges.Contains(edgeFirst))
                {
                    result._edges.Add(edgeFirst);
                    Proceed(result, byPage.RelationFirst);
                }
                
                var edgeSecond = new PageEdge(byPage, byPage.RelationSecond);
                if (!result._edges.Contains(edgeSecond))
                {
                    result._edges.Add(edgeSecond);
                    Proceed(result, byPage.RelationSecond);
                }
            }

            foreach (var rel in byPage.RelatedBy)
            {
                var edge = new PageEdge(byPage, rel);
                if (result._edges.Contains(edge)) continue;

                result._edges.Add(edge);
                Proceed(result, rel);
            }

            foreach (var refer in byPage.ReferencedBy)
            {
                result._vertexes.Add(refer);
                var edge = new PageEdge(byPage, refer);
                if (result._edges.Contains(edge)) continue;

                result._edges.Add(edge);
                Proceed(result, refer);
            }
        }

        public static IEnumerable<ChapterLayoutGraph> GetGraphsFromChapter(IChapter chapter)
        {
            var result = new List<ChapterLayoutGraph>();
            var pagesSet = new HashSet<IPage>(chapter.PagesBlocks);
            //desert out pagesSet
            while (pagesSet.Count > 0)
            {
                var graph = ExtractGraph(pagesSet);
                Uncycle(graph);
                result.Add(graph);
            }
            return result;
        }
    }
}