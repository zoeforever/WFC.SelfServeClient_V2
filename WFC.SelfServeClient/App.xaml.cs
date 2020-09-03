using System;
using System.Configuration;
using System.Windows;
using WFC.ServerClient;

namespace WFC.SelfServeClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static string ServerUri = ConfigurationManager.AppSettings["WebApiServiceUrl"];
        public App()
        {
            this.Startup += App_Startup;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            WebApiClient.HttpApi.Register<IHendersonVisitorApi>().ConfigureHttpApiConfig(option => { option.HttpHost = new Uri(ServerUri); });
        }
    }
}
