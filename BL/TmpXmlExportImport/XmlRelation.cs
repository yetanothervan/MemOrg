using System;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlRelation
    {
        public Int32 RelationId;
        public XmlRelationType RelationType;
        public Int32 FirstBlockId;
        public Int32 SecondBlockId;
        public Int32 RelationBlockId;
    }
}