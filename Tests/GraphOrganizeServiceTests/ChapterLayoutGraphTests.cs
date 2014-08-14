using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using MemOrg.Interfaces;
using NUnit.Framework;

namespace GraphOrganizeServiceTests
{
    [TestFixture]
    public class ChapterLayoutGraphTests
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
                = new TmpXmlExportImportService.TmpXmlExportImportService { GraphService = result };
            xmlExportImport.LoadGraph(xmlFileName);

            return result;
        }

    }
}
