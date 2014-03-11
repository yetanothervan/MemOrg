using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    public abstract class Component : IComponent
    {
        private List<IComponent> _childs;
        public List<IComponent> Childs
        {
            get
            {
                if (_childs == null) Childs = new List<IComponent>(0);
                return _childs;
            }
            set { _childs = value; }
        }

        public abstract List<DrawingVisual> Render(Point p);
        public abstract Size GetSize();
    }
}
