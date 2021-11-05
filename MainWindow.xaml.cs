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
    }
}
