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
            ExportCommand = new DelegateCommand(ExecuteExportCommand, CanExecuteExportCommand);
        }

        public DelegateCommand ExportCommand { get; set; }

        public void ExecuteExportCommand()
        {
           _exportImportService.SaveGraph(); 
        }

        public bool CanExecuteExportCommand()
        {
            return true;
        }
    }
}
