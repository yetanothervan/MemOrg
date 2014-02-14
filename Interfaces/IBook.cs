using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IBook
    {
        string Caption { get; }
        IList<IChapter> Chapters { get; }
    }
}