using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;

namespace WinApp.MainView
{
    public class MainViewViewModel : ViewModelBase
    {
        private readonly ITmpXmlExportImportService _exportImportService;

        public MainViewViewModel(ITmpXmlExportImportService exportImportService)
        {
            _exportImportService = exportImportService;
            ExportCommand = new DelegateCommand(ExecuteExportCommand, () => true);
            ImportCommand = new DelegateCommand(ExecuteImportCommand, () => true);
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand ImportCommand { get; set; }

        public void ExecuteExportCommand()
        {
           _exportImportService.SaveGraph(); 
        }

        public void ExecuteImportCommand()
        {
            _exportImportService.LoadGraph();
        }
    }
}
