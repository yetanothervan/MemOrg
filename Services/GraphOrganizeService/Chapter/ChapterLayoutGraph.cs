using System;
using System.Collections.Generic;
using System.Linq;
using MemOrg.Interfaces;

namespace GraphOrganizeService.Chapter
{
    public class ChapterLayoutGraph
    {
        private readonly HashSet<PageEdge> _edges;
        private readonly HashSet<IPage> _vertexes;
        private readonly HashSet<PageEdge> _removedEdge;
        public readonly IChapter MyChapter;

        public IEnumerable<PageEdge> GetEdges()
        {
            return _edges;
        }

        public IEnumerable<PageEdge> GetRemovedEdges()
        {
            return _removedEdge;
        }

        private ChapterLayoutGraph(IChapter myChapter)
        {
            MyChapter = myChapter;
            _edges = new HashSet<PageEdge>();
            _vertexes = new HashSet<IPage>();
            _removedEdge = new HashSet<PageEdge>();
        }

        private Dictionary<IPage, int> _vertexChildCount;

        private void CalculateVertexChildCount()
        {
            _vertexChildCount = new Dictionary<IPage, int>();
            var root = GetMostLargestNumberOfEdgeVertex();
            CalculateVertexChildCount(root, null);

        }

        private void CalculateVertexChildCount(IPage vertex, PageEdge parentEdge)
        {
            var childs = GetEdgesForVertex(vertex).Where(e => !e.Equals(parentEdge)).ToList();
            _vertexChildCount[vertex] = childs.Count();
            foreach (var pageEdge in childs)
                CalculateVertexChildCount(pageEdge.GetOther(vertex), pageEdge);
        }

        public int GetChildCountForVertex(IPage vertex)
        {
            if (_vertexChildCount == null)
                throw new ArgumentException("Graph is non uncycled yet");
            return _vertexChildCount[vertex];
        }

        public IEnumerable<PageEdge> GetEdgesForVertex(IPage vertex)
        {
            return _edges.Where(e => e.First == vertex || e.Second == vertex);
        }

        public IPage GetMostLargestNumberOfEdgeVertex()
        {
            if (_vertexes.Count == 0) return null;
            if (_edges.Count == 0)
                return _vertexes.First();
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

                    if (!edge.First.LinksBy
                        .Any(l => l.LinkType == PageLinkType.ToRelationBlock && l.OppPage == edge.Second))
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
            graph.CalculateVertexChildCount();
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
                var other = edge.GetOther(vertex);
                cycPars.EdgesVisited.Add(edge);
                cycPars.Path.Push(edge);
                var c = UncycleProceed(cycPars, other);
                if (c != null) return c;
                cycPars.Path.Pop();
            }
            return null;
        }

        private static IEnumerable<ChapterLayoutGraph> ExtractGraph(IChapter chapter,
            HashSet<IPage> pagesInBook)
        {
            var pagesSet = new HashSet<IPage>(chapter.PagesBlocks);
            var result = new List<ChapterLayoutGraph>();

            //desert out pagesSet
            while (pagesSet.Count > 0)
            {
                var byPage = pagesSet.FirstOrDefault();
                if (byPage == null) throw new ArgumentNullException();

                var graphRes = new ChapterLayoutGraph(chapter);
                Proceed(graphRes, pagesSet, byPage, null, pagesInBook);
                
                Uncycle(graphRes);
                result.Add(graphRes);
            }
            
            return result;
        }


        /*
         * �������: 
         * ���� �������� ������ - ���������
         * ���� �������� ������ � ������ ����� - ���������
         * ���� �������� ��� � � ��������: ���� ��� ��� ���������, �� ����� ��������� � removed
         *  ����� ���������
         * ���� �������� ��� � ���-�� � �����: ���� ��� ��� ���������, �� ����� ��������� � removed
         *  ����� ���������
         * ���� �������� � ������ ����� - ���������, �� �� ������ �� ����         
         */

        private static void Proceed(ChapterLayoutGraph result, HashSet<IPage> remainingPages, 
            IPage addingPage, PageEdge addingEdge, HashSet<IPage> pagesInBook)
        {
            if (result._edges.Contains(addingEdge) || result._removedEdge.Contains(addingEdge)) return;

            if (addingPage.MyChapter != null && addingPage.MyChapter != result.MyChapter)
            {
                result._removedEdge.Add(addingEdge);
                return;
            }

            if (addingPage.MySources == BlockQuoteParticleSources.NeightborChapter ||
                addingPage.MySources == BlockQuoteParticleSources.MyBook)
            {
                if (pagesInBook.Contains(addingPage))
                {
                    if (addingEdge != null)
                        result._removedEdge.Add(addingEdge);
                    remainingPages.Remove(addingPage);
                    return;
                }
                pagesInBook.Add(addingPage);
                result._vertexes.Add(addingPage);
                remainingPages.Remove(addingPage);
            }

            if (addingEdge != null)
                result._edges.Add(addingEdge);

            if (addingPage.MySources == BlockQuoteParticleSources.MyChapterOnly
                || addingPage.MySources == BlockQuoteParticleSources.NoSources
                || addingPage.MySources == BlockQuoteParticleSources.OtherBook)
            {
                result._vertexes.Add(addingPage);
                remainingPages.Remove(addingPage);
                if (addingPage.MySources == BlockQuoteParticleSources.OtherBook) return;
            }

            if (addingPage.IsBlockRel)
            {
                var edgeFirst = new PageEdge(addingPage, addingPage.RelationFirst,
                    new PageLink {LinkType = PageLinkType.ToRelationBlock, OppPage = addingPage.RelationFirst});
                Proceed(result, remainingPages, addingPage.RelationFirst, edgeFirst, pagesInBook);
                
                var edgeSecond = new PageEdge(addingPage, addingPage.RelationSecond,
                    new PageLink { LinkType = PageLinkType.ToRelationBlock, OppPage = addingPage.RelationSecond });
                Proceed(result, remainingPages, addingPage.RelationSecond, edgeSecond, pagesInBook);
            }

            foreach (var rel in addingPage.LinksBy)
            {
                var edge = new PageEdge(addingPage, rel.OppPage, rel);
                Proceed(result, remainingPages, rel.OppPage, edge, pagesInBook);
            }
        }
        
        public static IEnumerable<ChapterLayoutGraph> GetGraphsFromBook(IBook book)
        {
            var result = new List<ChapterLayoutGraph>();

            var pagesInBook = new HashSet<IPage>();

            foreach (var chapter in book.Chapters)
                result.AddRange(ExtractGraph(chapter, pagesInBook));
            
            return result;
        }
    }
}