using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using MemOrg.Interfaces;

namespace TmpXmlExportImportService
{
    internal static class Graph2XmlGraphConverter
    {
        public static XmlGraph Convert(IGraphService graphService)
        {
            var xmlBlocks = 
            graphService.BlockOthers
                .Union(graphService.BlockRels)
                .Union(graphService.BlockSources)
                .Union(graphService.BlockTags).Select(block => new XmlBlock
            {
                
                BlockId = block.BlockId, 
                Caption = block.Caption, 
                Particles = XmlParticle.Convert(block.Particles),
                References = XmlReference.Convert(block.References),
                Tags = XmlTag.Convert(block.Tags)
            }).ToList();

            var xmlTags =
                graphService.TagsBlock.Union(graphService.TagsNoBlock).ToList();

            return new XmlGraph { Blocks = xmlBlocks, Tags = XmlTag.Convert(xmlTags)};
        }
    }
}
