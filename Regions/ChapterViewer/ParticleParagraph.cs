using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;

namespace ChapterViewer
{
    public class ParticleParagraph : Paragraph
    {
        private bool _isEditing;
        private readonly DelegateCommand _editCommand;

        private bool _isChanged;
        private readonly DelegateCommand _saveCommand;

        private readonly DelegateCommand _discardCommand;

        public ParticleParagraph(Inline content) : base(content)
        {
            Over = false;
            _isSelected = false;
            UpdateView();

            this.BorderThickness = new Thickness(1);
            
            this.MouseEnter += (sender, args) =>
            {
                if (Over) return;
                Over = true;
                UpdateView();
            };
            
            this.MouseLeave += (sender, args) =>
            {
                if (Over == false) return;
                Over = false;
                UpdateView();
            };
        }
        

        public bool Over { get; private set; }

        void UpdateView()
        {
            if (IsSelected)
            {
                this.BorderBrush = Brushes.Red;
                this.BorderThickness = new Thickness(2);
            }
            else
            {
                this.BorderThickness = new Thickness(1);
                this.BorderBrush = Over
                    ? Brushes.Red
                    : Brushes.Transparent;
            }
        }
        
        private bool _isSelected;
        
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                UpdateView();
            }
        }
    }
}
