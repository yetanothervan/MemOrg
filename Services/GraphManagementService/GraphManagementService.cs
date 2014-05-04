﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Particles = new List<Particle>()
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
            _eventAggregator.GetEvent<ParticleChanged>().Publish(particle);
        }

        public void AddSourceParticle(Block sourceBlock)
        {
            var block = _graphService.TrackingBlocks.FirstOrDefault(b => b.BlockId == sourceBlock.BlockId);
            if (block == null) return;
            var max = block.Particles.Count > 0 ? block.Particles.Max(o => o.Order) : 0;
            var particle = new SourceTextParticle {Block = block, Order = ++max};
            block.Particles.Add(particle);
            _graphService.SaveChanges();
            _eventAggregator.GetEvent<BlockChanged>().Publish(block);
        }

        public void RemoveSourceParticle(Particle particle)
        {
            var block = particle.Block;
            _graphService.RemoveSourceParticle(particle);
            _graphService.SaveChanges();
            var blockFromBase = _graphService.BlockAll.First(b => b.BlockId == block.BlockId);
            _eventAggregator.GetEvent<BlockChanged>().Publish(blockFromBase);
        }

        public Block ExtractNewBlockFromParticle(Particle particle, int start, int length, string caption)
        {
            var body = ExtractSourcePartition(particle, start, length);
            var block = new Block
            {
                Caption = caption,
                Particles =
                    new Collection<Particle>
                    {
                        new QuoteSourceParticle {Order = 0, SourceTextParticleId = body.ParticleId}
                    }
            };
            _graphService.AddBlock(block);
            _graphService.SaveChanges();
            _eventAggregator.GetEvent<BlockChanged>().Publish(body.Block);
            return block;
        }

        public void ExtractParticleToExistBlock(Particle particle, Block targetBlock, int start, int length)
        {
            var body = ExtractSourcePartition(particle, start, length);
            var block = _graphService.TrackingBlocks.FirstOrDefault(b => b.BlockId == targetBlock.BlockId);
            if (block == null) return;
            var max = block.Particles.Count > 0 ? block.Particles.Max(o => o.Order) : 0;
            block.Particles.Add(new QuoteSourceParticle
            {
                Block = block,
                Order = ++max,
                SourceTextParticleId = body.ParticleId
            });
            _graphService.SaveChanges();
            _eventAggregator.GetEvent<BlockChanged>().Publish(body.Block);
            _eventAggregator.GetEvent<BlockChanged>().Publish(block);
        }

        public void AddNewRelation(string relType, Block first, string captionFirst, Block second, string captionSecond,
            Particle particle, int start, int length)
        {
            var relTypeBase = _graphService.RelationTypes.FirstOrDefault(rt => rt.Caption == relType);
            if (relTypeBase == null)
            {
                relTypeBase = new RelationType {Caption = relType};
                _graphService.AddRelationType(relTypeBase);
                _graphService.SaveChanges();
            }

            if (first == null)
            {
                first = new Block {Caption = captionFirst};
                _graphService.AddBlock(first);
                _graphService.SaveChanges();
            }

            if (second == null)
            {
                second = new Block { Caption = captionSecond };
                _graphService.AddBlock(second);
                _graphService.SaveChanges();
            }

            Block relBlock = null;
            if (particle != null)
            {
                string relBlockCaption = String.Format("{0} {1} {2}", first.Caption, relType, second.Caption);
                relBlock = ExtractNewBlockFromParticle(particle, start, length,
                    relBlockCaption);
            }

            var rel = new Relation
            {
                FirstBlockId = first.BlockId,
                SecondBlockId = second.BlockId,
                RelationBlockId = (relBlock == null) ? null : (int?)relBlock.BlockId,
                RelationType = relTypeBase
            };

            _graphService.AddRelation(rel);
            _graphService.SaveChanges();
        }

        private SourceTextParticle ExtractSourcePartition(Particle particle, int start, int length)
        {
            var sp = particle as SourceTextParticle;
            if (sp == null) return null;

            var text1 = sp.Content.Substring(0, start);
            var text2 = sp.Content.Substring(start, length);
            var text3 = sp.Content.Substring(start + length);

            int order1 = -1;
            int order2;
            int order3 = -1;
            int addind = 0;

            if (!String.IsNullOrEmpty(text1))
            {
                order1 = particle.Order;
                order2 = particle.Order + 1;
                ++addind;
            }
            else
                order2 = particle.Order;

            if (!String.IsNullOrEmpty(text3))
            {
                order3 = order2 + 1;
                ++addind;
            }

            var blockId = particle.Block.BlockId;

            var parts = _graphService.TrackingParticles.Where(p => p.Block.BlockId == particle.Block.BlockId);
            foreach (var part in parts)
            {
                if (part.Order > particle.Order)
                    part.Order += addind;
            }
            _graphService.RemoveSourceParticle(particle);
            _graphService.SaveChanges();

            var block = _graphService.TrackingBlocks.First(b => b.BlockId == blockId);

            if (!String.IsNullOrEmpty(text1))
                block.Particles.Add(new SourceTextParticle { Block = block, Content = text1, Order = order1 });
            var body = new SourceTextParticle { Block = block, Content = text2, Order = order2 };
            block.Particles.Add(body);
            if (!String.IsNullOrEmpty(text3))
                block.Particles.Add(new SourceTextParticle { Block = block, Content = text3, Order = order3 });
            _graphService.SaveChanges();
            return body;
        }
    }
}
