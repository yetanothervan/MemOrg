using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public interface IBlockRepository
    {
        IQueryable<Block> All { get; }
    }
}
