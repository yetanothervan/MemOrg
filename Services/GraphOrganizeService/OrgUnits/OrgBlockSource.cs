﻿using System.Collections.Generic;
using DAL.Entity;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphOrganizeService.OrgUnits
{
    public class OrgBlockSource : OrgBlock, IOrgBlockSource
    {
        public OrgBlockSource(IPage page, List<NESW> conPoints)
            : base(page, conPoints)
        {
        }
    }
}