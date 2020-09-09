using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFC.SelfServeClient.Models;
using WFC.SelfServeClient.ViewModels;
using WFC.ServerClient.HttpModels;

namespace WFC.SelfServeClient
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : Conductor<Screen>
    {
        public HendersonVisitor hendersonVisitor { get; set; }
        WelcomeViewModel welcomeViewModel { get; set; }
        IdentityIDCardViewModel identityIDCardModel { get; set; }
        InformationInputViewModel informationInputViewModel { get; set; }
        public MainWindowViewModel()
        {
            hendersonVisitor = new HendersonVisitor();
            hendersonVisitor.StartTime = DateTime.Now;
            welcomeViewModel = new WelcomeViewModel(hendersonVisitor);
            welcomeViewModel.OnWelcomeButtonClick += WelcomeButtonClick;
            this.ActivateItem(welcomeViewModel);
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
            this.ActivateItem(welcomeViewModel);
        }

        private void GotoInputInfoClick()
        {
            informationInputViewModel = new InformationInputViewModel(hendersonVisitor);
            this.ActivateItem(informationInputViewModel);
        }
    }
}
