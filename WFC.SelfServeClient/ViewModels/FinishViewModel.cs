using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WFC.ServerClient;
using WFC.ServerClient.HttpModels;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(FinishViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class FinishViewModel : Screen
    {
        private HendersonVisitor hendersonVisitor { get; set; }
        public event System.Action OnGotoWelcomeClick;
        DispatcherTimer gotoWelcomeTimer;
        public FinishViewModel(HendersonVisitor hendersonVisitor)
        {
            gotoWelcomeTimer = new DispatcherTimer();
            gotoWelcomeTimer.Interval = TimeSpan.FromSeconds(10);
            gotoWelcomeTimer.Tick += Snapshot_Tick;
            gotoWelcomeTimer.Start();
            this.hendersonVisitor = hendersonVisitor;
        }

        public void FinishButtonClick()
        {
            gotoWelcomeTimer.Stop();
            OnGotoWelcomeClick?.Invoke();
        }

        private void Snapshot_Tick(object sender, EventArgs e)
        {
            FinishButtonClick();
        }
    }
}
