using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using 脸滚键盘.控件方法类;
using 脸滚键盘.自定义控件;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            RunningCheck();
            FileOperate.CreateFolder(Gval.Path.Books);
            Gval.SQLClass.Pools.Add("index", new SqliteOperate(Gval.Path.Books, "index.db"));
        }

        private void RunningCheck()
        {
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                MessageBox.Show("应用程序运行中");
                Application.Current.Shutdown();
                return;
            }
        }

        public string CurrentBookName
        {
            get { return (string)GetValue(CurrentBookNameProperty); }
            set { SetValue(CurrentBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentBookNameProperty =
            DependencyProperty.Register("CurrentBookName", typeof(string), typeof(MainWindow), new PropertyMetadata(null));




        private void Mw_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.MainWin = this;
            Gval.Uc.BooksPanel = this.BooksPanel;
        }

        private void UcTreeBook_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeBook = sender as UcTreeBook;
            BooksChooseWindow win = new BooksChooseWindow();
            win.Window_Loaded(null, null);
        }

        private void UcTreeMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeMaterial = sender as UcTreeBook;
            Gval.Uc.TreeMaterial.LoadBook("index", "material");
        }

        private void UcTreeNote_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeNote = sender as UcTreeBook;
        }

        private void UcTreeTask_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeTask = sender as UcTreeTask;
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TabControl = sender as HandyControl.Controls.TabControl;
        }

        private void Editor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = Gval.Uc.TabControl.SelectedItem as HandyControl.Controls.TabItem;
            if (tabItem != null)
            {
                UcEditor ucEditor = tabItem.Content as UcEditor;
                Gval.CurrentBook.CurNode = ucEditor.DataContext as TreeViewNode;
            }
            else
            {
                Gval.CurrentBook.CurNode = new TreeViewNode();
            }
            Gval.Uc.RoleCards.MarkNamesInChapter();
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            if (Gval.Uc.TabControl != null)
            {
                foreach (HandyControl.Controls.TabItem tabItem in Gval.Uc.TabControl.Items)
                {
                    tabItem.Focus();
                    UEditor.TabItemClosing(tabItem, e);
                }
            }
            foreach (SqliteOperate sqlConn in Gval.SQLClass.Pools.Values)
            {
                sqlConn.Close();
            }
            Application.Current.Shutdown(0);
        }

        private void RoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.RoleCards = sender as UcCards;
        }

        private void OtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.OtherCards = sender as UcCards;
        }

        private void PublicRoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicRoleCards = sender as UcCards;
            Gval.Uc.PublicRoleCards.WpCards.Children.Clear();
            CardOperate.TryToBuildBaseTable("index", "角色");
            Gval.Uc.PublicRoleCards.LoadCards("index", "角色");
        }

        private void PublicOtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicOtherCards = sender as UcCards;
            Gval.Uc.PublicOtherCards.WpCards.Children.Clear();
            CardOperate.TryToBuildBaseTable("index", "其他");
            Gval.Uc.PublicOtherCards.LoadCards("index", "其他");
        }

        #region 向上/向下调整节点

        /// <summary>
        /// 事件：向上调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            UTreeView.NodeMoveUp(Gval.CurrentBook.Name, "book", (TreeViewNode)Gval.Uc.TreeBook.Tv.SelectedItem, Gval.Uc.TreeBook.TreeViewNodeList);
        }

        /// <summary>
        /// 事件：向下调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            UTreeView.NodeMoveDown(Gval.CurrentBook.Name, "book", (TreeViewNode)Gval.Uc.TreeBook.Tv.SelectedItem, Gval.Uc.TreeBook.TreeViewNodeList);
        }


        #endregion

        #region 工具栏
        private void BooksChoose_Click(object sender, RoutedEventArgs e)
        {
            BooksChooseWindow win = new BooksChooseWindow
            {
                Left = Mw.Left + 100,
                Top = Mw.Top + 100
            };
            win.ShowDialog();
        }

        private void NameTool_Click(object sender, RoutedEventArgs e)
        {
            NameToolWindow win = new NameToolWindow();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void HansTool_Click(object sender, RoutedEventArgs e)
        {
            HansToolWindow win = new HansToolWindow();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void CollectTool_Click(object sender, RoutedEventArgs e)
        {
            CollectToolWindow win = new CollectToolWindow();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void PuzzleTool_Click(object sender, RoutedEventArgs e)
        {

        }
        private void InspirationTool_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecycleBin_Click(object sender, RoutedEventArgs e)
        {

        }

        #region 番茄时间
        public string ShowTimeText { get { return String.Format("{0:D2}:{1:D2}", (int)StopWatch.Elapsed.TotalMinutes, StopWatch.Elapsed.Seconds); } }
        Stopwatch StopWatch = new Stopwatch();
        DispatcherTimer Timer = new DispatcherTimer();
        bool TagChange = false;

        /// <summary>
        /// 事件：控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTime_Loaded(object sender, RoutedEventArgs e)
        {
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            Timer.Tick += TimeRuner;
        }

        /// <summary>
        /// 事件：点击开始，启动番茄时间的计时功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TomatoTimeStart_Click(object sender, RoutedEventArgs e)
        {
            if (Timer.IsEnabled)
            {
                BtnTime.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resourses/图标/工具栏/ic_action_playback_play.png"));
                TomatoTimeStop_Click(null, null);
            }
            else
            {
                BtnTime.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resourses/图标/工具栏/ic_action_playback_stop.png"));

                if (TagChange == true)
                {
                    SettingsOperate.Set("tomatoTime", CbTime.Value.ToString());
                    TagChange = false;
                }

                CbTime.Visibility = Visibility.Hidden;
                MeDida.Stop();
                MeRing.Stop();
                Timer.Start();
                StopWatch.Start();
            }
        }

        /// <summary>
        /// 方法：每次间隔运行的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeRuner(object sender, EventArgs e)
        {
            TbTime.Text = ShowTimeText;
            MeDida.Stop();
            MeDida.Play();

            if ((int)StopWatch.Elapsed.TotalMinutes == CbTime.Value)
            {
                MeRing.Play();
                TomatoTimeStart_Click(null, null);
            }
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TomatoTimeStop_Click(object sender, RoutedEventArgs e)
        {
            StopWatch = new Stopwatch();
            Timer.Stop();
            TbTime.Text = ShowTimeText;
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TomatoTimePause_Click(object sender, RoutedEventArgs e)
        {
            StopWatch.Stop();
            Timer.Stop();
            TbTime.Text = ShowTimeText;

        }

        /// <summary>
        /// 事件：时间设置值更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbTime_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            TagChange = true;
        }

        /// <summary>
        /// 事件：时间设置值载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbTime_Loaded(object sender, RoutedEventArgs e)
        {
            double.TryParse(SettingsOperate.Get("tomatoTime"), out double value);
            CbTime.Value = value;
        }

        /// <summary>
        /// 事件：右键进入设置模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BorderTomatoTime_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CbTime.Visibility == Visibility.Visible)
            {
                CbTime.Visibility = Visibility.Hidden;
            }
            else
            {
                CbTime.Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void MapTool_Click(object sender, RoutedEventArgs e)
        {
            MapWindow win = new MapWindow();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void DesignTool_Click(object sender, RoutedEventArgs e)
        {
            DesignToolWindow win = new DesignToolWindow();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void AnchorTool_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LockTool_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DBTool_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppSettings_Click(object sender, RoutedEventArgs e)
        {

        }






        #endregion

        /// <summary>
        /// 事件：内容呈现（加载完成）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mw_ContentRendered(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}
