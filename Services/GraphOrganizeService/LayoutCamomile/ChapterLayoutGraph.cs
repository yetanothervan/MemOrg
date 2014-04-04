using System;
using System.Collections.Generic;
using System.Linq;
using MemOrg.Interfaces;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutGraph
    {
        private readonly HashSet<PageEdge> _edges;
        private readonly HashSet<IPage> _vertex;
        private readonly HashSet<PageEdge> _removedEdge;


        private ChapterLayoutGraph()
        {
            _edges = new HashSet<PageEdge>();
            _vertex = new HashSet<IPage>();
            _removedEdge = new HashSet<PageEdge>();
        }

        public static void Uncycle(ChapterLayoutGraph graph)
        {
            if (graph._vertex.Count == 0) return;
            var cycPars = new CyclingParams(graph);

            IPage c;
            while ((c = UncycleProceed(cycPars, graph._vertex.First())) != null)
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

        public static ChapterLayoutGraph ExtractGraph(HashSet<IPage> fromArray)
        {
            var byPage = fromArray.FirstOrDefault(r => !r.IsBlockRel);
            if (byPage == null) throw new ArgumentException();

            var result = new ChapterLayoutGraph();
            Proceed(result, byPage);

            foreach (var vertex in result._vertex)
                fromArray.Remove(vertex);
            
            return result;
        }

        private static void Proceed(ChapterLayoutGraph result, IPage byPage)
        {
            result._vertex.Add(byPage);

            foreach (var rel in byPage.RelatedBy)
            {
                result._vertex.Add(rel);
                var edge = new PageEdge(byPage, rel);
                if (result._edges.Contains(edge)) continue;
                
                result._edges.Add(edge);
                Proceed(result, rel);

                var other = (rel.RelationFirst == byPage) ? rel.RelationSecond : rel.RelationFirst;

                var edge2 = new PageEdge(rel, other);
                if (result._edges.Contains(edge2)) continue;

                result._edges.Add(edge2);
                Proceed(result, other);
            }

            foreach (var refer in byPage.ReferencedBy)
            {
                result._vertex.Add(refer);
                var edge = new PageEdge(byPage, refer);
                if (result._edges.Contains(edge)) continue;

                result._edges.Add(edge);
                Proceed(result, refer);
            }
        }
    }
}