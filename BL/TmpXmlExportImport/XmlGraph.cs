using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlGraph
    {
        public List<XmlRelationType> RelationTypes;
        public List<XmlBlock> Blocks;
        public List<XmlTag> Tags;
        public List<XmlRelation> Relations;
    }
}
