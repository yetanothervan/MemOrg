using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChapterViewer.ModuleDefinition;
using GraphDrawService.ModuleDefinition;
using GraphManagementService.ModuleDefinition;
using GraphVizualizeService.ModuleDefinition;
using TempToolbar.ModuleDefinition;
using GraphOrganizeService.ModuleDefinition;
using GraphService.ModuleDefinition;
using GraphViewer.ModuleDefinition;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using MemOrg.WinApp.Avalon;
using WinApp.Shell;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace MemOrg.WinApp.Unity
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.MainViewRegion, typeof(MainView.MainView));
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.TempToolbarRegion, typeof(TempToolbar.ContentView));

            Container.RegisterType<ITmpXmlExportImportService, TmpXmlExportImportService.TmpXmlExportImportService>();

            return Container.Resolve<Shell>();
        }
        
        protected override void InitializeShell()
        {
            base.InitializeShell();
            
            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }

        RegionAdapterMappings _mappings;
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            _mappings = base.ConfigureRegionAdapterMappings();

            _mappings.RegisterMapping(typeof(LayoutAnchorable), new AnchorableRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));
            _mappings.RegisterMapping(typeof(LayoutDocument), new DocumentRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));
            _mappings.RegisterMapping(typeof(DockingManager), new DockingManagerRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));

            return _mappings;
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();
            return factory;
        }

        protected override void ConfigureModuleCatalog()
        {
            AddModule<GraphServiceModule>();
            AddModule<GraphDrawServiceModule>();
            AddModule<GraphOrganizeServiceModule>();
            AddModule<GraphVisualizeServiceModule>();
            AddModule<GraphViewerModule>();
            AddModule<GraphManagementServiceModule>();
            AddModule<TempToolbarModule>();
            AddModule<ChapterViewerModule>();
        }

        private void AddModule<T>() where T : IModule
        {
            var moduleType = typeof(T);
            ModuleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }
    }
}
