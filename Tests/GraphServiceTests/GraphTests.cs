using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using GraphService;
using MemOrg.Interfaces;
using NUnit.Framework;

namespace GraphServiceTests
{
    [TestFixture]
    public class GraphTests
    {
        private IGraphService GetGraphService(string xmlFileName)
        {
            var context = new MemOrgContext("MemOrgTest");

            IGraphService result = new GraphService.GraphService
                (new BlockRepository(context),
                    new TagRepository(context),
                    new RelationRepository(context),
                    new ParticlesRepository(context));

            result.ClearGraph();
            
            var xmlExportImport
                = new TmpXmlExportImportService.TmpXmlExportImportService {GraphService = result};
            xmlExportImport.LoadGraph(xmlFileName);
            
            return result;
        }

        [Test]
        public void Books_BookCount_Correct()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var books = gs.GetGraph().Books;

            Assert.AreNotSame(null, books);
            Assert.AreEqual(3, books.Count());
        }

        [Test]
        public void Books_ChaptersCount_Correct()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var books = gs.GetGraph().Books;
            var booksList = books as IList<IBook> ?? books.ToList();

            Assert.AreEqual(3, booksList.First(b => b.Caption == "b1").Chapters.Count);
            Assert.AreEqual(2, booksList.First(b => b.Caption == "b2").Chapters.Count);
            Assert.AreEqual(3, booksList.First(b => b.Caption == "b3").Chapters.Count);
        }

        [Test]
        public void Books_ChaptersOfBookB1_Correct()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var books = gs.GetGraph().Books;
            var b1 = books.First(b => b.Caption == "b1");

            Assert.AreEqual(b1.Chapters[1], b1.Chapters[0].NextChapter);
            Assert.AreEqual(b1.Chapters[2], b1.Chapters[1].NextChapter);
            Assert.AreEqual(b1.Chapters[1], b1.Chapters[2].PrevChapter);
            Assert.AreEqual(b1.Chapters[0], b1.Chapters[1].PrevChapter);
        }

        [Test]
        public void DetermineBlockQuatasSources_NoParticles_ReturnsNoSources()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var chapterB1C1 = gs.GetGraph().Books.First(b => b.Caption == "b1")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b1c1");
            var blockNoParticles = gs.BlockAll.First(b => b.Caption == "noParticles");

            var res = Graph.DetermineBlockQuatasSources(blockNoParticles, chapterB1C1);

            Assert.AreEqual(BlockQuoteParticleSources.NoSources, res);
        }

        [Test]
        public void DetermineBlockQuatasSources_ChapterOnly_ReturnsMyChapterOnly()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var chapterB1C1 = gs.GetGraph().Books.First(b => b.Caption == "b1")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b1c1");

            var blockChapterOnly = gs.BlockAll.First(b => b.Caption == "chapterOnly");

            var res = Graph.DetermineBlockQuatasSources(blockChapterOnly, chapterB1C1);

            Assert.AreEqual(BlockQuoteParticleSources.MyChapterOnly, res);
        }

        [Test]
        public void DetermineBlockQuatasSources_Neightbor_ReturnsNeightbor()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var chapterB1C1 = gs.GetGraph().Books.First(b => b.Caption == "b1")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b1c1");

            var chapterB1C2 = gs.GetGraph().Books.First(b => b.Caption == "b1")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b1c2");

            var blockNeightbor = gs.BlockAll.First(b => b.Caption == "neightbor");

            var res1 = Graph.DetermineBlockQuatasSources(blockNeightbor, chapterB1C1);
            var res2 = Graph.DetermineBlockQuatasSources(blockNeightbor, chapterB1C2);

            Assert.AreEqual(BlockQuoteParticleSources.NeightborChapter, res1);
            Assert.AreEqual(BlockQuoteParticleSources.NeightborChapter, res2);
        }

        [Test]
        public void DetermineBlockQuatasSources_MyBook_ReturnsMyBook()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var chapterB1C1 = gs.GetGraph().Books.First(b => b.Caption == "b1")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b1c1");

            var chapterB1C3 = gs.GetGraph().Books.First(b => b.Caption == "b1")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b1c3");

            var blockMyBook = gs.BlockAll.First(b => b.Caption == "myBook");

            var res1 = Graph.DetermineBlockQuatasSources(blockMyBook, chapterB1C1);
            var res2 = Graph.DetermineBlockQuatasSources(blockMyBook, chapterB1C3);

            Assert.AreEqual(BlockQuoteParticleSources.MyBook, res1);
            Assert.AreEqual(BlockQuoteParticleSources.MyBook, res2);
        }

        [Test]
        public void DetermineBlockQuatasSources_OtherBook_ReturnsOtherBook()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var chapterB1C1 = gs.GetGraph().Books.First(b => b.Caption == "b1")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b1c1");

            var chapterB2C1 = gs.GetGraph().Books.First(b => b.Caption == "b2")
                .Chapters.First(c => c.ChapterPage.Block.Caption == "b2c1");

            var blockOtherBook = gs.BlockAll.First(b => b.Caption == "otherBook");

            var res1 = Graph.DetermineBlockQuatasSources(blockOtherBook, chapterB1C1);
            var res2 = Graph.DetermineBlockQuatasSources(blockOtherBook, chapterB2C1);

            Assert.AreEqual(BlockQuoteParticleSources.OtherBook, res1);
            Assert.AreEqual(BlockQuoteParticleSources.OtherBook, res2);
        }
    }
}
