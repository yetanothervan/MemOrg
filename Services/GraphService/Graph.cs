using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    public class Graph : IGraph
    {
        private readonly IGraphService _graphService;
        private IList<IBook> _books;

        public Graph(IGraphService graphService)
        {
            _graphService = graphService;
            _books = null;
        }

        public IGraphService GraphService
        {
            get { return _graphService; }
        }

        public IList<IBook> Books
        {
            get
            {
                if (_books == null)
                {
                    FillBooksWithChapters();
                    FillChaptersWithBlocks();
                }
                return _books; 
            }
        }

        private void FillBooksWithChapters()
        {
            _books = new List<IBook>();
            Book book = null;
            foreach (var source in
                _graphService.BlockSources
                .OrderBy(b => b.ParamName)
                .ThenBy(b => b.ParamValue))
            {
                if (book != null && book.Caption != source.ParamName)
                {
                    Books.Add(book);
                    book = new Book { CaptionInternal = source.ParamName };
                }
                if (book == null)
                    book = new Book { CaptionInternal = source.ParamName };
                book.ChaptersInternal.Add(new Chapter {ChapterBlock = source});
            }
            if (book != null)
                Books.Add(book);
        }

        private void FillChaptersWithBlocks()
        {
            if (_books == null || _books.Count == 0) return;

            foreach (var block in _graphService.BlockAll
                .Where(b => //block is not source or rel and have any quote particles
                    !_graphService.BlockRels.Contains(b) 
                    && !_graphService.BlockSources.Contains(b)
                    && b.Particles.Any(p => p is QuoteSourceParticle)
                    ))
            {
                foreach (var particle in block.Particles)
                {
                    var p = particle as QuoteSourceParticle;
                    if (p != null)
                    {
                        foreach (var book in _books)
                        {
                            var chapter =
                                book.Chapters.FirstOrDefault(c => c.ChapterBlock.BlockId == p.SourceTextParticle.Block.BlockId);
                            if (chapter != null && chapter.PagesBlocks.All(page => page.Block != block))
                            {
                                Tag tag = 
                                    _graphService.TagsBlock.FirstOrDefault(t => t.TagBlock.BlockId == block.BlockId);

                                chapter.PagesBlocks.Add(new Page
                                {
                                    Block = block, 
                                    IsBlockTag = tag != null,
                                    Tag = tag
                                });
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
