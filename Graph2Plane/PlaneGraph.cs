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
            const double sp = 10;
            double x = 0;
            foreach (var b in gr.Blocks)
            {
                x += sp;
                var pb = new PlaneBlock(b, 200);
                pb.P1 = new Point(x, sp);
                x += pb.TextWidth + 2 * sp;
                pb.P2 = new Point(x, pb.TextHeight + 3 * sp);
                _planeBlocks.Add(pb);

                for (var i = 1; i < 100; ++i)
                {
                    var morePb = new PlaneBlock(b, 200);
                    morePb.P1 = new Point(pb.P1.X, pb.P2.Y + (pb.TextHeight + 3 * sp) * (i - 1));
                    morePb.P2 = new Point(pb.P2.X, pb.P2.Y + (pb.TextHeight + 3 * sp) * i);
                    _planeBlocks.Add(morePb);
                }
            }
        }

        public List<PlaneBlock> GetPlainBlocks()
        {
            return _planeBlocks;
        }

        private readonly List<PlaneBlock> _planeBlocks;
    }
}
