using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TmpDal
{
    public class GraphLoader
    {
        public Graph GetGraph()
        {
            var result = new Graph {Blocks = new BlocksRepository().GetBlocks()};
            return result;
        }
    }
}
