using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private void Window_Closing(object sender, EventArgs e)
        {
            foreach (HandyControl.Controls.TabItem tabItem in Gval.Uc.TabControl.Items)
            {
                tabItem.Focus();
                UEditor.TabItemClosing(tabItem, e);
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
            Gval.Uc.PublicRoleCards.LoadCards("index", "角色");
            CardOperate.TryToBuildBaseTable("index", "角色");
        }

        private void PublicOtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicOtherCards = sender as UcCards;
            Gval.Uc.PublicOtherCards.WpCards.Children.Clear();
            Gval.Uc.PublicOtherCards.LoadCards("index", "其他");
            CardOperate.TryToBuildBaseTable("index", "其他");
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


        private void NameTool_Click(object sender, RoutedEventArgs e)
        {
            NameToolWindow win = new NameToolWindow();
            win.Left = 600;
            win.Top = 60;
            win.ShowDialog();
        }

        private void HansTool_Click(object sender, RoutedEventArgs e)
        {
            HansToolWindow win = new HansToolWindow();
            win.Left = 600;
            win.Top = 60;
            win.ShowDialog();
        }

        private void CollectTool_Click(object sender, RoutedEventArgs e)
        {
            CollectToolWindow win = new CollectToolWindow();
            win.Left = 600;
            win.Top = 60;
            win.ShowDialog();
        }

        private void BooksChoose_Click(object sender, RoutedEventArgs e)
        {
            BooksChooseWindow win = new BooksChooseWindow();
            win.Left = 100;
            win.Top = 60;
            win.ShowDialog();
        }
    }
}
