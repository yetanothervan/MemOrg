using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Events;

namespace GraphManagementService
{
    public class GraphManagementService : IGraphManagementService
    {
        private readonly IGraphService _graphService;
        private readonly IEventAggregator _eventAggregator;

        public GraphManagementService(IGraphService graphService, IEventAggregator eventAggregator)
        {
            _graphService = graphService;
            _eventAggregator = eventAggregator;
        }

        public void AddNewChapter( string caption, string bookName, int chapterNumber)
        {
            var chapter = new Block
            {
                Caption = caption,
                ParamName = bookName,
                ParamValue = chapterNumber,
                Particles = new List<Particle>
                {
                    new SourceTextParticle {Content = ""}
                }
            };

            _graphService.AddBlock(chapter);
        }

        public void UpdateParticleText(int particleId, string newText)
        {
            var particle = _graphService.TrackingParticles
                .FirstOrDefault(p => p.ParticleId == particleId);
            if (particle == null) return;
            if (particle is SourceTextParticle)
                (particle as SourceTextParticle).Content = newText;
            else if (particle is UserTextParticle)
                (particle as UserTextParticle).Content = newText;
            _graphService.SaveChanges();
            _eventAggregator.GetEvent<ParticleChanged>().Publish(particleId);
        }
    }
}
