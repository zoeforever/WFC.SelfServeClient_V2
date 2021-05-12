using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Windows;
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
            if (string.IsNullOrEmpty(hendersonVisitor.Phone))
            {
                MessageBox.Show("请输入手机号！");
                return;
            }
            if (string.IsNullOrEmpty(hendersonVisitor.IdCardNo))
            {
                MessageBox.Show("证件号不能为空！");
            }
            OnGotoFaceIdentification?.Invoke();
        }

        public void GoBack()
        {
            OnGotoWelcomeClick?.Invoke();
        }
    }
}
