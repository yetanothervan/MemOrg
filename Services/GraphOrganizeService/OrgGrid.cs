using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;
using Microsoft.Practices.Unity.Utility;

namespace GraphOrganizeService
{
    public class OrgGrid : Grid, IOrgGrid
    {
        public OrgGrid()
        {
            ColStarWidth = new Dictionary<int, double>();
            RowStarHeight = new Dictionary<int, double>();
        }
        public IDictionary<int, double> ColStarWidth { get; set; }
        public IDictionary<int, double> RowStarHeight { get; set; }
    }
}
