namespace MemOrg.Interfaces
{
    public interface IGraphVizulaizeService
    {
        IVisualizeOptions GetVisualizeOptions();
        IComponent VisualizeGrid(IGrid grid, IVisualizeOptions options, IDrawer drawer);
    }
}