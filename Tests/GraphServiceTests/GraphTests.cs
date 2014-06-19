using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
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

            Assert.AreEqual(2, booksList.First(b => b.Caption == "BookOne").Chapters.Count);
            Assert.AreEqual(3, booksList.First(b => b.Caption == "BookTwo").Chapters.Count);
            Assert.AreEqual(2, booksList.First(b => b.Caption == "BookThree").Chapters.Count);
        }

        [Test]
        public void Books_ChaptersOfBookTwoOrder_Correct()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var books = gs.GetGraph().Books;
            var bookTwo = books.First(b => b.Caption == "BookTwo");

            Assert.AreEqual(bookTwo.Chapters[1], bookTwo.Chapters[0].NextChapter);
            Assert.AreEqual(bookTwo.Chapters[2], bookTwo.Chapters[1].NextChapter);
            Assert.AreEqual(bookTwo.Chapters[1], bookTwo.Chapters[2].PrevChapter);
            Assert.AreEqual(bookTwo.Chapters[0], bookTwo.Chapters[1].PrevChapter);
        }

        [Test]
        public void DetermineBlockQuatasSources_NoParticles_ReturnNoSources()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");


        }
    }
}
