﻿using DAL.Entity;

namespace MemOrg.Interfaces.OrgUnits
{
    public interface IOrgBlockTag : IOrgBlock
    {
        Tag Tag { get; }
    }
}