using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using EF;
using MemOrg.Interfaces;

namespace GraphService
{
    public class GraphService : IGraphService
    {
        readonly IBlockRepository _blockRepository;

        private List<Block> _blocks;

        public GraphService(IBlockRepository blockRepository)
        {
            this._blockRepository = blockRepository;
        }

        public IList<Block> Blocks
        {
            get { return _blocks ?? (_blocks = LoadGraph()); }
        }

        List<Block> LoadGraph()
        {
            return _blockRepository.All.ToList();
        }
    }
}
