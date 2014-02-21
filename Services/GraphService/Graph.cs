﻿using System;
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
            {
                book.ChaptersInternal =
                    Enumerable.Cast<IChapter>(
                        _graphService.BlockSources
                            .Where(b => b.ParamName == book.CaptionInternal)
                            .OrderBy(b => b.ParamName)
                            .Select(c => new Chapter {ChapterBlock = c})).ToList();
                
                foreach (var chapter in book.ChaptersInternal.Cast<Chapter>())
                    chapter.MyBookInternal = book;
            }

            //enumerate chapters
            foreach (var book in books)
                for (int i = 0; i < book.Chapters.Count; ++i)
                {
                    if (i != 0)
                        book.Chapters[i].PrevChapter = book.Chapters[i - 1];
                    if (i != book.Chapters.Count - 1)
                        book.Chapters[i].NextChapter = book.Chapters[i + 1];
                }

            //feel chapters with page-blocks
            foreach (var book in books.Cast<Book>())
                foreach (var chapter in book.Chapters.Cast<Chapter>())
                    FillChapterWithPageBlocks(chapter);

            //tune page-rels
            foreach (var book in books.Cast<Book>())
                foreach (var chapter in book.Chapters)
                    TunePageRels(chapter);

            //add references
            foreach (var book in books.Cast<Book>())
                foreach (var chapter in book.Chapters)
                    ProvideWithReferences(chapter);

            return books;
        }

        private void ProvideWithReferences(IChapter chapter)
        {
            var blocks = chapter.PagesBlocks.Select(c => c.Block.BlockId).ToList();
            var relnoblocksOfChapter = _graphService.RelationsNoBlock.Where(r => blocks.Contains(r.FirstBlock.BlockId)
                                                                                 ||
                                                                                 blocks.Contains(r.SecondBlock.BlockId))
                .ToList();

            foreach (var rel in relnoblocksOfChapter)
            {
                var first = chapter.PagesBlocks.FirstOrDefault(p => p.Block.BlockId == rel.FirstBlock.BlockId);
                var second = chapter.PagesBlocks.FirstOrDefault(p => p.Block.BlockId == rel.SecondBlock.BlockId);
                if (first == null)
                    first = new Page {Block = rel.FirstBlock};
                if (second == null)
                    second = new Page {Block = rel.SecondBlock};
                first.ReferencedBy.Add(second);
                second.ReferencedBy.Add(first);
            }

            foreach (var page in chapter.PagesBlocks.Where(p => p.Block.References.Any()).ToList())
            {
                foreach (var reference in page.Block.References)
                {
                    var referencedBlock =
                        chapter.PagesBlocks.FirstOrDefault(p =>
                            p.Block.BlockId == reference.ReferencedBlock.BlockId)
                        ?? new Page {Block = reference.ReferencedBlock};
                    page.ReferencedBy.Add(referencedBlock);
                }
            }
        }

        private void FillChapterWithPageBlocks(Chapter chapter)
        {
            //all blocks with chapter's block as source
            var chaptersBlocks = _graphService.BlockAll
                .Where(block => block.Particles
                    .Any(p => p is QuoteSourceParticle
                              && (p as QuoteSourceParticle).SourceTextParticle.Block.BlockId
                              == chapter.ChapterBlock.BlockId)).ToList();

            foreach (var block in chaptersBlocks)
            {
                var sources = DetermineBlockQuatasSources(block, chapter);

                if (sources == BlockQuoteParticleSources.OtherBook)
                    continue;

                if (sources != BlockQuoteParticleSources.MyChapterOnly 
                    && chapter.MyBook.GetPageByBlock(block.BlockId) != null) continue;

                chapter.PagesBlocks.Add(new Page
                {
                    MyChapterInternal = chapter,
                    Block = block,
                    Tag = _graphService.TagsBlock
                        .FirstOrDefault(t => t.TagBlock.BlockId == block.BlockId),
                    Relation = _graphService.RelationsBlock
                        .FirstOrDefault(r => r.RelationBlock.BlockId == block.BlockId),
                    MySources = sources
                });
            }
        }

        private void TunePageRels(IChapter chapter)
        {
            foreach (var page in chapter.PagesBlocks.Where(p => p.IsBlockRel).ToList())
            {
                var firstPage = chapter.MyBook.GetPageByBlock(page.Relation.FirstBlock.BlockId);
                if (firstPage == null)
                {
                    firstPage = new Page
                    {
                        Block = page.Relation.FirstBlock,
                        MySources = BlockQuoteParticleSources.NoSources
                    };
                    chapter.PagesBlocks.Add(firstPage);
                }
                
                var secondPage = chapter.MyBook.GetPageByBlock(page.Relation.SecondBlock.BlockId);
                if (secondPage == null)
                {
                    secondPage = new Page
                    {
                        Block = page.Relation.SecondBlock,
                        MySources = BlockQuoteParticleSources.NoSources
                    };
                    chapter.PagesBlocks.Add(secondPage);
                }

                page.RelationFirst = firstPage;
                page.RelationSecond = secondPage;
                firstPage.RelatedBy.Add(page);
                secondPage.RelatedBy.Add(page);
            }
        }

        private BlockQuoteParticleSources DetermineBlockQuatasSources(Block pageBlock, Chapter chapter)
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
                    && IsQuoteInBook(chapter.MyBook, qp))
                {
                    mySources = BlockQuoteParticleSources.MyBook;
                    continue;
                }

                mySources = BlockQuoteParticleSources.OtherBook;
                break;
            }
            return mySources;
        }

        private bool IsQuoteInBook(IBook book, QuoteSourceParticle qp)
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
