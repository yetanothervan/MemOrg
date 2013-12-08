using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using MemOrg.WinApp.Unity;
using Microsoft.Practices.Unity;

namespace MemOrg.WinApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var btsr = new Bootstrapper();
            btsr.Run();
        }
    }
}
