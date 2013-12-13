using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class GraphOrganizeService : IGraphOrganizeService
    {
        private IPlanarGraph _planarGraph;

        public IPlanarGraph MakePlanarGraph(IList<Block> blocks)
        {
            return _planarGraph ?? (_planarGraph = GenerateGraph(blocks));
        }

        protected IPlanarGraph GenerateGraph(IList<Block> blocks)
        {
            var planeBlocks = new List<IPlanarGraphBlock>();
            const double sp = 10;
            double x = 0;
            var layout = new GraphLayout {X1 = 0, X2 = 0, Y1 = 0, Y2 = 0};

            foreach (var b in blocks)
            {
                x += sp;
                var pb = new PlanarGraphBlock(b, 200);
                pb.P1 = new Point(x, sp);
                x += pb.TextWidth + 2*sp;
                pb.P2 = new Point(x, pb.TextHeight + 3*sp);
                planeBlocks.Add(pb);

                layout.X1 = Math.Min(layout.X1, pb.P1.X);
                layout.Y1 = Math.Min(layout.Y1, pb.P1.Y);
                layout.X2 = Math.Max(layout.X2, pb.P2.X);
                layout.Y2 = Math.Max(layout.Y2, pb.P2.Y);

                /* for (var i = 1; i < 100; ++i)
                 {
                     var morePb = new PlaneBlock(b, 200);
                     morePb.P1 = new Point(pb.P1.X, pb.P2.Y + (pb.TextHeight + 3*sp)*(i - 1));
                     morePb.P2 = new Point(pb.P2.X, pb.P2.Y + (pb.TextHeight + 3*sp)*i);
                     planeBlocks.Add(morePb);
                 }*/
            }
            var graph = new PlanarGraph();
            graph.SetBlocks(planeBlocks, layout);
            return graph;
        }
    }
}
