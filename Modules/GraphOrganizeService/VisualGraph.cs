using System.Collections.Generic;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class VisualGraph : IVisualGraph
    {
        private IList<IVisualGraphElem> _blocks;
        public IList<IVisualGraphElem> GetGraphElems()
        {
            return _blocks;
        }

        public void SetBlocks(IList<IVisualGraphElem> blocks)
        {
            _blocks = blocks;
        }
    }
}