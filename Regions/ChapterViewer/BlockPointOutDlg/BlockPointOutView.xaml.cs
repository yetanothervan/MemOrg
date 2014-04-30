using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChapterViewer.BlockPointOutDlg
{
    /// <summary>
    /// Логика взаимодействия для BlockPointOutView.xaml
    /// </summary>
    public partial class BlockPointOutView : Window
    {
        public BlockPointOutView()
        {
            InitializeComponent();
        }

        private void Button_Create(object sender, RoutedEventArgs e)
        {
            BlockPointOutResult = BlockPointOutDlg.BlockPointOutResult.Create;
            if (String.IsNullOrEmpty(Caption))
                MessageBox.Show("Введите название");
            else
                Close();
        }

        private void Button_Point(object sender, RoutedEventArgs e)
        {
            BlockPointOutResult = BlockPointOutDlg.BlockPointOutResult.Point;
            Close();
        }

        private void Caption_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = e.Source as TextBox;
            if (textBox != null) Caption = textBox.Text;
        }

        public string Caption { get; set; }
        public BlockPointOutResult? BlockPointOutResult { get; set; }
    }

    public enum BlockPointOutResult
    {
        Create, 
        Point
    }
}
