using System;
using System.Configuration;
using System.Windows;
using WFC.SelfServeClient.Helper;
using WFC.ServerClient;

namespace WFC.SelfServeClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static string ServerUri = ConfigurationManager.AppSettings["WebApiServiceUrl"];
        private static string TokenFetchUrl = ConfigurationManager.AppSettings["TokenFetchUrl"];
        public App()
        {
            this.Startup += App_Startup;
            this.Exit += App_Exit;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Environment.Exit(0);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error("App_DispatcherUnhandledException:" + e.Exception.ToString());
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {

            Logger.Open();
            string level = ConfigurationManager.AppSettings["LoggerLever"];
            Logger.SetCanWriteLeval(level);

            WebApiClient.HttpApi.Register<IHendersonVisitorApi>().ConfigureHttpApiConfig(option => { option.HttpHost = new Uri(ServerUri); });
            WebApiClient.HttpApi.Register<IFaceApi>().ConfigureHttpApiConfig(option => { option.HttpHost = new Uri(ServerUri); });
            WebApiClient.HttpApi.Register<IAccountsApi>().ConfigureHttpApiConfig(option => { option.HttpHost = new Uri(TokenFetchUrl); });
        }
    }
}
