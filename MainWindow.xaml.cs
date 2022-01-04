﻿using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.Media;
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
            RunningCheck();
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
            Gval.Uc.MWindow = this;
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
                ucEditor.MarkNamesInChapter();
            }
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
            Application.Current.Shutdown();
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
            BooksChooseWindow win = new BooksChooseWindow();
            win.Left = Mw.Left + 100;
            win.Top = Mw.Top + 100;
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
        public string ShowTimeText { get { return String.Format("{0:D2}:{1:D2}", (int)stopWatch.Elapsed.TotalMinutes, stopWatch.Elapsed.Seconds); } }
        Stopwatch stopWatch = new Stopwatch();
        DispatcherTimer timer = new DispatcherTimer();
        bool changeTag = false;
        private void TomatoTimeStart_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                BtnTime.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resourses/图标/工具栏/ic_action_playback_play.png"));
                TomatoTimeStop_Click(null, null);
            }
            else
            {
                BtnTime.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resourses/图标/工具栏/ic_action_playback_stop.png"));
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1000);
                timer.Tick += TimeRuner;
                timer.Start();
                if (changeTag == true)
                {
                    SettingsOperate.Set("tomatoTime", CbTime.Value.ToString());
                    changeTag = false;
                }
                CbTime.Visibility = Visibility.Hidden;
                mediaElement.Source = new Uri(Gval.Path.Resourses + "/声音/dida.wav", UriKind.Relative);
                stopWatch.Start();
            }
        }

        private void TimeRuner(object sender, EventArgs e)
        {
            TbTime.Text = ShowTimeText;
            mediaElement.Stop();
            mediaElement.Play();

            if ((int)stopWatch.Elapsed.TotalMinutes == CbTime.Value)
            {
                mediaElement.Source = new Uri(Gval.Path.Resourses + "/声音/ring.wav", UriKind.Relative);
                mediaElement.Play();
                TomatoTimeStart_Click(null, null);
            }
        }


        private void TomatoTimeStop_Click(object sender, RoutedEventArgs e)
        {
            stopWatch = new Stopwatch();
            timer.Tick -= TimeRuner;
            timer.Stop();
            TbTime.Text = ShowTimeText;
        }

        private void TomatoTimePause_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Stop();
            timer.Tick -= TimeRuner;
            timer.Stop();
            TbTime.Text = ShowTimeText;

        }

        private void CbTime_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            changeTag = true;
        }

        private void CbTime_Loaded(object sender, RoutedEventArgs e)
        {
            double value;
            double.TryParse(SettingsOperate.Get("tomatoTime"), out value);
            CbTime.Value = value;
        }

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


    }
}
