using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    [XmlInclude(typeof(XmlQuoteSource))]
    [XmlInclude(typeof(XmlUserText))]
    [XmlInclude(typeof(XmlSourceText))]
    public abstract class XmlParticle
    {
        public Int32 ParticleId;
        public Int32 Order;

        public static List<XmlParticle> Convert(IEnumerable<Particle> particles)
        {
            var xmlParticles = new List<XmlParticle>();

            foreach (var particle in particles)
            {
                if (particle is SourceTextParticle)
                {
                    var xmlSourceText = new XmlSourceText
                    {
                        Order = particle.Order,
                        ParticleId = particle.ParticleId,
                        Content = (particle as SourceTextParticle).Content
                    };
                    xmlParticles.Add(xmlSourceText);
                } 
                
                if (particle is QuoteSourceParticle)
                {
                    var xmlQuoteParticle = new XmlQuoteSource()
                    {
                        Order = particle.Order,
                        ParticleId = particle.ParticleId,
                        SourceTextId = (particle as QuoteSourceParticle).SourceTextParticleId
                    };
                    xmlParticles.Add(xmlQuoteParticle);
                }

                if (particle is UserTextParticle)
                {
                    var xmlUserText = new XmlUserText
                    {
                        Order = particle.Order,
                        ParticleId = particle.ParticleId,
                        Content = (particle as UserTextParticle).Content
                    };
                    xmlParticles.Add(xmlUserText);
                } 
            }

            return xmlParticles;
        }
    }
}