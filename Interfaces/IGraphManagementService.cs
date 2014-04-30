using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphManagementService
    {
        void AddNewChapter(string caption, string bookName, int chapterNumber);
        void UpdateParticleText(int particleId, string newText);
        void AddSourceParticle(Block sourceBlock);
        void RemoveSourceParticle(Particle particle);
        void ExtractNewBlockFromParticle(Particle particle, int start, int length, string caption);
    }
}
