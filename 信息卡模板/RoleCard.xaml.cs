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
    /// RoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class RoleCard : Window
    {
        public RoleCard(TreeViewItem selectedItem, MouseButtonEventArgs e)
        {
            InitializeComponent();
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            Point pointToWindow = (e.Source as FrameworkElement).PointToScreen(p);//转化为屏幕中的坐标
            this.Left = pointToWindow.X - this.Width / 2;
            this.Top = pointToWindow.Y - this.Height / 2;
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            Grid g = this.Content as Grid;
            UIElementCollection elements = g.Children;
            foreach (UIElement element in elements)
            {
                if (element.GetType() == typeof(Label))
                {
                    Console.WriteLine((element as Label).Content);
                    
                }
            }
        }
    }
}
