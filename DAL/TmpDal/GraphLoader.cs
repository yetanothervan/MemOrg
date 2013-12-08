using DAL.Entity;

namespace DAL.TmpDal
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
