using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

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
                Rtf.SelectionChanged += RtfOnSelectionChanged;
            };
            CurParticleTextBox.TextChanged += (sender, args) =>
            {
                var dc = DataContext as ContentViewModel;
                if (dc != null) dc.TextChanged = true;
            };
            CurParticleTextBox.KeyDown += (sender, args) =>
            {
                if (Keyboard.Modifiers == ModifierKeys.Control && args.Key == Key.S)
                {
                    var dc = DataContext as ContentViewModel;
                    if (dc != null) dc.SaveCommand.Execute();
                }
                if (args.Key == Key.Escape)
                {
                    var dc = DataContext as ContentViewModel;
                    if (dc != null)
                    {
                        dc.DiscardCommand.Execute();
                        dc.CloseEditingCommand.Execute();
                    }
                }
            };
        }

        private void RtfOnSelectionChanged(object sender, RoutedEventArgs args)
        {
            var dc = DataContext as ContentViewModel;
            if (dc == null) return;
            
            if (Rtf.Selection == null || Rtf.Selection.IsEmpty ||
                Rtf.Selection.Start.Paragraph == null || Rtf.Selection.End.Paragraph == null
                || !Equals(Rtf.Selection.Start.Paragraph, Rtf.Selection.End.Paragraph))
                dc.SetParagraphSelection(null);
            else
                dc.SetParagraphSelection(Rtf.Selection);
        }
    }
}
