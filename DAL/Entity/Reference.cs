﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Reference
    {
        public Int32 ReferenceId { get; set; }
        public Int32 BlockId { get; set; }
        public virtual Block ReferencedBlock { get; set; }
        public string CaptionsString { get; set; }
    }
}
