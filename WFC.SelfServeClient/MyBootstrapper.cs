
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace WFC.SelfServeClient
{
    /// <summary>
    ///     The app bootstrapper.
    /// </summary>
    public class MyBootstrapper : BootstrapperBase
    {
        #region 字段

        /// <summary>
        ///     The container.
        /// </summary>
        private CompositionContainer container;

        #endregion

        #region 构造函数
        static MyBootstrapper()
        {
            ConventionManager.AddElementConvention<FrameworkElement>(UIElement.IsEnabledProperty, "IsEnabled", "IsEnabledChanged");
            var baseBindProperties = ViewModelBinder.BindProperties;
            ViewModelBinder.BindProperties = (frameworkElements, viewModels) =>
            {
                foreach (var frameworkElement in frameworkElements)
                {
                    var propertyName = frameworkElement.Name + "Enabled";
                    var property = viewModels.GetPropertyCaseInsensitive(propertyName);
                    if (property != null)
                    {
                        var convention = ConventionManager.GetElementConvention(typeof(FrameworkElement));
                        ConventionManager.SetBindingWithoutBindingOverwrite(viewModels, propertyName, property, frameworkElement, convention, convention.GetBindableProperty(frameworkElement));
                    }
                }
                return baseBindProperties(frameworkElements, viewModels);
            };
        }
        private readonly IEventAggregator eventAggregator;
        /// <summary>
        ///     Initializes a new instance of the <see cref="MyBootstrapper" /> class.
        /// </summary>
        public MyBootstrapper()
        {
            this.Initialize();
            this.eventAggregator = IoC.Get<IEventAggregator>();
            this.eventAggregator.Subscribe(this);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// The build up.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        protected override void BuildUp(object instance)
        {
            this.container.SatisfyImportsOnce(instance);
        }

        /// <summary>
        ///     The configure.
        /// </summary>
        protected override void Configure()
        {
            this.container =
                new CompositionContainer(
                    new AggregateCatalog(
                        AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(this.container);

            this.container.Compose(batch);
        }

        /// <summary>
        /// The get all instances.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        /// <summary>
        /// The get instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = this.container.GetExportedValues<object>(contract);

            if (exports.Any())
            {
                return exports.First();
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        /// <summary>
        /// The on startup.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            this.DisplayRootViewFor<MainWindowViewModel>();
        }

        /// <summary>
        /// The select assemblies.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            string path = Path.GetDirectoryName(
                       Assembly.GetExecutingAssembly().Location);
            return base.SelectAssemblies()
                .Concat(
                new Assembly[] {
                });
        }

        #endregion
    }
}
