using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChapterViewer.CreatePointBlock;
using DAL.Entity;
using Block = DAL.Entity.Block;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace ChapterViewer.BlockPointOutDlg
{
    /// <summary>
    /// Логика взаимодействия для BlockPointOutView.xaml
    /// </summary>
    public partial class BlockPointOutView : Window
    {

// ReSharper disable once InconsistentNaming
        private const int GWL_STYLE = -16;
// ReSharper disable once InconsistentNaming
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private readonly CreatePointBlockViewModel _blockDlg;

        public BlockPointOutView(Particle particle, int startSelection, int selectionLength)
        {
            StartSelection = startSelection;
            SelectionLength = selectionLength;
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            };
            MyParticle = particle;
            _blockDlg = new CreatePointBlockViewModel();
            BlockSelectDlg.DataContext = _blockDlg;
            _blockDlg.CaptionView = "Укажите блок";

        }

        public bool IsCreateNew
        {
            get { return _blockDlg.IsCreateNew; }
        }

        public string Caption
        {
            get { return _blockDlg.CaptionBlock; }
        }

        public Block MyBlock
        {
            get { return _blockDlg.MyBlock; }
            set { _blockDlg.MyBlock = value; }
        }

        public BlockPointOutViewResult? Result { get; set; }
        public Particle MyParticle { get; set; }
        public int StartSelection { get; set; }
        public int SelectionLength { get; set; }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (_blockDlg.IsCreateNew && string.IsNullOrEmpty(_blockDlg.CaptionBlock))
            {
                MessageBox.Show("Задайте имя создаваемого блока");
                return;
            }
            if (!_blockDlg.IsCreateNew && _blockDlg.MyBlock == null)
            {
                MessageBox.Show("Выберите блок или создайте новый");
                return;
            }
            Result = BlockPointOutViewResult.Ok;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = BlockPointOutViewResult.Cancel;
            Close();
        }

        private void OnBlockPointed(object sender, ExecutedRoutedEventArgs e)
        {
            Result = BlockPointOutViewResult.SelectBlock;
            Hide();
        }
    }

    public enum BlockPointOutViewResult
    {
        SelectBlock,
        Ok,
        Cancel
    }
}
