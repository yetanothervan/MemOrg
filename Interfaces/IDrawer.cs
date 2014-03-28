using System.Collections.Generic;
using MemOrg.Interfaces.OrgUnits;

namespace MemOrg.Interfaces
{
    public interface IDrawer
    {
        IComponent DrawGrid(IDictionary<int, double> colStarWidth, 
            IDictionary<int, double> rowStarHeight);
        IComponent DrawGridElem(IOrgGridElem orgElem);
        IComponent DrawGridElem(int row, int col);
        IComponent DrawCaption(string text);
        IComponent DrawBacking();
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
        IComponent DrawLink(IReadOnlyList<GridLinkPart> gridLinkParts);
    }
}