using System.Collections.Generic;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class PlanarGraph : IPlanarGraph
    {
        private IList<IPlanarGraphBlock> _blocks;
        private GraphLayout _layout;

        public IList<IPlanarGraphBlock> GetBlocks()
        {
            return _blocks;
        }

        public GraphLayout GetGraphLayout()
        {
            return _layout;
        }

        public void SetBlocks(IList<IPlanarGraphBlock> blocks, GraphLayout layout)
        {
            _blocks = blocks;
            _layout = layout;
        }
    }
}