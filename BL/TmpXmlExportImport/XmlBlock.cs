using System;
using System.Collections.Generic;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlBlock
    {
        public Int32 BlockId { get; set; }
        public String Caption { get; set; }

        public virtual List<XmlParticle> Particles { get; set; }
        public virtual List<XmlReference> References { get; set; }
        public virtual List<XmlTag> Tags { get; set; }
    }
}