using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlRelation
    {
        public Int32 RelationId;
        public Int32 RelationType;
        public Int32 FirstBlockId;
        public Int32 SecondBlockId;
        public Int32? RelationBlockId;
        
        public static List<XmlRelation> Convert(IEnumerable<Relation> rels)
        {
            return rels.Select(rel => new XmlRelation
            {
                RelationId = rel.RelationId,
                RelationType = rel.RelationTypeId,
                FirstBlockId = rel.FirstBlockId,
                SecondBlockId = rel.SecondBlockId,
                RelationBlockId = rel.RelationBlockId
            }).ToList();
        }
    }
}