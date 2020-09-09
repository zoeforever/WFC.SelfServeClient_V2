using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFC.ServerClient.HttpModels;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(IdentityIDCardViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class InputUserInfoViewModel : Screen
    {
        public HendersonVisitor hendersonVisitor { get; set; }

        public InputUserInfoViewModel(HendersonVisitor hendersonVisitor)
        {
            this.hendersonVisitor = hendersonVisitor;
        }
    }
}
