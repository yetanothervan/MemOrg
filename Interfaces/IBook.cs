using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public interface IBook
    {
        string Caption { get; }
        IEnumerable<IChapter> Chapters { get; }
    }
}