using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WFC.SelfServeClient
{
    /// <summary>
    /// IdentificationIDCard.xaml 的交互逻辑
    /// </summary>
    public partial class IdentificationIDCard : Window
    {
        public bool connect_idcard_interface = false;
        public const int COUNTDOWN = 5;
        DispatcherTimer timer;

        public IdentificationIDCard()
        {
            InitializeComponent();
            /*
            此处加一个定时器，超过1分钟未放置身份证跳回第一页            
            */
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(COUNTDOWN);
            timer.Tick += timer1_Tick;
            //timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!connect_idcard_interface)
            {
                timer.Stop();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();                
                this.Close();
            }
        }
    }
}
