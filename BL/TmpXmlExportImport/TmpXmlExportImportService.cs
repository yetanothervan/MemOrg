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
        const string GraphPath = "..\\..\\..\\BL\\TmpXmlExportImport\\graph.xml";

        private IGraphService _graphService;

        public TmpXmlExportImportService()
        {
            _graphService = (IGraphService)ServiceLocator.Current.GetService(typeof(IGraphService));
        }

        public TmpXmlExportImportService(IGraphService graphService)
        {
            _graphService = graphService;
        }

        public void SaveGraph()
        {
            SaveGraph(GraphPath);
        }

        public void LoadGraph()
        {
            LoadGraph(GraphPath);
        }

        public void SaveGraph(string path)
        {
            var g = Graph2XmlGraphConverter.Convert(_graphService);
            SerializeGraph(g, path);
        }

        public void LoadGraph(string path)
        {
            var xmlGraph = XmlDeserializeSolution(path);

            var blockIds = new int[xmlGraph.Blocks.Max(b => b.BlockId) + 1];
            var relationTypeIds = xmlGraph.RelationTypes.Count == 0 
                ? new int[0]
                : new int[xmlGraph.RelationTypes.Max(t => t.RelationTypeId) + 1];
            var tagIds = xmlGraph.Tags.Count == 0 
                ? new int[0]
                : new int[xmlGraph.Tags.Max(t => t.TagId) + 1];
            var partIds = !xmlGraph.Blocks.Any(b => b.Particles != null && b.Particles.Count != 0)
                ? new int[0]
                : new int[xmlGraph.Blocks
                .Where(b => b.Particles != null && b.Particles.Count != 0)
                .Max(b => b.Particles.Max(p => p.ParticleId)) + 1];

            foreach (var rt in xmlGraph.RelationTypes)
            {
                var i = new RelationType {Caption = rt.Caption};
                _graphService.AddRelationType(i);
                relationTypeIds[rt.RelationTypeId] = i.RelationTypeId;
            }
            _graphService.SaveChanges();

            foreach (var block in xmlGraph.Blocks)
            {
                var i = new Block
                {
                    Caption = block.Caption,
                    ParamName = block.ParamName,
                    ParamValue = block.ParamValue
                };
                _graphService.AddBlock(i);
                blockIds[block.BlockId] = i.BlockId;
            }
            _graphService.SaveChanges();

            foreach (var tag in xmlGraph.Tags)
            {
                var i = new Tag
                {
                    Caption = tag.Caption,
                    TagBlockId = tag.TagBlockId != null ? blockIds[tag.TagBlockId.Value] : (int?) null
                };
                _graphService.AddTag(i);
                tagIds[tag.TagId] = i.TagId;
            }
            _graphService.SaveChanges();

            foreach (var tag in xmlGraph.Tags)
            {
                int id = tagIds[tag.TagId];
                _graphService.TrackingTags
                    .First(t => t.TagId == id).ParentId = (tag.ParentTagId != null)
                        ? tagIds[tag.ParentTagId.Value]
                        : (int?) null;
                _graphService.SaveChanges();
            }

            foreach (var block in xmlGraph.Blocks)
            {
                var id = blockIds[block.BlockId];
                var bb = _graphService.TrackingBlocks.First(b => b.BlockId == id);
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
                _graphService.SaveChanges();
                for (int index = 0; index < i.Count; index++)
                    partIds[toi[index].ParticleId] = i[index].ParticleId;

                var trueTags = block.Tags.Select(t => tagIds[t]);
                
                bb.Tags = _graphService.TrackingTags.Where(t => trueTags.Contains(t.TagId)).ToList();
                bb.References = block.References.Select(r => new Reference
                {
                    CaptionsString = r.CaptionString,
                    ReferencedBlockId = blockIds[r.ReferenceBlockId]
                }).ToList();
            }
            _graphService.SaveChanges();

            foreach (var block in xmlGraph.Blocks)
            {
                var ii = blockIds[block.BlockId];
                var bb = _graphService.TrackingBlocks.First(b => b.BlockId == ii);
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
            _graphService.SaveChanges();

            foreach (var relation in xmlGraph.Relations)
            {
                _graphService.AddRelation(new Relation
                {
                    RelationTypeId = relationTypeIds[relation.RelationType],
                    RelationBlockId = (relation.RelationBlockId != null) 
                    ? blockIds[relation.RelationBlockId.Value] 
                    : (int?)null,
                    FirstBlockId = blockIds[relation.FirstBlockId],
                    SecondBlockId = blockIds[relation.SecondBlockId]
                });
            }
            _graphService.SaveChanges();
        }

        public static void SerializeGraph(XmlGraph s, string path)
        {
            var xmlSerializer = new XmlSerializer(typeof (XmlGraph));
            var sb = new StringBuilder();
            var w = new StringWriter(sb);
            xmlSerializer.Serialize(w, s);

            var doc = new XmlDocument {InnerXml = sb.ToString()};
            doc.Save(path);
        }

        public static XmlGraph XmlDeserializeSolution(string path)
        {
            TextReader tr = new StreamReader(path);
            var xmlDeserializer = new XmlSerializer(typeof(XmlGraph));
            var res = (XmlGraph) xmlDeserializer.Deserialize(tr);
            tr.Close();
            return res;
        }
    }
}
