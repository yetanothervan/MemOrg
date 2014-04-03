using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace TmpXmlExportImportService
{
    public class TmpXmlExportImportService : ITmpXmlExportImportService
    {
        public void SaveGraph()
        {
            var graphService = (IGraphService)ServiceLocator.Current.GetService(typeof(IGraphService));
            var g = Graph2XmlGraphConverter.Convert(graphService);
            SerializeGraph(g);
        }

        public void LoadGraph()
        {
            var graphService = (IGraphService) ServiceLocator.Current.GetService(typeof (IGraphService));
            var xmlGraph = XmlDeserializeSolution();

            var blockIds = new int[xmlGraph.Blocks.Max(b => b.BlockId) + 1];
            var relationTypeIds = new int[xmlGraph.RelationTypes.Max(t => t.RelationTypeId) + 1];
            var tagIds = new int[xmlGraph.Tags.Max(t => t.TagId) + 1];
            var partIds = new int[xmlGraph.Blocks
                .Where(b => b.Particles != null && b.Particles.Count != 0)
                .Max(b => b.Particles.Max(p => p.ParticleId)) + 1];

            foreach (var rt in xmlGraph.RelationTypes)
            {
                var i = new RelationType {Caption = rt.Caption};
                graphService.AddRelationType(i);
                relationTypeIds[rt.RelationTypeId] = i.RelationTypeId;
            }
            graphService.SaveChanges();

            foreach (var block in xmlGraph.Blocks)
            {
                var i = new Block
                {
                    Caption = block.Caption,
                    ParamName = block.ParamName,
                    ParamValue = block.ParamValue
                };
                graphService.AddBlock(i);
                blockIds[block.BlockId] = i.BlockId;
            }
            graphService.SaveChanges();

            foreach (var tag in xmlGraph.Tags)
            {
                var i = new Tag
                {
                    Caption = tag.Caption,
                    TagBlockId = tag.TagBlockId != null ? blockIds[tag.TagBlockId.Value] : (int?) null
                };
                graphService.AddTag(i);
                tagIds[tag.TagId] = i.TagId;
            }
            graphService.SaveChanges();

            foreach (var tag in xmlGraph.Tags)
            {
                int id = tagIds[tag.TagId];
                graphService.TrackingTags
                    .First(t => t.TagId == id).ParentId = (tag.ParentTagId != null)
                        ? tagIds[tag.ParentTagId.Value]
                        : (int?) null;
                graphService.SaveChanges();
            }

            foreach (var block in xmlGraph.Blocks)
            {
                var id = blockIds[block.BlockId];
                var bb = graphService.TrackingBlocks.First(b => b.BlockId == id);
                var toi = block.Particles.Where(p => p is XmlSourceText || p is XmlUserText).ToList();
                var i = toi.Select(p =>
                {
                    Particle res;
                    if (p is XmlSourceText)
                        res = new SourceTextParticle {Content = (p as XmlSourceText).Content};
                    else if (p is XmlUserText)
                        res = new UserTextParticle {Content = (p as XmlUserText).Content};
                    else
                        throw new NotImplementedException();

                    res.Order = p.Order;
                    return res;
                }).ToList();
                bb.Particles = i;
                graphService.SaveChanges();
                for (int index = 0; index < i.Count; index++)
                    partIds[toi[index].ParticleId] = i[index].ParticleId;

                var trueTags = block.Tags.Select(t => tagIds[t]);
                
                bb.Tags = graphService.TrackingTags.Where(t => trueTags.Contains(t.TagId)).ToList();
                bb.References = block.References.Select(r => new Reference
                {
                    CaptionsString = r.CaptionString,
                    ReferencedBlockId = blockIds[r.ReferenceBlockId]
                }).ToList();
            }
            graphService.SaveChanges();

            foreach (var block in xmlGraph.Blocks)
            {
                var ii = blockIds[block.BlockId];
                var bb = graphService.TrackingBlocks.First(b => b.BlockId == ii);
                foreach (var p in block.Particles.OfType<XmlQuoteSource>())
                {
                    var res = new QuoteSourceParticle
                    {
                        SourceTextParticleId = partIds[p.SourceTextId],
                        Order = p.Order
                    };
                    bb.Particles.Add(res);
                }
            }
            graphService.SaveChanges();

            foreach (var relation in xmlGraph.Relations)
            {
                graphService.AddRelation(new Relation
                {
                    RelationTypeId = relationTypeIds[relation.RelationType],
                    RelationBlockId = (relation.RelationBlockId != null) 
                    ? blockIds[relation.RelationBlockId.Value] 
                    : (int?)null,
                    FirstBlockId = blockIds[relation.FirstBlockId],
                    SecondBlockId = blockIds[relation.SecondBlockId]
                });
            }
            graphService.SaveChanges();
        }

        const string GraphPath = "..\\..\\..\\BL\\TmpXmlExportImport\\graph.xml";
        
        public static void SerializeGraph(XmlGraph s)
        {
            var xmlSerializer = new XmlSerializer(typeof (XmlGraph));
            var sb = new StringBuilder();
            var w = new StringWriter(sb);
            xmlSerializer.Serialize(w, s);

            var doc = new XmlDocument {InnerXml = sb.ToString()};
            doc.Save(GraphPath);
        }

        public static XmlGraph XmlDeserializeSolution()
        {
            TextReader tr = new StreamReader(GraphPath);
            var xmlDeserializer = new XmlSerializer(typeof(XmlGraph));
            var res = (XmlGraph) xmlDeserializer.Deserialize(tr);
            tr.Close();
            return res;
        }
    }
}
