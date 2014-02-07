using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphService
{
    public class Page : IPage
    {
        public Block Block { get; set; }
        public Tag Tag { get; set; }
        public bool IsBlockTag { get; set; }
    }
}