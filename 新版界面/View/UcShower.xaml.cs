using RootNS.Helper;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
        private void ButtonRole_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext.GetType() == typeof(Card))
            {
                (sender as Button).ToolTip = new CardHover((sender as Button).DataContext as Card);
            }
        }

        private void ButtonOther_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext.GetType() == typeof(Card))
            {
                (sender as Button).ToolTip = new CardHover((sender as Button).DataContext as Card);
            }
        }

        private void ButtonYear_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext.GetType() == typeof(Node))
            {
                (sender as Button).ToolTip = ((sender as Button).DataContext as Node);
            }
        }

        private void ButtonYear_Click(object sender, RoutedEventArgs e)
        {
            SettingsHelper.Set(Gval.CurrentBook.Name, Book.SettingKeyName.CurrentYearUid.ToString(), ((sender as Button).DataContext as Node).Uid);
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
            if (Gval.EditorTabControl.SelectedItem == null)
            {
                return;
            }
            Node curNode = ((Gval.EditorTabControl.SelectedItem as TabItem).Content as Editorkernel).DataContext as Node;
            (Gval.View.UcShower.DataContext as Shower).RefreshCards(curNode);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollHelper.ScrollLR(sender, e); 
        }

        private void SvHistory_Loaded(object sender, RoutedEventArgs e)
        {
            SvHistory.ScrollToRightEnd();
        }
    }
}
