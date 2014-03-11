using MemOrg.Interfaces.GridElems;

namespace MemOrg.Interfaces
{
    public interface IDrawer
    {
        IComponent DrawGrid();
        IComponent DrawCaption(string text);
        IComponent DrawBlockOthers(IGridElem gridElem);
        IComponent DrawBlockOthersNoParticles(IGridElem gridElem);
        IComponent DrawBlockSource(IGridElem gridElem);
        IComponent DrawBlockRelation(IGridElem gridElem);
        IComponent DrawBlockTag(IGridElem gridElem);
        IComponent DrawBlockUserText(IGridElem gridElem);
        IComponent DrawTag(IGridElem gridElem);
        IComponent DrawQuoteText(string text);
        IComponent DrawQuoteBox();
        IComponent DrawStackBox();
        IComponent DrawTree(IGridElem gridElem);
        IComponent DrawLink(GridLink gridLink);
    }
}