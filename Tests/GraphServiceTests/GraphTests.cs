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
            
            ITmpXmlExportImportService xmlExportImport
                = new TmpXmlExportImportService.TmpXmlExportImportService(result);
            xmlExportImport.LoadGraph(xmlFileName);
            
            return result;
        }

        [Test]
        public void Books_LoadedCorrect_CountCorrect()
        {
            var gs = GetGraphService("..\\..\\GraphTestBooks.xml");

            var books = gs.GetGraph().Books;

            Assert.AreNotSame(null, books);
            Assert.AreEqual(3, books.Count());
        }
    }
}
