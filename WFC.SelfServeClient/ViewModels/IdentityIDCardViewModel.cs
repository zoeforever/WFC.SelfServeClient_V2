using AForge.Video.DirectShow;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        IdentityIDCardView identityIDCardView;
        CameraCaptureHelper helper;
        VideoCaptureDevice videoSource = null;
        Bitmap Snapshot = null;
        public string SnapShotPath { get; set; }
        DispatcherTimer snapshotTimer;
        public Visitor Visitor { get; set; }
        public IdentityIDCardViewModel(Visitor visitor)
        {
            this.Visitor = visitor;
            snapshotTimer = new DispatcherTimer();
            snapshotTimer.Interval = TimeSpan.FromSeconds(5);
            snapshotTimer.Tick += Snapshot_Tick;
            snapshotTimer.Start();
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
                Snapshot = snapshot;
                identityIDCardView.wfh.Visibility = Visibility.Collapsed;
                identityIDCardView.imgUserHead.Visibility = Visibility.Visible;
                identityIDCardView.imgUserHead.Source = ImageHelper.GetImage(snapshot);
            });
        }
        private void Snapshot_Tick(object sender, EventArgs e)
        {
            snapshotTimer.Stop();
            identityIDCardView.wfh.Visibility = Visibility.Collapsed;
            identityIDCardView.imgUserHead.Visibility = Visibility.Visible;
            identityIDCardView.imgBG.Visibility = Visibility.Visible;
            identityIDCardView.imgUserHead.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/rzcg.png"));
            //helper.Snapshot();
        }
    }
}
