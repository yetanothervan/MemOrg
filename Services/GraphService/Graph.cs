using System;
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
            get { return _books ?? (_books = GetBooks()); }
        }

        private IList<IBook> GetBooks()
        {
            var books = Enumerable.Cast<IBook>(
                    _graphService.BlockSources.GroupBy(o => o.ParamName)
                        .Select(@g => new Book {CaptionInternal = @g.Key}))
                        .ToList();

            //feel with chapters
            foreach (var book in books.Cast<Book>())
                book.ChaptersInternal =
                    Enumerable.Cast<IChapter>(
                        _graphService.BlockSources
                            .Where(b => b.ParamName == book.CaptionInternal)
                            .OrderBy(b => b.ParamName)
                            .Select(c => new Chapter { ChapterBlock = c })).ToList();

            //enumerate chapters
            foreach (var book in books)
                for (int i = 0; i < book.Chapters.Count; ++i)
                {
                    if (i != 0)
                        book.Chapters[i].PrevChapter = book.Chapters[i - 1];
                    if (i != book.Chapters.Count - 1)
                        book.Chapters[i].NextChapter = book.Chapters[i + 1];
                }

            //feel chapters with pages
            foreach (var book in books.Cast<Book>())
                foreach (var chapter in book.Chapters.Cast<Chapter>())
                {
                    var pageBlocks = _graphService.BlockAll
                        .Where(block => block.Particles
                            .Any(p => p is QuoteSourceParticle
                                      && (p as QuoteSourceParticle).SourceTextParticle.Block.BlockId
                                      == chapter.ChapterBlock.BlockId)).ToList();

                    foreach (var pageBlock in pageBlocks)
                    {
                        var page = new Page
                        {
                            Block = pageBlock,
                            Tag = _graphService.TagsBlock
                                .FirstOrDefault(t => t.TagBlock.BlockId == pageBlock.BlockId),
                            Relation = _graphService.RelationsBlock
                                .FirstOrDefault(r => r.RelationBlock.BlockId == pageBlock.BlockId),
                            MySources = DeterminePageQuatasSources(pageBlock, chapter, book),
                        };

                        if (page.IsBlockRel)
                        {
                            
                            page.RelFirstSources = DeterminePageQuatasSources(page.Relation.FirstBlock, chapter, book);
                            if (page.RelFirstSources == BlockQuoteParticleSources.NoSources
                                && chapter.PagesBlocks.All(b => b.Block.BlockId != page.Relation.FirstBlock.BlockId))
                            {
                                var firstPage = new Page
                                {
                                    Block = page.Relation.FirstBlock,
                                    MySources = BlockQuoteParticleSources.NoSources
                                };
                                chapter.PagesBlocks.Add(firstPage);
                                page.RelFirstSources = BlockQuoteParticleSources.MyChapterOnly;
                            }
                            
                            page.RelSecondSources = DeterminePageQuatasSources(page.Relation.SecondBlock, chapter, book);
                            if (page.RelSecondSources == BlockQuoteParticleSources.NoSources
                                    && chapter.PagesBlocks.All(b => b.Block.BlockId != page.Relation.SecondBlock.BlockId))
                            {
                                var secondPage = new Page
                                {
                                    Block = page.Relation.SecondBlock,
                                    MySources = BlockQuoteParticleSources.NoSources
                                };
                                chapter.PagesBlocks.Add(secondPage);
                                page.RelSecondSources = BlockQuoteParticleSources.MyChapterOnly;
                            }
                        }

                        chapter.PagesBlocks.Add(page);
                    }
                }

            return books;
        }

        private BlockQuoteParticleSources DeterminePageQuatasSources(Block pageBlock, Chapter chapter, Book book)
        {
            if (pageBlock.Particles.Count == 0) return BlockQuoteParticleSources.NoSources;
            
            var mySources = BlockQuoteParticleSources.MyChapterOnly;
            
            foreach (var qp in pageBlock.Particles
                .Select(particle => particle as QuoteSourceParticle)
                .Where(qp => qp != null))
            {
                if (IsQuoteInMyChapter(qp, chapter)) continue;

                if (mySources == BlockQuoteParticleSources.MyChapterOnly &&
                    IsQuoteInMyNeghtborChapter(chapter, qp))
                {
                    mySources = BlockQuoteParticleSources.NeightborChapter;
                    continue;
                }

                if ((mySources == BlockQuoteParticleSources.MyChapterOnly
                     || mySources == BlockQuoteParticleSources.NeightborChapter)
                    && IsQuoteInBook(book, qp))
                {
                    mySources = BlockQuoteParticleSources.MyBook;
                    continue;
                }

                mySources = BlockQuoteParticleSources.OtherBook;
                break;
            }
            return mySources;
        }

        private bool IsQuoteInBook(Book book, QuoteSourceParticle qp)
        {
            return book.Chapters.Any(c => c.ChapterBlock.BlockId == qp.SourceTextParticle.Block.BlockId);
        }

        private static bool IsQuoteInMyNeghtborChapter(Chapter chapter, QuoteSourceParticle qp)
        {
            if ((chapter.PrevChapter != null &&
                 qp.SourceTextParticle.Block.BlockId
                 == chapter.PrevChapter.ChapterBlock.BlockId)
                ||
                (chapter.NextChapter != null &&
                 qp.SourceTextParticle.Block.BlockId
                 == chapter.NextChapter.ChapterBlock.BlockId))
                return true;
            return false;
        }

        private static bool IsQuoteInMyChapter(QuoteSourceParticle qp, Chapter chapter)
        {
            if (qp.SourceTextParticle.Block.BlockId
                == chapter.ChapterBlock.BlockId) return true;
            return false;
        }
    }
}
