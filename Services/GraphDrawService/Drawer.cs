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

        public IComponent DrawBlockOthers(IGridElem gridElem)
        {
            return new BlockOthers(_style, gridElem);
        }

        public IComponent DrawBlockSource(IGridElem gridElem)
        {
            return new BlockSource(_style, gridElem);
        }

        public IComponent DrawBlockRelation(IGridElem gridElem)
        {
            return new BlockRelation(_style, gridElem);
        }

        public IComponent DrawBlockTag(IGridElem gridElem)
        {
            return new BlockTag(_style, gridElem);
        }

        public IComponent DrawBlockUserText(IGridElem gridElem)
        {
            return new BlockUserText(_style, gridElem);
        }

        public IComponent DrawTag(IGridElem gridElem)
        {
            return new TagBox(_style, gridElem);
        }
    }
}
