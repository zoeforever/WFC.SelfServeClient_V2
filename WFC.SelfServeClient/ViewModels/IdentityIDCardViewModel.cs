using AForge.Video.DirectShow;
using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
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
    [Export(typeof(IdentityIDCardViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class IdentityIDCardViewModel : Screen
    {
        IdCardInfo idCardInfo = null;
        IdentityIDCardView identityIDCardView;
      //  CameraCaptureHelper helper;
    //    VideoCaptureDevice videoSource = null;
     //   Bitmap Snapshot = null;
        //2秒执行一次
        int snapshotTimer_timespan = 2;
        //执行6次=1分钟
     //   int snapshotTimer_count = 0;
        public string SnapShotPath { get; set; }
        DispatcherTimer snapshotTimer;
        // DispatcherTimer gotoTimer;
        private HendersonVisitor hendersonVisitor { get; set; } = new HendersonVisitor();

        public event System.Action OnGotoWelcomeClick;
        public event System.Action OnConfirmInfo;

        public IdentityIDCardViewModel(HendersonVisitor hendersonVisitor)
        {
            this.hendersonVisitor = hendersonVisitor;
            snapshotTimer = new DispatcherTimer() { IsEnabled = true };
            snapshotTimer.Interval = TimeSpan.FromSeconds(snapshotTimer_timespan);
            snapshotTimer.Tick += Snapshot_Tick;
            snapshotTimer.Start();

            //gotoTimer = new DispatcherTimer();
            //gotoTimer.Interval = TimeSpan.FromSeconds(2);
            //gotoTimer.Tick += Goto_Tick;
        }
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            identityIDCardView = (IdentityIDCardView)view;
            //this.helper = new CameraCaptureHelper(identityIDCardView.videoSourcePlayer);
            //helper.OnSnapShot += Helper_OnSnapShot;
            //helper.OnConnect += Helper_OnConnect;
            //helper.Connect();
        }

        private void Helper_OnConnect()
        {
            Execute.OnUIThread(() =>
            {
                //this.identityIDCardView.imgLoadCamera.Visibility = Visibility.Collapsed;
                //this.identityIDCardView.wfh.Visibility = Visibility.Visible;
            });
        }

        private void Helper_OnSnapShot(object sender, Bitmap snapshot)
        {
            Execute.OnUIThread(() =>
            {
                //try
                //{
                //    var tmpFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
                //    Snapshot = snapshot;
                //    snapshot.Save(tmpFile);
                //    identityIDCardView.wfh.Visibility = Visibility.Collapsed;
                //    identityIDCardView.imgBG.Visibility = Visibility.Visible;
                //    identityIDCardView.imgUserHead.Visibility = Visibility.Visible;
                //    identityIDCardView.imgUserHead.Source = ImageHelper.GetImage(snapshot);
                //    hendersonVisitor.IdCardNo = idCardInfo.Code;
                //    hendersonVisitor.Name = idCardInfo.Name;
                //    hendersonVisitor.VisitorPhoto = tmpFile;
                //    hendersonVisitor.Gender = idCardInfo.Gender;
                //    hendersonVisitor.Nation = idCardInfo.Nation;

                //    var client = WebApiClient.HttpApi.Resolve<IFaceApi>();

                //    var response = client.CompareAsync(new FaceCompareRequest
                //    {
                //        Image1 = Convert.ToBase64String(ImageHelper.GetBytesByImagePath(idCardInfo.ImagePath))
                //        ,
                //        Image2 = Convert.ToBase64String(ImageHelper.GetAllBytesFromBitmap(Snapshot))
                //    }).GetAwaiter().GetResult();

                //    //身份证头像+抓拍头像 调用接口头像对比
                //    if (response.Same.ToLower() == "true")
                //    {
                //        identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzcg.png"));
                //        gotoTimer.Tag = "success";
                //        gotoTimer.Start();
                //    }
                //    else
                //    {
                //        identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzsb.png"));
                //        gotoTimer.Tag = "fail";
                //        gotoTimer.Start();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Logger.Error($"Compare Exception:{ex}");
                //    identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzsb.png"));
                //    gotoTimer.Tag = "fail";
                //    gotoTimer.Start();
                //}
            });
        }
        private void Snapshot_Tick(object sender, EventArgs e)
        {
            try
            {
                snapshotTimer.Stop();
#if !TEST
                idCardInfo = IdCardReaderHelper.ReadIdCard();
#else
                idCardInfo = new IdCardInfo { Code = "123456", Name = "czb", ImagePath = "", Gender = "男", Nation = "汉" };
#endif
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return;
                //暂不处理
            }
            hendersonVisitor.IdCardNo = idCardInfo.Code;
            hendersonVisitor.Name = idCardInfo.Name;
            hendersonVisitor.VisitorPhoto = idCardInfo.ImagePath;
            hendersonVisitor.Gender = idCardInfo.Gender;
            hendersonVisitor.Nation = idCardInfo.Nation;
            if (hendersonVisitor.IdCardNo.Length < 10)
            {
                Logger.Error("调试判断--身份证号<10位：" + hendersonVisitor.IdCardNo);
                return;
            }
            
            ////身份证头像获取成功，跳转页面
            OnConfirmInfo?.Invoke();

            ////身份证头像获取成功，开始摄像头抓拍
            //helper.Snapshot();

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

        public void GoBack()
        {
            snapshotTimer.Stop();
            OnGotoWelcomeClick?.Invoke();
        }

        private void Goto_Tick(object sender, EventArgs e)
        {
            //helper.Disconnect();
            //gotoTimer.Stop();
            //DispatcherTimer dispatcherTimer = (DispatcherTimer)sender;
            //if (dispatcherTimer.Tag.Equals("success"))
            //{
            //    //hendersonVisitor.IdCardNo = "123";
            //    //hendersonVisitor.Name = "abc";
            //    //hendersonVisitor.VisitorPhoto = @"E:\MyWork\WFC.SelfServeClient\WFC.SelfServeClient\Resources\sfzh.png"; //idCardInfo.ImagePath;
            //    //OnGotoInputInfoClick?.Invoke();
            //}
            //else
            //{
            //    OnGotoWelcomeClick?.Invoke();
            //}
        }

        protected override void OnDeactivate(bool close)
        {
           // base.OnDeactivate(close);
            //try
            //{
            //    helper.Disconnect();
            //}
            //catch
            //{
            //}
        }
    }
}
