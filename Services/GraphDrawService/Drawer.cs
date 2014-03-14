using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDrawService.Draw;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;
using Grid = GraphDrawService.Draw.Grid;
using GridElem = GraphDrawService.Draw.GridElem;

namespace GraphDrawService
{
    public class Drawer : IDrawer
    {
        private readonly IDrawStyle _style;
        public Drawer(IDrawStyle style)
        {
            _style = style;
        }

        public IComponent DrawGrid()
        {
            return new Grid(_style);
        }

        public IComponent DrawGridElem(int row, int col)
        {
            return new GridElem(row, col);
        }

        public IComponent DrawCaption(string text)
        {
            return new Caption(text, _style);
        }

        public IComponent DrawBacking()
        {
            return new Backing(_style);
        }

        public IComponent DrawQuoteText(string text)
        {
            return new Text(text, _style);
        }

        public IComponent DrawQuoteBox()
        {
            return new QuoteBlock(_style);
        }

        public IComponent DrawStackBox()
        {
            return new StackBox();
        }

        public IComponent DrawTree()
        {
            return new Tree(_style);
        }

        public IComponent DrawLink()
        {
            return new Link(_style.SourceBlockPen);
        }

        public IComponent DrawBlockOthers()
        {
            return new BlockRectangle(_style.OthersBlockBrush, _style.OthersBlockPen);
        }

        public IComponent DrawBlockOthersNoParticles()
        {
            return new BlockRectangle(_style.OthersBlockNoParticlesBrush, _style.OthersBlockNoParticlesPen);
        }

        public IComponent DrawBlockSource()
        {
            return new BlockRectangle(_style.SourceBlockBrush, _style.SourceBlockPen);
        }

        public IComponent DrawBlockRelation()
        {
            return new BlockRectangle(_style.RelationBlockBrush, _style.RelationBlockPen);
        }

        public IComponent DrawBlockTag()
        {
            return new BlockRoundedRectangle(_style.TagBlockBrush, _style.TagBlockPen);
        }

        public IComponent DrawBlockUserText()
        {
            return new BlockRectangle(_style.UserTextBlockBrush, _style.UserTextBlockPen);
        }

        public IComponent DrawTag()
        {
            return new BlockRectangle(_style.TagBlockBrush, _style.TagBlockPen);
        }
    }
}
