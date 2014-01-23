using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
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
        private VisualGraph _visualGraph;

        public IVisualGraph ProcessGraph(IGraph graph)
        {
            return _visualGraph ?? (_visualGraph =
                SnapGraphToGrid.SnapGraph(
                    VisaulGraphProcessing.CreateVisualGraph(graph)
                    ));
        }

        /*private IVisualGraph GenerateGraph(IList<Block> blocks)
        {
            var planeBlocks = new List<IVisualGridElem>();
            const double sp = 10;
            double x = 0;
            var layout = new Layout {X = 0, Width = 0, Y = 0, Height = 0};

            foreach (var b in blocks)
            {
                x += sp;
                var pb = new VisualGridBlock(b, 200);
                pb.P1 = new Point(x, sp);
                x += pb.TextWidth + 2*sp;
                pb.P2 = new Point(x, pb.TextHeight + 3*sp);
                planeBlocks.Add(pb);

                layout.X = Math.Min(layout.X, pb.P1.X);
                layout.Y = Math.Min(layout.Y, pb.P1.Y);
                layout.Width = Math.Max(layout.Width, pb.P2.X);
                layout.Height = Math.Max(layout.Height, pb.P2.Y);
        
                 //for (var i = 1; i < 100; ++i)
                 //{
                 //    var morePb = new PlaneBlock(b, 200);
                 //    morePb.P1 = new Point(pb.P1.X, pb.P2.Y + (pb.TextHeight + 3*sp)*(i - 1));
                 //    morePb.P2 = new Point(pb.P2.X, pb.P2.Y + (pb.TextHeight + 3*sp)*i);
                 //    planeBlocks.Add(morePb);
                 //}
            }
            var graph = new VisualGraph();
            graph.SetBlocks(planeBlocks, layout);
            return graph;
        }*/
    }
}
