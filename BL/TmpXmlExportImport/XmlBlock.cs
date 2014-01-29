using System;
using System.Collections.Generic;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlBlock
    {
        public Int32 BlockId;
        public String Caption;

        public List<XmlParticle> Particles;
        public List<XmlReference> References;
        public List<XmlTag> Tags;
    }
}