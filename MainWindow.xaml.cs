using System.Windows;
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

        private void UcTreeRoleCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeRoleCard = sender as UcTreeRoleCard;
        }

        private void UcTreeInfoCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeInfoCard = sender as UcTreeInfoCard;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PublicRoleCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicRoleCard = sender as UcTreeRoleCard;
            Gval.Uc.PublicRoleCard.LoadBook("index", "角色");
            CardOperate.TryToBuildBaseTable("index", "角色");

        }

        private void PublicInfoCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicInfoCard = sender as UcTreeInfoCard;
            Gval.Uc.PublicInfoCard.LoadBook("index", "其他");
            CardOperate.TryToBuildBaseTable("index", "其他");
        }
    }
}
