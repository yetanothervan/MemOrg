using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    internal class Graph2XmlGraphConverter
    {
        public static XmlGraph Convert(IList<Block> blocks)
        {
            var xmlBlocks = blocks.Select(block => new XmlBlock
            {
                BlockId = block.BlockId, 
                Caption = block.Caption, 
                Particles = XmlParticle.Convert(block.Particles),
                References = XmlReference.Convert(block.References)
            }).ToList();

            return new XmlGraph { Blocks = xmlBlocks };
        }
    }
}
