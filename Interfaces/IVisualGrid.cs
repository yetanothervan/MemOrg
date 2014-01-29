namespace MemOrg.Interfaces
{
    public interface IVisualGrid : IGrid
    {
        void Prerender(IDrawer drawer);
    }
}