using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlRelationType
    {
        public Int32 RelationTypeId;
        public string Caption;

        public static List<XmlRelationType> Convert(IEnumerable<RelationType> relationTypes)
        {
            return relationTypes.Select(rt => new XmlRelationType
            {
                RelationTypeId = rt.RelationTypeId,
                Caption = rt.Caption
            }).ToList();
        }
    }
}
