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

namespace ChapterViewer
{
    public class ParticleParagraph : Table
    {
        public ParticleParagraph(Inline content)
        {
            Columns.Add(new TableColumn {Name = "Content", Width = new GridLength(1, GridUnitType.Star)});
            Columns.Add(new TableColumn { Name = "Toolbar", Width = new GridLength(20, GridUnitType.Pixel) });
            RowGroups.Add(new TableRowGroup());
            var currentRow = new TableRow();
            RowGroups[0].Rows.Add(currentRow);

            var toolbar = new BlockUIContainer();
            var editButton = new Button {Content = "Edit..."};
            toolbar.Child = editButton;

            currentRow.Cells.Add(new TableCell(toolbar));
            currentRow.Cells.Add(new TableCell(new Paragraph(content)));
            
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
            {
                this.BorderBrush = Brushes.Red;
            }
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
