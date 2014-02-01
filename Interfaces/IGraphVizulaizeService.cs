namespace MemOrg.Interfaces
{
    public interface IGraphVizulaizeService
    {
        IVisualizeOptions GetVisualizeOptions();
        IComponent VisualizeGrid(IGrid grid, IDrawer drawer);
    }
}