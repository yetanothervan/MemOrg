using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDrawService.Draw;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

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

        public IComponent DrawCaption(string text)
        {
            return new Caption(text, _style);
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

        public IComponent DrawTree(IGridElem gridElem)
        {
            return new Tree(_style, gridElem);
        }

        public IComponent DrawBlockOthers(IGridElem gridElem)
        {
            return new BlockRectangle(_style.OthersBlockBrush, _style.OthersBlockPen, gridElem);
        }

        public IComponent DrawBlockOthersNoParticles(IGridElem gridElem)
        {
            return new BlockRectangle(_style.OthersBlockNoParticlesBrush, _style.OthersBlockNoParticlesPen, gridElem);
        }

        public IComponent DrawBlockSource(IGridElem gridElem)
        {
            return new BlockRectangle(_style.SourceBlockBrush, _style.SourceBlockPen, gridElem);
        }

        public IComponent DrawBlockRelation(IGridElem gridElem)
        {
            return new BlockRectangle(_style.RelationBlockBrush, _style.RelationBlockPen, gridElem);
        }

        public IComponent DrawBlockTag(IGridElem gridElem)
        {
            return new BlockRoundedRectangle(_style.TagBlockBrush, _style.TagBlockPen, gridElem);
        }

        public IComponent DrawBlockUserText(IGridElem gridElem)
        {
            return new BlockRectangle(_style.UserTextBlockBrush, _style.UserTextBlockPen, gridElem);
        }

        public IComponent DrawTag(IGridElem gridElem)
        {
            return new BlockRectangle(_style.TagBlockBrush, _style.TagBlockPen, gridElem);
        }
    }
}
