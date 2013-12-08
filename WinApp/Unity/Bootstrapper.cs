﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BlocksMapViewer;
using BlocksMapViewer.ModuleDefinition;
using GraphService.ModuleDefinition;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using MemOrg.WinApp.Avalon;
using WinApp.MainView;
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
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.MainViewRegion, typeof(MainView));

            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            
            App.Current.MainWindow = (Window) Shell;
            App.Current.MainWindow.Show();
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
            AddModule<BlocksMapViewerModule>();
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
