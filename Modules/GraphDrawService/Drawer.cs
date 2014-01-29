using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDrawService.Draw;
using MemOrg.Interfaces;

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
            return new Grid();
        }

        public IComponent DrawCaption(string text)
        {
            return new Caption(text, _style);
        }

        public IComponent DrawBox()
        {
            return new Block(_style);
        }

        public IComponent DrawQuoteText(string text)
        {
            return new Text(text, _style);
        }

        public IComponent DrawQuoteBox()
        {
            return new QuoteBlock(_style);
        }
    }
}
