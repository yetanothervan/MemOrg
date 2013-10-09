using System;
using System.Windows;
using MemOrg.WinApp;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;
using Xceed.Wpf.AvalonDock.Layout;

namespace MemOrg.WinApp.Avalon
{
    class AvalonDockRegion : DependencyObject
    {
        #region AnchorName

        /// <summary>
        /// AnchorName Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnchorNameProperty =
            DependencyProperty.RegisterAttached("AnchorName", typeof(string), typeof(AvalonDockRegion),
                new FrameworkPropertyMetadata((string)null,
                    new PropertyChangedCallback(OnAnchorNameChanged)));

        /// <summary>
        /// Gets the AnchorName property.  This dependency property 
        /// indicates the region name of the layout item.
        /// </summary>
        public static string GetAnchorName(DependencyObject d)
        {
            return (string)d.GetValue(AnchorNameProperty);
        }

        /// <summary>
        /// Sets the AnchorName property.  This dependency property 
        /// indicates the region name of the layout item.
        /// </summary>
        public static void SetAnchorName(DependencyObject d, string value)
        {
            d.SetValue(AnchorNameProperty, value);
        }

        /// <summary>
        /// Handles changes to the AnchorName property.
        /// </summary>
        private static void OnAnchorNameChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            CreateAnchorRegion((LayoutAnchorable)s, (string)e.NewValue);
        }

        #endregion

        #region DocName

        /// <summary>
        /// DocName Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty DocNameProperty =
            DependencyProperty.RegisterAttached("DocName", typeof(string), typeof(AvalonDockRegion),
                new FrameworkPropertyMetadata((string)null,
                    new PropertyChangedCallback(OnDocNameChanged)));

        /// <summary>
        /// Gets the DocName property.  This dependency property 
        /// indicates the region name of the layout item.
        /// </summary>
        public static string GetDocName(DependencyObject d)
        {
            return (string)d.GetValue(DocNameProperty);
        }

        /// <summary>
        /// Sets the AnchorName property.  This dependency property 
        /// indicates the region name of the layout item.
        /// </summary>
        public static void SetDocName(DependencyObject d, string value)
        {
            d.SetValue(DocNameProperty, value);
        }

        /// <summary>
        /// Handles changes to the DocName property.
        /// </summary>
        private static void OnDocNameChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            CreateDocRegion((LayoutDocument)s, (string)e.NewValue);
        }

        #endregion

        static void CreateAnchorRegion(LayoutAnchorable element, string regionName)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            //If I'm in design mode the main window is not set
            if (App.Current == null ||
                App.Current.MainWindow == null)
                return;

            try
            {
                if (ServiceLocator.Current == null)
                    return;

                // Build the region
                var mappings = ServiceLocator.Current.GetInstance<RegionAdapterMappings>();
                if (mappings == null)
                    return;
                IRegionAdapter regionAdapter = mappings.GetMapping(element.GetType());
                if (regionAdapter == null)
                    return;

                regionAdapter.Initialize(element, regionName);
            }
            catch (Exception ex)
            {
                throw new RegionCreationException(string.Format("Unable to create region {0}", regionName), ex);
            }

        }

        static void CreateDocRegion(LayoutDocument element, string regionName)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            //If I'm in design mode the main window is not set
            if (App.Current == null ||
                App.Current.MainWindow == null)
                return;

            try
            {
                if (ServiceLocator.Current == null)
                    return;

                // Build the region
                var mappings = ServiceLocator.Current.GetInstance<RegionAdapterMappings>();
                if (mappings == null)
                    return;
                IRegionAdapter regionAdapter = mappings.GetMapping(element.GetType());
                if (regionAdapter == null)
                    return;

                regionAdapter.Initialize(element, regionName);
            }
            catch (Exception ex)
            {
                throw new RegionCreationException(string.Format("Unable to create region {0}", regionName), ex);
            }

        }
    }
}
