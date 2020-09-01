using Caliburn.Micro;
using System.ComponentModel.Composition;
using WFC.SelfServeClient.Models;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(SecondStepViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class SecondStepViewModel : Screen
    {
        public Visitor Visitor { get; set; }
        public event System.Action OnSecondStepClickPre;
        public event System.Action OnSecondStepClickNext;
        public SecondStepViewModel(Visitor visitor)
        {
            this.DisplayName = "第二页";
            this.Visitor = visitor;
        }

        public void SecondStepClickPre()
        {
            OnSecondStepClickPre?.Invoke();
        }
        public void SecondStepClickNext()
        {
            OnSecondStepClickNext?.Invoke();
        }
    }
}
