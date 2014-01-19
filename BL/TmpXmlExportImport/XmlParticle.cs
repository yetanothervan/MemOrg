using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    [XmlInclude(typeof(XmlParagraph))]
    [XmlInclude(typeof(XmlParagraphRef))]
    public abstract class XmlParticle
    {
        public Int32 ParticleId;
        public Int32 BlockId;
        public Int32 Order;

        public static List<XmlParticle> Convert(IList<Particle> particles)
        {
            var xmlParticles = new List<XmlParticle>();

            foreach (var particle in particles)
            {
                if (particle is SourceTextParticle)
                {
                    var xmlParagraph = new XmlParagraph
                    {
                        BlockId = particle.BlockId,
                        Order = particle.Order,
                        ParticleId = particle.ParticleId,
                        Content = (particle as SourceTextParticle).Content
                    };
                    xmlParticles.Add(xmlParagraph);
                } 
                
                if (particle is QuoteSourceParticle)
                {
                    var xmlParagraphRef = new XmlParagraphRef
                    {
                        BlockId = particle.BlockId,
                        Order = particle.Order,
                        ParticleId = particle.ParticleId,
                        ParagraphId = (particle as QuoteSourceParticle).ParticleId
                    };
                    xmlParticles.Add(xmlParagraphRef);
                }
            }

            return xmlParticles;
        }
    }
}