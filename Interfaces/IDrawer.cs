namespace MemOrg.Interfaces
{
    public interface IDrawer
    {
        IComponent DrawGrid();
        IComponent DrawCaption(string text);
        IComponent DrawBox(IGridElem gridElem);
        IComponent DrawSourceBox();
        IComponent DrawQuoteText(string text);
        IComponent DrawQuoteBox();
    }
}