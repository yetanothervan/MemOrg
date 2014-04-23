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
    public class ParticleParagraph : Table
    {
        public readonly Paragraph MyContent;
        private readonly TableCell _toolbarCell;
        private BlockUIContainer _toolbar;

        private bool _isEditing;
        private readonly DelegateCommand _editCommand;

        private bool _isChanged;
        private readonly DelegateCommand _saveCommand;

        public ParticleParagraph(Inline content)
        {
            _isEditing = false;
            _editCommand = new DelegateCommand(Edit, () => !_isEditing);

            _isChanged = false;
            _saveCommand = new DelegateCommand(Save, () => _isChanged);

            Columns.Add(new TableColumn { Name = "Toolbar", Width = new GridLength(40, GridUnitType.Pixel)});
            Columns.Add(new TableColumn { Name = "Content"});
            
            RowGroups.Add(new TableRowGroup());
            var currentRow = new TableRow();
            RowGroups[0].Rows.Add(currentRow);
            
            _toolbarCell = new TableCell();

            MyContent = new Paragraph(content);
            currentRow.Cells.Add(_toolbarCell);
            currentRow.Cells.Add(new TableCell(MyContent));
            
            IsSelected = false;
            Over = false;
            this.BorderThickness = new Thickness(1);
            this.MouseEnter += (sender, args) => UpdateView(true);
            this.MouseLeave += (sender, args) => UpdateView(false);
        }

        private void Save()
        {
            _isChanged = false;
        }

        private void Edit()
        {
            RichTextBox parent = null;
            var fd = Parent as FlowDocument;
            if (fd != null) parent = fd.Parent as RichTextBox;

            if (parent != null)
            {
                _isEditing = true;
                parent.IsReadOnly = false;
                UpdateView(true);
                _editCommand.RaiseCanExecuteChanged();
            }
        }

        public bool Over { get; private set; }

        void UpdateView(bool over)
        {
            Over = over;
            if (IsSelected)
            {
                this.BorderBrush = Brushes.Red;
                this.Background = _isEditing 
                    ? Brushes.White 
                    : Brushes.Transparent;
            }
            else
                this.BorderBrush = over
                    ? Brushes.LightCoral
                    : Brushes.Transparent;
        }

        void Selected(bool isSelected)
        {
            if (_toolbar == null)
            {
                _toolbar = new BlockUIContainer();

                var stackPanel = new StackPanel();

                var editButton = new Button {Content = "Edit", Command = _editCommand};
                var saveButton = new Button {Content = "Save", Command = _saveCommand};

                stackPanel.Children.Add(editButton);
                stackPanel.Children.Add(saveButton);

                _toolbar.Child = stackPanel;
                _toolbarCell.Blocks.Add(_toolbar);
            }
            this.Columns.First(c => c.Name == "Toolbar").Width = isSelected 
                ? new GridLength(40, GridUnitType.Pixel) 
                : new GridLength(0, GridUnitType.Pixel);
        }
        
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                Selected(value);
                UpdateView(value);
            }
        }
    }
}
