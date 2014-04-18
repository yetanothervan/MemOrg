using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ChapterViewer
{
    public class ParticleParagraph : Paragraph
    {
        public ParticleParagraph(Inline content) : base(content)
        {
            IsSelected = false;
            Over = false;
            this.BorderThickness = new Thickness(1);
            this.MouseEnter += (sender, args) => UpdateView(true);
            this.MouseLeave += (sender, args) => UpdateView(false);
        }

        public bool Over { get; private set; }

        void UpdateView(bool over)
        {
            Over = over;
            if (IsSelected)
                this.BorderBrush = Brushes.Red;
            else
                this.BorderBrush = over 
                    ? Brushes.LightCoral 
                    : Brushes.Transparent;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                UpdateView(value);
            }
        }
    }
}
