using NSMain.Bricks;
using NSMain.Cards;
using NSMain.Editor;
using NSMain.Notes;
using NSMain.Scenes;
using NSMain.Searcher;
using NSMain.Tools;
using NSMain.TreeViewPlus;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static NSMain.TreeViewPlus.CNodeModule;

namespace NSMain
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            CFileOperate.CreateFolder(GlobalVal.Path.Books);
            GlobalVal.SQLClass.Pools.Add("index", new CSqlitePlus(GlobalVal.Path.Books, "index.db"));
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
            GlobalVal.Uc.MainWin = this;
            GlobalVal.Uc.BooksPanel = this.BooksPanel;
        }

        private void UcTreeBook_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.TreeBook = sender as UTreeViewPlus;
            WBooksChoose win = new WBooksChoose();
            win.Window_Loaded(null, null);
        }

        private void UcTreeMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.TreeMaterial = sender as UTreeViewPlus;
            GlobalVal.Uc.TreeMaterial.LoadBook("index", "material");
        }

        private void UcTreeNote_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.TreeNote = sender as UTreeViewPlus;
        }

        private void UcTreeTask_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.TreeTask = sender as UTreeViewPlus;
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.TabControl = sender as HandyControl.Controls.TabControl;
        }

        private void Editor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = GlobalVal.Uc.TabControl.SelectedItem as HandyControl.Controls.TabItem;
            if (tabItem != null)
            {
                UEditor ucEditor = tabItem.Content as UEditor;
                GlobalVal.CurrentBook.CurNode = ucEditor.DataContext as TreeViewNode;
            }
            else
            {
                GlobalVal.CurrentBook.CurNode = new TreeViewNode();
            }
            GlobalVal.Uc.RoleCards.MarkNamesInChapter();
        }

        private void USearcher_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.Searcher = sender as USearcher;
        }

        private void RoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.RoleCards = sender as UCards;
        }

        private void OtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.OtherCards = sender as UCards;
        }

        private void WorldCards_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.WorldCards = sender as UCards;
        }

        private void PublicRoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.PublicRoleCards = sender as UCards;
            GlobalVal.Uc.PublicRoleCards.WpCards.Children.Clear();
            GlobalVal.Uc.PublicRoleCards.LoadCards("index", "角色");
        }

        private void PublicOtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.PublicOtherCards = sender as UCards;
            GlobalVal.Uc.PublicOtherCards.WpCards.Children.Clear();
            GlobalVal.Uc.PublicOtherCards.LoadCards("index", "其他");
        }
        private void PublicWorldCards_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVal.Uc.PublicWorldCards = sender as UCards;
            GlobalVal.Uc.PublicWorldCards.WpCards.Children.Clear();
            GlobalVal.Uc.PublicWorldCards.LoadCards("index", "世界");
        }
        #region 向上/向下调整节点

        /// <summary>
        /// 事件：向上调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            CTreeView.NodeMoveUp(GlobalVal.CurrentBook.Name, "book", (TreeViewNode)GlobalVal.Uc.TreeBook.Tv.SelectedItem, GlobalVal.Uc.TreeBook.TreeViewNodeList);
        }

        /// <summary>
        /// 事件：向下调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            CTreeView.NodeMoveDown(GlobalVal.CurrentBook.Name, "book", (TreeViewNode)GlobalVal.Uc.TreeBook.Tv.SelectedItem, GlobalVal.Uc.TreeBook.TreeViewNodeList);
        }


        #endregion

        #region 工具栏
        private void BooksChoose_Click(object sender, RoutedEventArgs e)
        {
            WBooksChoose win = new WBooksChoose
            {
                Left = Mw.Left + 100,
                Top = Mw.Top + 100
            };
            win.ShowDialog();
        }

        private void NameTool_Click(object sender, RoutedEventArgs e)
        {
            WToolMakeName win = new WToolMakeName();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void HansTool_Click(object sender, RoutedEventArgs e)
        {
            WToolHans win = new WToolHans();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void CollectTool_Click(object sender, RoutedEventArgs e)
        {
            WToolCollect win = new WToolCollect();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void PuzzleTool_Click(object sender, RoutedEventArgs e)
        {
            WPuzzles win = new WPuzzles();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }
        private void InspirationTool_Click(object sender, RoutedEventArgs e)
        {
            WNotes win = new WNotes("index", "book");
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void RecycleBin_Click(object sender, RoutedEventArgs e)
        {
            WRecycleBin win = new WRecycleBin();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();

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
                    CSettings.Set("tomatoTime", CbTime.Value.ToString());
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
            double.TryParse(CSettings.Get("tomatoTime"), out double value);
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
            WToolMap win = new WToolMap();
            win.Left = Mw.Left + Mw.ActualWidth / 2 - win.Width / 2;
            win.Top = Mw.Top + 25;
            win.ShowDialog();
        }

        private void DesignTool_Click(object sender, RoutedEventArgs e)
        {
            WScenes win = new WScenes(GlobalVal.CurrentBook.Name, "book");
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
            //GlobalVal.Uc.SpWin.Dispatcher.Invoke((Action)(() => GlobalVal.Uc.SpWin.Close()));//在GlobalVal.Uc.SpWin的线程上关闭SplashWindow
            GlobalVal.Flag.Loading = false;

        }

        private void Mw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GlobalVal.Uc.TabControl != null)
            {
                foreach (HandyControl.Controls.TabItem tabItem in GlobalVal.Uc.TabControl.Items)
                {
                    tabItem.Focus();
                    UEditor ucEditor = tabItem.Content as UEditor;
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
            foreach (CSqlitePlus sqlConn in GlobalVal.SQLClass.Pools.Values)
            {
                sqlConn.Close();
            }
            GlobalVal.Uc.SpWin.Dispatcher.Invoke(() => AngleImg_Loaded(null, null));//在GlobalVal.Uc.SpWin的线程上关闭SplashWindow

            Application.Current.Shutdown(0);
        }


        DispatcherTimer AngleTimer = new DispatcherTimer();
        private void AngleImg_Loaded(object sender, RoutedEventArgs e)
        {
            AngleTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            GlobalVal.Uc.SpWin.AngleImg.Visibility = Visibility.Visible;
            AngleTimer.Tick += Timer_Tick;
            AngleTimer.Start();
            if (GlobalVal.Uc.SpWin.AngleImg.Width > 535)
            {
                GlobalVal.Uc.SpWin.AngleImg.Width = GlobalVal.Uc.SpWin.AngleImg.Height = 535;
            }
            GlobalVal.Uc.SpWin.AngleImg.Opacity = 1;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            GlobalVal.Uc.SpWin.Angle += 7.2;
            GlobalVal.Uc.SpWin.AngleImg.RenderTransform = new RotateTransform(GlobalVal.Uc.SpWin.Angle);
            GlobalVal.Uc.SpWin.ImgWidth -= 10;
            if (GlobalVal.Uc.SpWin.ImgWidth < 0)
            {
                GlobalVal.Uc.SpWin.ImgWidth = 0;
            }
            GlobalVal.Uc.SpWin.AngleImg.Width = GlobalVal.Uc.SpWin.AngleImg.Height = GlobalVal.Uc.SpWin.ImgWidth;
            GlobalVal.Uc.SpWin.AngleImg.Opacity -= 0.01;
            if (GlobalVal.Uc.SpWin.AngleImg.Opacity <= 0 || GlobalVal.Uc.SpWin.ImgWidth <= 25)
            {
                AngleTimer.Stop();
                AngleTimer.Tick -= Timer_Tick;
                GlobalVal.Uc.SpWin.Close();
            }
        }


    }
}
