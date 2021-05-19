using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Windows;
using WFC.ServerClient.HttpModels;


namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(VisitorConfirmViewModel))]
    [PropertyChanged.ImplementPropertyChanged]
    public class VisitorConfirmViewModel : Screen
    {
        public HendersonVisitor VisitorhendersonVisitor { get; set; } = new HendersonVisitor();
        public event System.Action OnGotoWelcomeClick;
         public event System.Action OnGotoFaceIdentification;
        public VisitorConfirmViewModel(HendersonVisitor hendersonVisitor)
        {
            this.VisitorhendersonVisitor = hendersonVisitor;
        }

        public void NextPage()
        {
            if (string.IsNullOrEmpty(VisitorhendersonVisitor.Phone))
            {
                MessageBox.Show("请输入手机号！");
                return;
            }
            if (string.IsNullOrEmpty(VisitorhendersonVisitor.IdCardNo))
            {
                MessageBox.Show("证件号不能为空！");
                return;
            }
             OnGotoFaceIdentification?.Invoke();
        }

        public void GoBack()
        {
            OnGotoWelcomeClick?.Invoke();
        }
    }
}
