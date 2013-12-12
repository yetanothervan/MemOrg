using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlReference
    {
        public Int32 ReferenceId;
        public String Caption;
        public Int32 BlockId;

        public static List<XmlReference> Convert(IList<Reference> references)
        {
            return references.Select(reference => new XmlReference
            {
                BlockId = reference.Block.BlockId, 
                Caption = reference.Caption, 
                ReferenceId = reference.ReferenceId
            }).ToList();
        }
    }
}