using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFC.SelfServeClient.Models;
using WFC.SelfServeClient.ViewModels;

namespace WFC.SelfServeClient
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : Conductor<Screen>
    {
        public Visitor Visitor { get; set; }
        WelcomeViewModel welcomeViewModel { get; set; }
        IdentityIDCardViewModel identityIDCardModel { get; set; }
        public MainWindowViewModel()
        {
            Visitor = new Visitor();
            welcomeViewModel = new WelcomeViewModel(Visitor);
            welcomeViewModel.OnWelcomeButtonClick += WelcomeButtonClick;
            this.ActivateItem(welcomeViewModel);
        }

        private void WelcomeButtonClick()
        {
            identityIDCardModel = new IdentityIDCardViewModel(Visitor);
            this.ActivateItem(identityIDCardModel);
        }
    }
}
