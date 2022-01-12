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

            FileOperate.CreateFolder(Gval.Path.Books);
            Gval.SQLClass.Pools.Add("index", new SqliteOperate(Gval.Path.Books, "index.db"));
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
            this.Activate();
            Gval.Uc.MainWin = this;
            Gval.Uc.BooksPanel = this.BooksPanel;
        }

        private void UcTreeBook_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeBook = sender as UcontrolTreeBook;
            WindowBooksChoose win = new WindowBooksChoose();
            win.Window_Loaded(null, null);
        }

        private void UcTreeMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeMaterial = sender as UcontrolTreeBook;
            Gval.Uc.TreeMaterial.LoadBook("index", "material");
        }

        private void UcTreeNote_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeNote = sender as UcontrolTreeBook;
        }

        private void UcTreeTask_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeTask = sender as UcontrolTreeTask;
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
                UcontrolEditor ucEditor = tabItem.Content as UcontrolEditor;
                Gval.CurrentBook.CurNode = ucEditor.DataContext as TreeViewNode;
            }
            else
            {
                Gval.CurrentBook.CurNode = new TreeViewNode();
            }
            Gval.Uc.RoleCards.MarkNamesInChapter();
        }



        private void RoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.RoleCards = sender as UcontrolCards;
        }

        private void OtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.OtherCards = sender as UcontrolCards;
        }

        private void PublicRoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicRoleCards = sender as UcontrolCards;
            Gval.Uc.PublicRoleCards.WpCards.Children.Clear();
            CardOperate.TryToBuildBaseTable("index", "角色");
            Gval.Uc.PublicRoleCards.LoadCards("index", "角色");
        }

        private void PublicOtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicOtherCards = sender as UcontrolCards;
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
            WindowBooksChoose win = new WindowBooksChoose
            {
                Left = Mw.Left + 100,
                Top = Mw.Top + 100
            };
            win.ShowDialog();
        }

        private void NameTool_Click(object sender, RoutedEventArgs e)
        {
            WindowNameTool win = new WindowNameTool();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void HansTool_Click(object sender, RoutedEventArgs e)
        {
            WindowHansTool win = new WindowHansTool();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void CollectTool_Click(object sender, RoutedEventArgs e)
        {
            WindowCollectTool win = new WindowCollectTool();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void PuzzleTool_Click(object sender, RoutedEventArgs e)
        {

        }
        private void InspirationTool_Click(object sender, RoutedEventArgs e)
        {
            WindowNotesTool win = new WindowNotesTool("index", "book");
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
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

            if ((int)StopWatch.Elapsed.TotalMinutes >= CbTime.Value)
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
            WindowMapTool win = new WindowMapTool();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void DesignTool_Click(object sender, RoutedEventArgs e)
        {
            WindowScenes win = new WindowScenes(Gval.CurrentBook.Name, "book");
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
            //Gval.Uc.SpWin.Dispatcher.Invoke((Action)(() => Gval.Uc.SpWin.Close()));//在Gval.Uc.SpWin的线程上关闭SplashWindow
        }

        private void Mw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Gval.Uc.TabControl != null)
            {
                foreach (HandyControl.Controls.TabItem tabItem in Gval.Uc.TabControl.Items)
                {
                    tabItem.Focus();
                    UcontrolEditor ucEditor = tabItem.Content as UcontrolEditor;
                    if (ucEditor.BtnSaveDoc.IsEnabled == true)
                    {
                        MessageBoxResult dr = MessageBox.Show("该章节尚未保存\n要在退出前保存更改吗？", "Tip", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
                        if (dr == MessageBoxResult.Yes)
                        {
                            ucEditor.BtnSaveText_Click(null, null);
                        }
                        if (dr == MessageBoxResult.No)
                        {

                        }
                        if (dr == MessageBoxResult.Cancel)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
            foreach (SqliteOperate sqlConn in Gval.SQLClass.Pools.Values)
            {
                sqlConn.Close();
            }

            Gval.Uc.SpWin.Dispatcher.Invoke(() => AngleImg_Loaded(null, null));//在Gval.Uc.SpWin的线程上关闭SplashWindow

            Application.Current.Shutdown(0);
        }


        DispatcherTimer AngleTimer = new DispatcherTimer();
        private void AngleImg_Loaded(object sender, RoutedEventArgs e)
        {
            AngleTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            Gval.Uc.SpWin.AngleImg.Visibility = Visibility.Visible;
            AngleTimer.Tick += Timer_Tick;
            AngleTimer.Start();
            if (Gval.Uc.SpWin.AngleImg.Width > 535)
            {
                Gval.Uc.SpWin.AngleImg.Width = Gval.Uc.SpWin.AngleImg.Height = 535;
            }
            Gval.Uc.SpWin.AngleImg.Opacity = 1;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            Gval.Uc.SpWin.Angle += 7.2;
            Gval.Uc.SpWin.AngleImg.RenderTransform = new RotateTransform(Gval.Uc.SpWin.Angle);
            Gval.Uc.SpWin.ImgWidth -= 10;
            if (Gval.Uc.SpWin.ImgWidth < 0)
            {
                Gval.Uc.SpWin.ImgWidth = 0;
            }
            Gval.Uc.SpWin.AngleImg.Width = Gval.Uc.SpWin.AngleImg.Height = Gval.Uc.SpWin.ImgWidth;
            Gval.Uc.SpWin.AngleImg.Opacity -= 0.01;
            if (Gval.Uc.SpWin.AngleImg.Opacity <= 0 || Gval.Uc.SpWin.ImgWidth <= 25)
            {
                AngleTimer.Stop();
                AngleTimer.Tick -= Timer_Tick;
                Gval.Uc.SpWin.Close();
            }
        }
    }
}
