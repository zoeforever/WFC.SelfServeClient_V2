using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WFC.SelfServeClient.Helper;
using WFC.SelfServeClient.ViewModels;
using WFC.ServerClient;
using WFC.ServerClient.HttpModels;

namespace WFC.SelfServeClient
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : Conductor<Screen>
    {
        public HendersonVisitor hendersonVisitor { get; set; }
        Thread tokenFetchThread;
        AutoResetEvent exitEvent = new AutoResetEvent(false);
        public bool HasLogin { get; set; }
        LoginViewModel loginViewModel { get; set; }
        WelcomeViewModel welcomeViewModel { get; set; }
        IdentityIDCardViewModel identityIDCardModel { get; set; }
        InformationInputViewModel informationInputViewModel { get; set; }
        FinishViewModel finishViewModel { get; set; }
        IAccountsApi client;
        public MainWindowViewModel()
        {
            HasLogin = false;
            client = WebApiClient.HttpApi.Resolve<IAccountsApi>();
            loginViewModel = new LoginViewModel();
            loginViewModel.OnValidateSuccess += GotoWelcomeClick;
            hendersonVisitor = new HendersonVisitor();
            hendersonVisitor.StartTime = DateTime.Now;
            welcomeViewModel = new WelcomeViewModel(hendersonVisitor);
            welcomeViewModel.OnWelcomeButtonClick += WelcomeButtonClick;
            this.ActivateItem(loginViewModel);
            tokenFetchThread = new Thread(() =>
               {
                   int scanInterval = 2 * 60 * 60 * 1000;

                   while (true)
                   {
                       if (WaitHandle.WaitAny(new[] { exitEvent }, scanInterval) == 0)
                       {
                           break;
                       }
                       try
                       {

                           if (!string.IsNullOrEmpty(WebApiClientHelper.RefreshToken))
                           {
                               try
                               {
                                   var resp = client.RefreshAsync(new GrantRefreshTokenRequest { GrantType = "refresh_token", RefreshToken = WebApiClientHelper.RefreshToken }).GetAwaiter().GetResult();
                                   WebApiClientHelper.AccessToken = resp.result[0].access_token;
                                   WebApiClientHelper.RefreshToken = resp.result[0].refresh_token;
                               }
                               catch (Exception ex)
                               {
                                   Logger.Error(ex.ToString());
                               }
                           }
                       }
                       catch (Exception ex)
                       {
                           Logger.Error($"Error when fetch token:{ex}");
                       }
                   }
               }
            );
            tokenFetchThread.Start();
        }

        public void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.L) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ConsoleLogHelper.Show();
                ConsoleLogHelper.WriteLineDebug("日志输出启动.....");
            }
            // 屏蔽Alt+F4
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.F4) && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                e.Handled = true;
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.LeftCtrl) && e.KeyStates == Keyboard.GetKeyStates(Key.LeftAlt) && e.KeyStates == Keyboard.GetKeyStates(Key.F4))
            {
                this.TryClose();
            }
        }

        public void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //var loginView = IoC.Get<LoginViewModel>();
            var result = MessageBox.Show("是否退出当前程序？", "系统提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void LoginViewModel_OnValidateSuccess()
        {
            throw new NotImplementedException();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            exitEvent.Set();
        }

        private void WelcomeButtonClick()
        {
            identityIDCardModel = new IdentityIDCardViewModel(hendersonVisitor);
            identityIDCardModel.OnGotoWelcomeClick += GotoWelcomeClick;
            identityIDCardModel.OnGotoInputInfoClick += GotoInputInfoClick;
            this.ActivateItem(identityIDCardModel);
        }

        private void GotoWelcomeClick()
        {
            hendersonVisitor = new HendersonVisitor();
            hendersonVisitor.StartTime = DateTime.Now;
            welcomeViewModel = new WelcomeViewModel(hendersonVisitor);
            welcomeViewModel.OnWelcomeButtonClick += WelcomeButtonClick;
            this.ActivateItem(welcomeViewModel);
        }

        private void GotoFinishClick()
        {
            finishViewModel = new FinishViewModel(hendersonVisitor);
            finishViewModel.OnGotoWelcomeClick += GotoWelcomeClick;
            this.ActivateItem(finishViewModel);
        }
        private void GotoInputInfoClick()
        {
            informationInputViewModel = new InformationInputViewModel(hendersonVisitor);
            informationInputViewModel.OnGotoFinishClick += GotoFinishClick;
            informationInputViewModel.OnGotoWelcomeClick += GotoWelcomeClick;
            this.ActivateItem(informationInputViewModel);
        }
    }
}
