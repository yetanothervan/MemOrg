using System.Windows.Media;

namespace MemOrg.Interfaces
{
    public interface IGraphDrawService
    {
        IDrawer GetDrawer(IDrawStyle style);
        IDrawStyle GetStyle();
    }
}
