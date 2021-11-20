using System.Windows;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using 脸滚键盘.自定义控件;

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



        private void UcTreeBook_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeBook = sender as UcTreeBook;
        }

        private void UcTreeMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeMaterial = sender as UcTreeMaterial;
        }

        private void UcTreeNote_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeNote = sender as UcTreeNote;
        }

        private void UcTreeTask_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeTask = sender as UcTreeTask;
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TabControl = sender as HandyControl.Controls.TabControl;
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
    }
}
