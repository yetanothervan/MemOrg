using System;
using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class VisaulGraphProcessing
    {
        public static VisualGraph CreateVisualGraph(IGraphService gs)
        {
            var visualGraphElems = new List<IVisualGraphElem>();
            
            foreach (var b in gs.BlockTags)
            {



                if (b.Particles.Count == 0)
                {
                    var planarGraphBlock = new VisualGraphElem(b, 200, VisualGraphBlockType.ReferenceBlock);
                    visualGraphElems.Add(planarGraphBlock);
            
                    continue;
                }

                bool severalSources = false;
                Int32 lastId = -1;
                foreach (var particle in b.Particles)
                {
                    if (!(particle is QuoteSourceParticle)) continue;
                    var parRef = particle as QuoteSourceParticle;
                    if (lastId == -1)
                    {
                        lastId = parRef.SourceTextParticle.BlockId;
                        continue;
                    }
                    if (lastId == parRef.SourceTextParticle.BlockId) continue;
                    
                    severalSources = true;
                    break;
                }
                if (severalSources)
                {
                    var planarGraphBlock = new VisualGraphElem(b, 200, VisualGraphBlockType.SeveralSourcesQuoteBlock);
                    visualGraphElems.Add(planarGraphBlock);
                    ++stats.SeveralSourcesQuoteBlockCount;
                    continue;
                }
                if (lastId != -1)
                {
                    var planarGraphBlock = new VisualGraphElem(b, 200, VisualGraphBlockType.OneSourceQuoteBlock);
                    visualGraphElems.Add(planarGraphBlock);
                    ++stats.OneSourceQuoteBlockCount;
                    continue;
                }
                {
                    var planarGraphBlock = new VisualGraphElem(b, 200, VisualGraphBlockType.SourceBlock);
                    visualGraphElems.Add(planarGraphBlock);
                    ++stats.SourceBlockCount;
                }
            }

            var result = new VisualGraph();
            result.SetBlocks(visualGraphElems);
            result.Stats = stats;
            return result;
        }
    }
}