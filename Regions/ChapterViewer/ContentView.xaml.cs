using System;
using System.Windows;
using System.Windows.Controls;

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
        }

        private void RtfOnSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var dc = DataContext as ContentViewModel;
            if (dc != null)
            {
                if (!Equals(Rtf.CaretPosition.Paragraph, dc.CurrentParagpaph))
                    Rtf.IsReadOnly = true;
            }
        }
    }
}
