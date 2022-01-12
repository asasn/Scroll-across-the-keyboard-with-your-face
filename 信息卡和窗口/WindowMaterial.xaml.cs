using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// MaterialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WindowMaterial : Window
    {
        public WindowMaterial()
        {
            InitializeComponent();
        }

        private void UcEditor_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = Gval.Uc.TreeMaterial.CurNode.NodeName;
            UcontrolEditor.DataContext = Gval.Uc.TreeMaterial.CurNode;
            UcontrolEditor.LoadChapter("index", "material");
        }
    }
}