using Caliburn.Micro;
using System;
using System.Windows.Input;
using System.Windows.Threading;
using WFC.SelfServeClient.Views;

namespace WFC.SelfServeClient.ViewModels
{
    public class MessageBoxViewModel : Screen
    {
        MessageBoxView View;
        FailAndRetry failAndRetry;
        private DispatcherTimer snapshotTimerMess;
        private int snapshotTimer_timespanMess = 5;
        public MessageBoxViewModel(FailAndRetry failAndRetry)
        {
            this.failAndRetry = failAndRetry;
            snapshotTimerMess = new DispatcherTimer();
            snapshotTimerMess.Interval = TimeSpan.FromSeconds(snapshotTimer_timespanMess);
            snapshotTimerMess.Tick += Snapshot_Tick;
            snapshotTimerMess.Start();
        }

        public void Click(object sender, KeyEventArgs e)
        {
            snapshotTimerMess.Stop();
            this.TryClose();
        }

        public void MouseClick(object sender, MouseButtonEventArgs e)
        {
            snapshotTimerMess.Stop();
            this.TryClose();
        }

        public void LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            snapshotTimerMess.Stop();
            this.TryClose();
        }

        protected override void OnViewLoaded(object view)
        {
            View = view as MessageBoxView;
            if (failAndRetry == FailAndRetry.FaceIdentificationFail)
            {
                View.Fail1.Visibility = System.Windows.Visibility.Visible;
            }
            else if (failAndRetry == FailAndRetry.FaceIdentificationFail3 || failAndRetry == FailAndRetry.InformationInputFail3)
            {
                View.Fail2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (failAndRetry == FailAndRetry.InformationInputFail)
            {
                View.Fail3.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void Snapshot_Tick(object sender, EventArgs e)
        {
            snapshotTimerMess.Stop();

            this.TryClose();
        }
    }

    public enum FailAndRetry
    {
        FaceIdentificationFail,
        FaceIdentificationFail3,
        InformationInputFail,
        InformationInputFail3,
    }
}
