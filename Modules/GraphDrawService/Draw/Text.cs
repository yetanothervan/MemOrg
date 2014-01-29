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
    class Text : IComponent
    {
        private string _text;
        
        public Text(string text)
        {
            _text = text;
        }

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

        public List<DrawingVisual> Render()
        {
            throw new NotImplementedException();
        }

        public Size GetSize()
        {
            throw new NotImplementedException();
        }
    }
}
