using AForge.Video.DirectShow;
using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WFC.SelfServeClient.Helper;
using WFC.SelfServeClient.Views;
using WFC.ServerClient;
using WFC.ServerClient.HttpModels;

namespace WFC.SelfServeClient.ViewModels
{
    public class FaceIdentificationViewModel : Screen
    {
        IdCardInfo idCardInfo = null;
        FaceIdentificationView identityIDCardView;
        CameraCaptureHelper helper;
        VideoCaptureDevice videoSource = null;
        Bitmap Snapshot = null;
        //5秒执行一次
        int snapshotTimer_timespan = 5;
        //执行6次=1分钟
        int snapshotTimer_count = 0;
        // 失败次数
        int failedTimes = 0;
        public string SnapShotPath { get; set; }
        DispatcherTimer snapshotTimer;
        DispatcherTimer gotoTimer;
        public HendersonVisitor hendersonVisitor { get; set; }
        public event System.Action OnGotoWelcomeClick;
        public event System.Action OnGotoInputInfoClick;

        public FaceIdentificationViewModel(HendersonVisitor hendersonVisitor)
        {
            this.hendersonVisitor = hendersonVisitor;
            snapshotTimer = new DispatcherTimer();
            snapshotTimer.Interval = TimeSpan.FromSeconds(snapshotTimer_timespan);
            snapshotTimer.Tick += Snapshot_Tick;
            snapshotTimer.Start();

            gotoTimer = new DispatcherTimer();
            gotoTimer.Interval = TimeSpan.FromSeconds(2);
            gotoTimer.Tick += Goto_Tick;
        }
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            identityIDCardView = (FaceIdentificationView)view;
            this.helper = new CameraCaptureHelper(identityIDCardView.videoSourcePlayer);
            helper.OnSnapShot += Helper_OnSnapShot;
            helper.OnConnect += Helper_OnConnect;
            helper.Connect();
        }

        private void Helper_OnConnect()
        {
            Execute.OnUIThread(() =>
            {
                this.identityIDCardView.imgLoadCamera.Visibility = Visibility.Collapsed;
                this.identityIDCardView.wfh.Visibility = Visibility.Visible;
            });
        }

        private void Helper_OnSnapShot(object sender, Bitmap snapshot)
        {
            Execute.OnUIThread(() =>
            {
                try
                {
                    var icCardPhoto = hendersonVisitor.VisitorPhoto;
                    var tmpFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
                    Snapshot = snapshot;
                    snapshot.Save(tmpFile);
                    identityIDCardView.wfh.Visibility = Visibility.Collapsed;
                    identityIDCardView.imgBG.Visibility = Visibility.Visible;
                    identityIDCardView.imgUserHead.Visibility = Visibility.Visible;
                    identityIDCardView.imgUserHead.Source = ImageHelper.GetImage(snapshot);
                    hendersonVisitor.VisitorPhoto = tmpFile;

                    var client = WebApiClient.HttpApi.Resolve<IFaceApi>();

#if TEST
                    //identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzcg.png"));
                    //gotoTimer.Tag = "success";
                    //gotoTimer.Start();
                    Fail("对比不一致");
#else

                    var response = client.CompareAsync(new FaceCompareRequest
                    {
                        Image1 = Convert.ToBase64String(ImageHelper.GetBytesByImagePath(icCardPhoto)),
                        Image2 = Convert.ToBase64String(ImageHelper.GetAllBytesFromBitmap(Snapshot))
                    }).GetAwaiter().GetResult();
                    //身份证头像+抓拍头像 调用接口头像对比
                    if (response.Same.ToLower() == "true")
                    {
                        identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzcg.png"));
                        gotoTimer.Tag = "success";
                        gotoTimer.Start();
                    }
                    else
                    {
                        Fail("对比不一致");
                    }
#endif
                }
                catch (Exception ex)
                {
                    Fail(ex.ToString());
                }

                void Fail(string msg)
                {
                    Logger.Warn(msg);

                    identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzsb.png"));

                    dynamic settings = new ExpandoObject();
                    settings.WindowStyle = WindowStyle.None;
                  //  settings.ShowInTaskbar = false;
                    settings.WindowState = WindowState.Normal;
                    settings.ResizeMode = ResizeMode.CanMinimize;

                    failedTimes++;
                    if (failedTimes == 3)
                    {
                        new WindowManager().ShowDialog(new MessageBoxViewModel(FailAndRetry.FaceIdentificationFail3), null, settings);
                    }
                    else
                    {
                        new WindowManager().ShowDialog(new MessageBoxViewModel(FailAndRetry.FaceIdentificationFail), null, settings);
                    }
                    gotoTimer.Tag = "fail";
                    gotoTimer.Start();
                }
            });
        }
        private void Snapshot_Tick(object sender, EventArgs e)
        {
            snapshotTimer.Stop();
            helper.Snapshot();

            //if (snapshotTimer_count > 60 / snapshotTimer_timespan)
            //{
            //    helper.Disconnect();
            //    snapshotTimer.Stop();
            //    //identityIDCardView.wfh.Visibility = Visibility.Collapsed;
            //    //identityIDCardView.imgBG.Visibility = Visibility.Visible;
            //    //identityIDCardView.imgUserHead.Visibility = Visibility.Visible;
            //    //identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzsb.png"));
            //    gotoTimer.Tag = "fail";
            //    gotoTimer.Start();
            //}
        }

        private void Goto_Tick(object sender, EventArgs e)
        {
            helper.Disconnect();
            gotoTimer.Stop();
            DispatcherTimer dispatcherTimer = (DispatcherTimer)sender;
            if (dispatcherTimer.Tag.Equals("success"))
            {
                //hendersonVisitor.IdCardNo = "123";
                //hendersonVisitor.Name = "abc";
                //hendersonVisitor.VisitorPhoto = @"E:\MyWork\WFC.SelfServeClient\WFC.SelfServeClient\Resources\sfzh.png"; //idCardInfo.ImagePath;
                OnGotoInputInfoClick?.Invoke();
            }
            else
            {
                if (failedTimes >= 3)
                {
                    OnGotoWelcomeClick?.Invoke();
                }
                else
                {
                    helper.Connect();
                    snapshotTimer.Start();
                }
            }
        }

        public void GoBack()
        {
            OnGotoWelcomeClick?.Invoke();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            try
            {
                helper.Disconnect();
            }
            catch
            {
            }
        }
    }
}
