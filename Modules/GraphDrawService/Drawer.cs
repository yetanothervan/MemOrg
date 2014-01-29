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
        public IComponent DrawGrid()
        {
            return new Grid();
        }

        public IComponent DrawCaption(string text)
        {
            return new Caption();
        }

        public IComponent DrawBox()
        {
            return new Block();
        }

        public IComponent DrawQuoteText(string text)
        {
            return new Text();
        }

        public IComponent DrawQuoteBox()
        {
            return new QuoteBlock();
        }
    }
}
