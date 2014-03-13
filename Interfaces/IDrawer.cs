using MemOrg.Interfaces.OrgUnits;

namespace MemOrg.Interfaces
{
    public interface IDrawer
    {
        IComponent DrawGrid();
        IComponent DrawCaption(string text);
        IComponent DrawBlockOthers();
        IComponent DrawBlockOthersNoParticles();
        IComponent DrawBlockSource();
        IComponent DrawBlockRelation();
        IComponent DrawBlockTag();
        IComponent DrawBlockUserText();
        IComponent DrawTag();
        IComponent DrawQuoteText(string text);
        IComponent DrawQuoteBox();
        IComponent DrawStackBox();
        IComponent DrawTree();
        IComponent DrawLink(GridLink gridLink);
    }
}