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

        private bool _isEditing;
        private readonly DelegateCommand _editCommand;

        private bool _isChanged;
        private readonly DelegateCommand _saveCommand;

        private readonly DelegateCommand _discardCommand;

        public ParticleParagraph(Inline content)
        {
            Over = false;
            _isSelected = false;
            _isEditing = false;
            _editCommand = new DelegateCommand(() => Edit(true), () => !_isEditing);

            _isChanged = false;
            _saveCommand = new DelegateCommand(Save, () => _isChanged);

            _discardCommand = new DelegateCommand(Discard, () => _isChanged);
            MyContent = new Paragraph(content);
            CreateToolbar();

            Selected(false);
            Over = false;
            
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

        private void Discard()
        {
            _isChanged = false;
            _discardCommand.RaiseCanExecuteChanged();
            _saveCommand.RaiseCanExecuteChanged();
        }

        private void Save()
        {
            _isChanged = false;
            _discardCommand.RaiseCanExecuteChanged();
            _saveCommand.RaiseCanExecuteChanged();
        }

        private void Edit(bool value)
        {
            RichTextBox parent = null;
            var fd = Parent as FlowDocument;
            if (fd != null) parent = fd.Parent as RichTextBox;

            if (parent != null)
            {
                _isEditing = value;
                parent.IsReadOnly = !value;
                if (value) parent.Selection.Select(MyContent.ContentStart, MyContent.ContentStart);
                UpdateView();
                _editCommand.RaiseCanExecuteChanged();
            }
        }

        public bool Over { get; private set; }

        void UpdateView()
        {
            if (IsSelected)
            {
                this.BorderBrush = Brushes.Red;
                this.BorderThickness = new Thickness(2);
                this.Background = _isEditing
                    ? Brushes.Yellow
                    : Brushes.Transparent;
            }
            else
            {
                this.Background = _isChanged
                        ? Brushes.Red
                        : Brushes.Transparent;

                this.BorderThickness = new Thickness(1);
                this.BorderBrush = Over
                    ? Brushes.Red
                    : Brushes.Transparent;
            }
        }

        void Selected(bool isSelected)
        {
            this.Columns.First(c => c.Name == "Toolbar").Width = isSelected 
                ? new GridLength(40, GridUnitType.Pixel) 
                : new GridLength(0, GridUnitType.Pixel);

            if (!isSelected && _isEditing)
                Edit(false);

            UpdateView();
        }

        private void CreateToolbar()
        {
            Columns.Add(new TableColumn { Name = "Toolbar", Width = new GridLength(40, GridUnitType.Pixel) });
            Columns.Add(new TableColumn { Name = "Content" });

            RowGroups.Add(new TableRowGroup());
            var currentRow = new TableRow();
            RowGroups[0].Rows.Add(currentRow);
            
            var toolbarCell = new TableCell();
            currentRow.Cells.Add(toolbarCell);
            currentRow.Cells.Add(new TableCell(MyContent));

            var toolbar = new BlockUIContainer();

            var stackPanel = new StackPanel();

            var editButton = new Button {Content = "Edit", Command = _editCommand};
            var saveButton = new Button {Content = "Save", Command = _saveCommand};
            var disButton = new Button { Content = "Dis", Command = _discardCommand };

            stackPanel.Children.Add(editButton);
            stackPanel.Children.Add(saveButton);
            stackPanel.Children.Add(disButton);

            toolbar.Child = stackPanel;
            toolbarCell.Blocks.Add(toolbar);
        }

        public void TextChanged()
        {
            if (_isChanged) return;
            
            _isChanged = true;
            UpdateView();
            _discardCommand.RaiseCanExecuteChanged();
            _saveCommand.RaiseCanExecuteChanged();
        }

        private bool _isSelected;
        
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                Selected(value);
            }
        }
    }
}
