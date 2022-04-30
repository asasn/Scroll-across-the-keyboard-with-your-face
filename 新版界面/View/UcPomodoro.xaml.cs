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

namespace RootNS.View
{
    /// <summary>
    /// UcPomodoro.xaml 的交互逻辑
    /// </summary>
    public partial class UcPomodoro : UserControl
    {
        public UcPomodoro()
        {
            InitializeComponent();
        }

        public Pomodoro ThisPomodoro { get; set; } = new Pomodoro();

        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            ThisPomodoro.MeDida = MeDida;
            ThisPomodoro.MeRing = MeRing;
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ThisPomodoro.MouseRightButtonDown();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ThisPomodoro.Start();
        }

        private void CbTime_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            ThisPomodoro.CbTime_ValueChanged();
        }

        private void CbTime_Loaded(object sender, RoutedEventArgs e)
        {
            ThisPomodoro.CbTime_Loaded();
        }

    }
}
