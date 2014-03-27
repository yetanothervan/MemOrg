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
        public virtual IEnumerable<IComponent> Childs
        {
            get { return _childs ?? (_childs = new List<IComponent>(0)); }
        }

        public virtual void AddChild(IComponent child)
        {
            (_childs ?? (_childs = new List<IComponent>(0))).Add(child);
        }

        public abstract List<DrawingVisual> Render(Point p);
        public abstract Size GetActualSize();
        public virtual Size? PreferSize { get; set; }
    }
}
