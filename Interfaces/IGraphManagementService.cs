using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphManagementService
    {
        void AddNewChapter(string caption, string bookName, int chapterNumber);
        void UpdateParticleText(int particleId, string newText);
        void AddSourceParticle(Block sourceBlock);
        bool RemoveParticle(Particle particle);
        Block ExtractNewBlockFromParticle(Particle particle, int start, int length, string caption);
        void ExtractParticleToExistBlock(Particle particle, Block targetBlock, int start, int length);
        void AddNewRelation(string relType, Block first, string captionFirst, Block second, string captionSecond, 
            Particle particle, int start, int length);
        void AddNewReference(string refType, Block first, Block second, string captionSecond, bool isUserText);
    }
}
