using Caliburn.Micro;
using System.ComponentModel.Composition;
using WFC.SelfServeClient.Models;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(FirstStepViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class FirstStepViewModel : Screen
    {
        public Visitor Visitor { get; set; }
        public event System.Action OnFirstStepClick;

        public FirstStepViewModel(Visitor visitor)
        {
            this.DisplayName = "第一页";
            this.Visitor = visitor;
        }

        public void FirstStepClick()
        {
            OnFirstStepClick?.Invoke();
        }
    }
}
