using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class Grid : IGrid, IEnumerable<IGridElem>
    {
        private readonly List<List<IGridElem>> _elems;

        public Grid(IGraph graph, IGridLayout layout)
        {
            _elems = layout.DoLayout(graph);
        }

        public int GetRowsCount { get { return _elems.Count; } }
        public int GetRowLength { get { return _elems.Count == 0 ? 0 : _elems[0].Count; } }
        
        public IEnumerator<IGridElem> GetEnumerator()
        {
            return new GridEnumerator(_elems);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
