using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using ChapterViewer.CreatePointBlock;
using DAL.Entity;

namespace ChapterViewer.AddRelDlg
{
    /// <summary>
    /// Логика взаимодействия для AddRelView.xaml
    /// </summary>
    public partial class AddRelView : Window
    {
        private readonly CreatePointBlockViewModel _blockFirstDlg;
        private readonly CreatePointBlockViewModel _blockSecondDlg;

// ReSharper disable once InconsistentNaming
        private const int GWL_STYLE = -16;
// ReSharper disable once InconsistentNaming
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public AddRelView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

                DataContext = new AddRelViewModel();
            };

            InitializeComponent();

            _blockFirstDlg = new CreatePointBlockViewModel();
            BlockFirstSelectDlg.DataContext = _blockFirstDlg;
            _blockFirstDlg.CaptionView = "Укажите первый блок";

            _blockSecondDlg = new CreatePointBlockViewModel();
            BlockSecondSelectDlg.DataContext = _blockSecondDlg;
            _blockSecondDlg.CaptionView = "Укажите второй блок";
        }

        public AddRelDlgResult? Result { get; set; }
        public bool Selection { get; set; }
        public int StartSelection { get; set; }
        public int SelectionLength { get; set; }
        public ParticleParagraph MyParticle { get; set; }

        public bool IsFirstCreateNew
        {
            get { return _blockFirstDlg.IsCreateNew; }
        }

        public bool IsSecondCreateNew
        {
            get { return _blockSecondDlg.IsCreateNew; }
        }

        public string CaptionFirst
        {
            get { return _blockFirstDlg.CaptionBlock; }
        }

        public string CaptionSecond
        {
            get { return _blockSecondDlg.CaptionBlock; }
        }

        public Block BlockFirst
        {
            get { return _blockFirstDlg.MyBlock; }
            set { _blockFirstDlg.MyBlock = value; }
        }

        public Block BlockSecond
        {
            get { return _blockSecondDlg.MyBlock; }
            set { _blockSecondDlg.MyBlock = value; }
        }

        public string RelType
        {
            get
            {
                var addRelViewModel = DataContext as AddRelViewModel;
                if (addRelViewModel != null) return addRelViewModel.RelType;
                return null;
            }
        }

        private void OnBlockSelected(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Source == BlockFirstSelectDlg)
                Result = AddRelDlgResult.SelectFirst;
            else
                Result = AddRelDlgResult.SelectSecond;
            Hide();
        }
        
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if ((_blockFirstDlg.IsCreateNew && string.IsNullOrEmpty(_blockFirstDlg.CaptionBlock))
                || (_blockSecondDlg.IsCreateNew && string.IsNullOrEmpty(_blockSecondDlg.CaptionBlock)))
            {
                MessageBox.Show("Задайте имя создаваемого блока");
                return;
            }

            if ((!_blockFirstDlg.IsCreateNew && _blockFirstDlg.MyBlock == null)
                || (!_blockSecondDlg.IsCreateNew && _blockSecondDlg.MyBlock == null))
            {
                MessageBox.Show("Выберите блок или создайте новый");
                return;
            }
            
            if (String.IsNullOrEmpty(RelType))
            {
                MessageBox.Show("Выберите тип реляции или создайте новый");
                return;
            }

            if (_blockFirstDlg.IsCreateNew && _blockSecondDlg.IsCreateNew && !Selection)
            {
                MessageBox.Show("Нельзя создать реляцию между двумя новыми блоками без создания блока реляции");
                return;
            }

            Result = AddRelDlgResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = AddRelDlgResult.Cancel;
            Close();
        }
    }

    public enum AddRelDlgResult
    {
        SelectFirst,
        SelectSecond,
        OK,
        Cancel
    }
}
