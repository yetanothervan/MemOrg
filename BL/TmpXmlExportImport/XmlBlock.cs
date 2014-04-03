using System;
using System.Collections.Generic;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlBlock
    {
        public XmlBlock()
        {
            Tags = new List<int>();
        }
        public Int32 BlockId;
        public String Caption;
        //!!! выпилить после реализации параметров тега
        public String ParamName;
        public Int32 ParamValue;
        public List<XmlParticle> Particles;

        public List<Int32> Tags;
        public List<XmlReference> References;
    }
}