using System;
using System.Collections.Generic;
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

namespace 脸滚键盘.信息卡模板
{
    /// <summary>
    /// MaterialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialWindow : Window
    {
        public MaterialWindow(TreeViewItem curItem, string ucTag)
        {
            InitializeComponent();

            if (ucEditor != null)
            {
                ucEditor.CurItem = curItem;
                ucEditor.UcTag = ucTag;
                ucEditor.DataContext = curItem;
            }

        }
    }
}
