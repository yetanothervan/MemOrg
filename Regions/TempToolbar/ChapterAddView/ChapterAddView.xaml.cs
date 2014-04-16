using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EF;

namespace TempToolbar.ChapterAddView
{
    public partial class ChapterAddView : Window
    {
        public ChapterAddView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                var dc = new ChapterAddViewModel();
                dc.PropertyChanged += (o, eventArgs) =>
                {
                    if (eventArgs.PropertyName == "CanClose")
                    {
                        if (dc.CanClose)
                        {
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                };
                DataContext = dc;
            };
        }

        public string GetBookName()
        {
            var dc = DataContext as ChapterAddViewModel;
            return dc == null ? null : dc.Book;
        }

        public string GetChapterCaption()
        {
            var dc = DataContext as ChapterAddViewModel;
            return dc == null ? null : dc.ChapterCaption;
        }

        public int? GetChapterNumber()
        {
            var dc = DataContext as ChapterAddViewModel;
            return dc == null ? null : dc.ChapterNumber;
        }
    }
}
