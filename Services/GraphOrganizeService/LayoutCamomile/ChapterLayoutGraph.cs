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

        private ChapterLayoutGraph()
        {
            _edges = new HashSet<PageEdge>();
            _vertex = new HashSet<IPage>();
        }

        public static bool CheckCyclic(ChapterLayoutGraph graph)
        {
            if (graph._vertex.Count == 0) return false;
            var vertexVisited = new HashSet<IPage>();
            var edgesVisited = new HashSet<PageEdge>();

            return CheckCyclicProceed(vertexVisited, edgesVisited, graph._vertex.First(), graph);
        }

        private static bool CheckCyclicProceed(HashSet<IPage> vertexVisited, HashSet<PageEdge> edgesVisited, IPage vertex, ChapterLayoutGraph graph)
        {
            if (vertexVisited.Contains(vertex)) return true;
            vertexVisited.Add(vertex);
            foreach (var edge in graph._edges.Where(e => (!edgesVisited.Contains(e) && (e.First == vertex || e.Second == vertex))))
            {
                var other = (edge.First == vertex) ? edge.Second : edge.First;
                edgesVisited.Add(edge);
                if (CheckCyclicProceed(vertexVisited, edgesVisited, other, graph)) return true;
            }
            return false;
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