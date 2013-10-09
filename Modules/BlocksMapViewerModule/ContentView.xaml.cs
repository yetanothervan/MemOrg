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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace BlocksMapViewerModule
{
    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView : UserControl, IModule
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;
        
        public ContentView(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
            InitializeComponent();
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.BlocksMapViewerRegion, typeof(ContentView));
        }
    }
}
