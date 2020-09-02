using Caliburn.Micro;
using System.ComponentModel.Composition;
using WFC.SelfServeClient.Models;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(WelcomeViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class WelcomeViewModel : Screen
    {
        public Visitor Visitor { get; set; }
        public event System.Action OnWelcomeButtonClick;
        public WelcomeViewModel(Visitor visitor)
        {
            this.Visitor = visitor;
        }

        public void WelcomeButtonClick()
        {
            OnWelcomeButtonClick?.Invoke();
        }
    }
}
