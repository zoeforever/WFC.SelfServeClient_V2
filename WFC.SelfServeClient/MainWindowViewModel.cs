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
        FirstStepViewModel FirstStep { get; set; }
        SecondStepViewModel SecondStep { get; set; }

        public MainWindowViewModel()
        {
            Visitor = new Visitor();
            FirstStep = new FirstStepViewModel(Visitor);
            FirstStep.OnFirstStepClick += FirstStep_OnFirstStepClick;
            SecondStep = new SecondStepViewModel(Visitor);
            SecondStep.OnSecondStepClickPre += SecondStep_OnSecondStepClickPre;
            SecondStep.OnSecondStepClickNext += SecondStep_OnSecondStepClickNext;
            this.ActivateItem(FirstStep);
        }

        private void SecondStep_OnSecondStepClickNext()
        {
        }

        private void SecondStep_OnSecondStepClickPre()
        {
            this.ActivateItem(FirstStep);
        }

        private void FirstStep_OnFirstStepClick()
        {
            this.ActivateItem(SecondStep);
        }
    }
}
