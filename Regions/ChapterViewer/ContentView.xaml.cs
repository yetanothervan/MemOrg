using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ChapterViewer
{
    /// <summary>
    /// Логика взаимодействия для ContentView.xaml
    /// </summary>
    public partial class ContentView : UserControl
    {
        public ContentView(ContentViewModel viewModel)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                viewModel.PropertyChanged += (o, eventArgs) =>
                {
                    var dc = DataContext as ContentViewModel;
                    if (dc != null && eventArgs.PropertyName == "Document")
                        Rtf.Document = dc.Document;
                };
                DataContext = viewModel; 
            };
            Rtf.SelectionChanged += RtfOnSelectionChanged;
            Rtf.TextChanged += RtfOnTextChanged;
        }

        private void RtfOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var dc = DataContext as ContentViewModel;
            if (dc == null || dc.CurrentParagpaph == null) return;
            dc.CurrentParagpaph.TextChanged();
        }

        private void RtfOnSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var dc = DataContext as ContentViewModel;
            if (dc == null) return;

            if (Rtf.CaretPosition.Paragraph != null)
            {
                if (!Equals(Rtf.CaretPosition.Paragraph, dc.CurrentParagraphContent))
                    ParagraphChanged(dc);
            }
            else
            {
                var myCell = (TableCell) dc.CurrentParagraphContent.Parent;
                var myRow = myCell != null ? myCell.Parent : null;

                if (Rtf.Selection.IsEmpty)
                {
                    if (!Equals(Rtf.CaretPosition.Parent, myRow))
                        ParagraphChanged(dc);
                }
                else if (
                    !(Equals(Rtf.Selection.Start.Paragraph, dc.CurrentParagraphContent)
                    || Equals(Rtf.Selection.Start.Parent, myRow)) ||
                    !(Equals(Rtf.Selection.End.Paragraph, dc.CurrentParagraphContent)
                    || Equals(Rtf.Selection.End.Parent, myRow))
                    )
                    ParagraphChanged(dc);
            }
        }

        void ParagraphChanged(ContentViewModel dc)
        {
            Rtf.IsReadOnly = true;
            dc.ParagraphBlur();
        }
    }
}
