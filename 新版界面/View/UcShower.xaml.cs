using RootNS.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RootNS.View
{
    /// <summary>
    /// UcShower.xaml 的交互逻辑
    /// </summary>
    public partial class UcShower : UserControl
    {
        public UcShower()
        {
            InitializeComponent();
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as Button).ToolTip = new CardHover((sender as Button).DataContext as Card);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            Timer.Tick += TimeRuner;
            Timer.Start();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        public DispatcherTimer Timer = new DispatcherTimer();

        /// <summary>
        /// 方法：每次间隔运行的内容
        /// </summary>
        private void TimeRuner(object sender, EventArgs e)
        {
            Shower.RefreshCards();
        }

    }
}
