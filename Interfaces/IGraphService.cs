using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace Interfaces
{
    public interface IGraphService
    {
        IList<Block> Blocks { get; }
    }
}
