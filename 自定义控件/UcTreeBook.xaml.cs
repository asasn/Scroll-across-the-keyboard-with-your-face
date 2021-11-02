using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using 脸滚键盘.公共操作类;
using static 脸滚键盘.公共操作类.TreeOperate;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcTreeBook.xaml 的交互逻辑
    /// </summary>
    public partial class UcTreeBook : UserControl
    {
        public UcTreeBook()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据源：节点列表
        /// </summary>
        ObservableCollection<TreeViewNode> TreeViewNodeList = new ObservableCollection<TreeViewNode>();


        private void tv_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBook();
        }

        void LoadBook()
        {
            //数据初始化
            TreeViewNodeList = new ObservableCollection<TreeViewNode>();

            //数据源加载节点列表
            tv.ItemsSource = TreeViewNodeList;

            //初始化顶层节点数据
            TreeViewNode TopNode = new TreeViewNode();
            TopNode.Uid = "";
            TopNode.IsDir = true;

            TreeViewNode one = AddNewNode(TreeViewNodeList, TopNode);
            AddNewNode(TreeViewNodeList, TopNode);
            AddNewNode(TreeViewNodeList, TopNode);

            AddNewNode(TreeViewNodeList, one);
            AddNewNode(TreeViewNodeList, one);
            AddNewNode(TreeViewNodeList, one);

            Gval.Flag.Loading = true;

            //从数据库中载入数据
            //Load(TreeViewNodeList, TopNode);

            AddButtonNode(TreeViewNodeList, TopNode);
            Gval.Flag.Loading = false;
        }

        /// <summary>
        /// 展示书籍目录抽屉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowBooks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TreeViewMenu_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
