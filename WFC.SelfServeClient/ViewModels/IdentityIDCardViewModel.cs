﻿using AForge.Video.DirectShow;
using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WFC.SelfServeClient.Helper;
using WFC.SelfServeClient.Models;
using WFC.SelfServeClient.Views;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(IdentityIDCardViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class IdentityIDCardViewModel : Screen
    {
        IdCardInfo idCardInfo = null;
        IdentityIDCardView identityIDCardView;
        CameraCaptureHelper helper;
        VideoCaptureDevice videoSource = null;
        Bitmap Snapshot = null;
        //10秒执行一次
        int snapshotTimer_timespan = 10;
        //执行6次=1分钟
        int snapshotTimer_count = 0;
        public string SnapShotPath { get; set; }
        DispatcherTimer snapshotTimer;
        DispatcherTimer gotoTimer;
        public Visitor Visitor { get; set; }

        public event System.Action OnGotoWelcomeClick;

        public event System.Action OnGotoInputInfoClick;

        public IdentityIDCardViewModel(Visitor visitor)
        {
            this.Visitor = visitor;
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
            identityIDCardView = (IdentityIDCardView)view;
            this.helper = new CameraCaptureHelper(identityIDCardView.videoSourcePlayer);
            helper.OnSnapShot += Helper_OnSnapShot;
            helper.Connect();
        }

        private void Helper_OnSnapShot(object sender, Bitmap snapshot)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    Snapshot = snapshot;
                    identityIDCardView.wfh.Visibility = Visibility.Collapsed;
                    identityIDCardView.imgBG.Visibility = Visibility.Visible;
                    identityIDCardView.imgUserHead.Visibility = Visibility.Visible;
                    identityIDCardView.imgUserHead.Source = ImageHelper.GetImage(snapshot);

                    //身份证头像+抓拍头像 调用接口头像对比
                    if (true)
                    {
                        identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzcg.png"));
                        gotoTimer.Tag = "success";
                        gotoTimer.Start();
                    }
                    else
                    {
                        identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzsb.png"));
                        gotoTimer.Tag = "fail";
                        gotoTimer.Start();
                    }
                }
                catch { }
                finally
                {
                    helper.Disconnect();
                }
            });
        }
        private void Snapshot_Tick(object sender, EventArgs e)
        {
            try
            {
                idCardInfo = IdCardReaderHelper.ReadIdCard();
            }
            catch (Exception ex)
            {
                //暂不处理
            }
            if (idCardInfo == null) snapshotTimer_count = snapshotTimer_count + 1;
            else
            {
                snapshotTimer.Stop();
                //身份证头像获取成功，开始摄像头抓拍
                helper.Snapshot();
            }
            if (snapshotTimer_count > 6)
            {
                helper.Disconnect();
                snapshotTimer.Stop();
                identityIDCardView.wfh.Visibility = Visibility.Collapsed;
                identityIDCardView.imgBG.Visibility = Visibility.Visible;
                identityIDCardView.imgUserHead.Visibility = Visibility.Visible;
                identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzsb.png"));
                gotoTimer.Tag = "fail";
                gotoTimer.Start();
            }
        }

        private void Goto_Tick(object sender, EventArgs e)
        {
            DispatcherTimer dispatcherTimer = (DispatcherTimer)sender;
            if (dispatcherTimer.Tag.Equals("success"))
            {
                OnGotoInputInfoClick?.Invoke();
            }
            else
            {
                OnGotoWelcomeClick?.Invoke();
            }
            gotoTimer.Stop();
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
