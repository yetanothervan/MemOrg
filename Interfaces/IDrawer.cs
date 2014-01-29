namespace MemOrg.Interfaces
{
    public interface IDrawer
    {
        IComponent DrawGrid();
        IComponent DrawCaption(string text);
        IComponent DrawBox();
        IComponent DrawQuoteText(string text);
        IComponent DrawQuoteBox();
    }
}