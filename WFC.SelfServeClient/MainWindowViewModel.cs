using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WFC.SelfServeClient.Helper;
using WFC.SelfServeClient.ViewModels;
using WFC.ServerClient;
using WFC.ServerClient.HttpModels;

namespace WFC.SelfServeClient
{
    [PropertyChanged.ImplementPropertyChanged]
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : Conductor<Screen>
    {
        public HendersonVisitor hendersonVisitor { get; set; }
        public bool HasLogin { get; set; }
        public string LeftTime { get; set; }
        public Visibility ShowCountDown { get; set; } = Visibility.Collapsed;

        Thread tokenFetchThread;
        string location = ConfigurationManager.AppSettings["Location"];
        int COUNTDOWN
        {
            get
            {
                int cd = 120;
                if (int.TryParse(ConfigurationManager.AppSettings["CountDown"], out cd))
                {
                    return cd;
                }
                return 120;
            }
        }

        AutoResetEvent exitEvent = new AutoResetEvent(false);
        // 第一步：登录
        LoginViewModel loginViewModel { get; set; }
        // 第二步：欢迎界面
        WelcomeViewModel welcomeViewModel { get; set; }
        // 第三步：验证身份证
        IdentityIDCardViewModel identityIDCardModel { get; set; }
        // 第四步：访客信息确认
        VisitorConfirmViewModel visitorConfirmViewModel { get; set; }
        // 第五步：人脸确认
        FaceIdentificationViewModel faceIdentificationViewModel { get; set; }
        // 第六步：受访人信息录入
        InformationInputViewModel informationInputViewModel { get; set; }
        // 第七步：完成页面
        FinishViewModel finishViewModel { get; set; }
        IAccountsApi client;
        MainWindowView View;
        DispatcherTimer timer;
        int CountDownSeconds;

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
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            this.ActivateItem(loginViewModel);
            CountDownSeconds = COUNTDOWN;
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.LeftTime = (CountDownSeconds--).ToString();
            if (CountDownSeconds <= 0)
            {
             
                GotoWelcomeClick();
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            View = view as MainWindowView;
            if (location == "wfc.east.tower")
            {
                View.bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/bgEast.png"));
            }
            else
            {
                View.bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/bgWest.png"));
            }
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
            ResetTimer();
        }

        /// <summary>
        /// 欢迎按钮
        /// </summary>
        private void WelcomeButtonClick()
        {
            identityIDCardModel = new IdentityIDCardViewModel(hendersonVisitor);
            identityIDCardModel.OnGotoWelcomeClick -= GotoWelcomeClick;
            identityIDCardModel.OnGotoWelcomeClick += GotoWelcomeClick;
            identityIDCardModel.OnConfirmInfo -= GotoConfirmInfoClick;
            identityIDCardModel.OnConfirmInfo += GotoConfirmInfoClick;
            this.ActivateItem(identityIDCardModel);
            StartTimer();
        }

        /// <summary>
        /// 返回欢迎页面
        /// </summary>
        private void GotoWelcomeClick()
        {
           
            hendersonVisitor = new HendersonVisitor();
            hendersonVisitor.StartTime = DateTime.Now;
            welcomeViewModel = new WelcomeViewModel(hendersonVisitor);
            welcomeViewModel.OnWelcomeButtonClick -= WelcomeButtonClick;
            welcomeViewModel.OnWelcomeButtonClick += WelcomeButtonClick;
            this.ActivateItem(welcomeViewModel);
            ResetTimer();
        }

        /// <summary>
        /// 跳转信息确认页面
        /// </summary>
        private void GotoConfirmInfoClick()
        {
            visitorConfirmViewModel = new VisitorConfirmViewModel(hendersonVisitor);
            visitorConfirmViewModel.OnGotoWelcomeClick -= GotoWelcomeClick;
            visitorConfirmViewModel.OnGotoWelcomeClick += GotoWelcomeClick;
            visitorConfirmViewModel.OnGotoFaceIdentification -= GotoFaceIdentification;
             visitorConfirmViewModel.OnGotoFaceIdentification += GotoFaceIdentification;

            this.ActivateItem(visitorConfirmViewModel);
        }

        /// <summary>
        /// 跳转人脸认证页面
        /// </summary>
        private void GotoFaceIdentification()
        {
            faceIdentificationViewModel = new FaceIdentificationViewModel(hendersonVisitor);
            faceIdentificationViewModel.OnGotoWelcomeClick -= GotoWelcomeClick;
            faceIdentificationViewModel.OnGotoWelcomeClick += GotoWelcomeClick;
            faceIdentificationViewModel.OnGotoInputInfoClick -= GotoInputInfoClick;
            faceIdentificationViewModel.OnGotoInputInfoClick += GotoInputInfoClick;
            this.ActivateItem(faceIdentificationViewModel);
        }

        /// <summary>
        /// 跳转信息录入页面
        /// </summary>
        private void GotoInputInfoClick()
        {
            informationInputViewModel = new InformationInputViewModel(hendersonVisitor);
            informationInputViewModel.OnGotoWelcomeClick -= GotoWelcomeClick;
            informationInputViewModel.OnGotoWelcomeClick += GotoWelcomeClick;
            informationInputViewModel.OnGotoFinishClick -= GotoFinishClick;
            informationInputViewModel.OnGotoFinishClick += GotoFinishClick;
            this.ActivateItem(informationInputViewModel);
        }

        /// <summary>
        /// 跳转完成页面
        /// </summary>
        private void GotoFinishClick()
        {
            finishViewModel = new FinishViewModel(hendersonVisitor);
            finishViewModel.OnGotoWelcomeClick -= GotoWelcomeClick;
            finishViewModel.OnGotoWelcomeClick += GotoWelcomeClick;
            this.ActivateItem(finishViewModel);
        }

        private void ResetTimer()
        {
            timer.Stop();
            CountDownSeconds = COUNTDOWN;
            LeftTime = COUNTDOWN.ToString();
            ShowCountDown = Visibility.Collapsed;
        }

        private void StartTimer()
        {
            ShowCountDown = Visibility.Visible;
            timer.Start();
        }
    }
}
