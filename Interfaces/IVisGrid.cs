namespace MemOrg.Interfaces
{
    public interface IVisGrid : IGrid
    {
        void SetColumnWidthInStars(int col, double width);
        void SetRowHeightInStars(int col, double width);
    }
}