using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class VisualGrid : IVisualGrid
    {
        private readonly IGrid _grid;

        public VisualGrid(IGrid grid)
        {
            _grid = grid;
        }
        
        public void Prerender(IDrawer drawer)
        {
            
        }
    }
}