﻿using System.Collections.Generic;
using DAL.Entity;

namespace MemOrg.Interfaces
{
    public interface IChapter
    {
        Block ChapterBlock { get; set; }
        List<IPage> PagesBlocks { get; set; }
        IChapter NextChapter { get; set; }
        IChapter PrevChapter { get; set; }
    }
}