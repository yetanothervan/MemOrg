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
        private readonly List<GridLink> _links;

        public OrgGrid()
        {
            _links = new List<GridLink>();
        }
        
        public List<GridLink> Links
        {
            get { return _links; }
        }

        public void AddLink(int fromRow, int fromCol, NESW cpb, int toRow, int toCol, NESW cpe)
        {
            var link = new GridLink
            {
                Begin = new GridLinkPoint {Col = fromCol, ConnectionPoint = cpb, Row = fromRow},
                End = new GridLinkPoint {Col = toCol, ConnectionPoint = cpe, Row = toRow}
            };
            _links.Add(link);
        }
    }
}
