using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlReference
    {
        public Int32 ReferenceId;
        public Int32 BlockId;
        public Int32 ReferenceBlockId;
        public String CaptionString;

        public static List<XmlReference> Convert(IEnumerable<Reference> references)
        {
            return references.Select(reference => new XmlReference
            {
                ReferenceId = reference.ReferenceId,
                BlockId = reference.BlockId, 
                CaptionString = reference.CaptionsString,
                ReferenceBlockId = reference.ReferencedBlock.BlockId
            }).ToList();
        }
    }
}