using System.Collections.Generic;
using System.Windows;
using DAL.Entity;

namespace BL.Graph2Plane
{
    public class PlaneGraph
    {
        public PlaneGraph()
        {
            _planeBlocks = new List<PlaneBlock>();
        }

        public PlaneGraph(IEnumerable<Block> blocks)
        {
            _planeBlocks = new List<PlaneBlock>();
            const double sp = 10;
            double x = 0;
            foreach (var b in blocks)
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
