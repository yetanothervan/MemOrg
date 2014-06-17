using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ChapterViewer.CreatePointBlock;
using DAL.Entity;

namespace ChapterViewer.AddRefDlg
{
    /// <summary>
    /// Логика взаимодействия для AddRefView.xaml
    /// </summary>
    public partial class AddRefView : Window
    {
        private readonly CreatePointBlockViewModel _blockDlg;

        // ReSharper disable once InconsistentNaming
        private const int GWL_STYLE = -16;
        // ReSharper disable once InconsistentNaming
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public AddRefView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

                DataContext = new AddRefViewModel();
            };

            InitializeComponent();

            _blockDlg = new CreatePointBlockViewModel();
            BlockSelectDlg.DataContext = _blockDlg;
            _blockDlg.CaptionView = "Укажите блок";
            _blockDlg.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsCreateNew")
                {
                    var addRefViewModel = DataContext as AddRefViewModel;
                    if (addRefViewModel != null) 
                        addRefViewModel.CreateUserTextCheckBoxEnabled = _blockDlg.IsCreateNew;
                }
            };
        }

        public string RefType
        {
            get
            {
                var addRefViewModel = DataContext as AddRefViewModel;
                if (addRefViewModel != null) return addRefViewModel.RefType;
                return null;
            }
        }

        public bool IsCreateUserText
        {
            get
            {
                var addRefViewModel = DataContext as AddRefViewModel;
                if (addRefViewModel != null)
                    return addRefViewModel.IsCreateUserText;
                throw new NotImplementedException();
            }
        }

        public string Caption
        {
            get { return _blockDlg.CaptionBlock; }
        }

        public Block Block
        {
            get { return _blockDlg.MyBlock; }
            set { _blockDlg.MyBlock = value; }
        }

        public AddRefDlgResult? Result { get; set; }

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

            if (String.IsNullOrEmpty(RefType))
            {
                MessageBox.Show("Выберите ссылочный тип");
                return;
            }

            Result = AddRefDlgResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = AddRefDlgResult.Cancel;
            Close();
        }

        private void OnBlockSelected(object sender, ExecutedRoutedEventArgs e)
        {
            Result = AddRefDlgResult.Select;
            Hide();
        }
    }

    public enum AddRefDlgResult
    {
        Select,
        OK,
        Cancel
    }
}
