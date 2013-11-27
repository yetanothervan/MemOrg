using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Models;

namespace Graph2Plane
{
    public class PlaneGraph
    {
        public PlaneGraph(Graph gr)
        {
            _planeBlocks = new List<PlaneBlock>();
            double d = 0;
            foreach (var pb in gr.Blocks.Select(block => 
                new PlaneBlock(block) 
                {
                    P1 = new Point(d += 100.0, 0.0),
                    P2 = new Point(d + 90.0, 40.0)
                }))
                _planeBlocks.Add(pb);
        }

        public List<PlaneBlock> GetPlainBlocks()
        {
            return _planeBlocks;
        }

        private readonly List<PlaneBlock> _planeBlocks;
    }
}
