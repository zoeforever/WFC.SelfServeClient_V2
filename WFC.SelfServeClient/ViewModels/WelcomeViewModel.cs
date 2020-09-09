﻿using Caliburn.Micro;
using System.ComponentModel.Composition;
using WFC.SelfServeClient.Models;
using WFC.ServerClient.HttpModels;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(WelcomeViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class WelcomeViewModel : Screen
    {
        public HendersonVisitor hendersonVisitor { get; set; }
        public event System.Action OnWelcomeButtonClick;
        public WelcomeViewModel(HendersonVisitor hendersonVisitor)
        {
            this.hendersonVisitor = hendersonVisitor;
        }

        public void WelcomeButtonClick()
        {
            OnWelcomeButtonClick?.Invoke();
        }
    }
}
