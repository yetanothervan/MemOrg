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
            graphService.BlockAll.ToList().Select(block => new XmlBlock
            {
                BlockId = block.BlockId, 
                Caption = block.Caption, 
                ParamName = block.ParamName,
                ParamValue = block.ParamValue,
                Tags = block.Tags.Select(t => t.TagId).ToList(),
                Particles = XmlParticle.Convert(block.Particles),
                References = XmlReference.Convert(block.References)
            }).ToList();

            var xmlTags = XmlTag.Convert(graphService.TagsAll);

            var xmlRelationTypes = XmlRelationType.Convert(graphService.RelationTypes);

            var xmlRelations = XmlRelation.Convert(
                graphService.RelationsBlock.Union(
                    graphService.RelationsNoBlock));

            return new XmlGraph
            {
                RelationTypes = xmlRelationTypes,
                Tags = xmlTags,
                Blocks = xmlBlocks,
                Relations = xmlRelations
            };
        }
    }
}
