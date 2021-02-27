using Caliburn.Micro;
using System.ComponentModel.Composition;
using WFC.ServerClient.HttpModels;


namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(IdentityIDCardViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class VisitorConfirmViewModel : Screen
    {
        public HendersonVisitor hendersonVisitor { get; set; }
        public event System.Action OnGotoWelcomeClick;
        public event System.Action OnGotoFaceIdentification;

        public VisitorConfirmViewModel(HendersonVisitor hendersonVisitor)
        {
            this.hendersonVisitor = hendersonVisitor;
        }

        public void NextPage()
        {
            OnGotoFaceIdentification?.Invoke();
        }

        public void GoBack()
        {
            OnGotoWelcomeClick?.Invoke();
        }
    }
}
