using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IGraphManagementService
    {
        void AddNewChapter(string caption, string bookName, int chapterNumber);
    }
}
