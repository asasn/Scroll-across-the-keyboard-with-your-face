using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using 脸滚键盘.公共操作类;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcTimeBar.xaml 的交互逻辑
    /// </summary>
    public partial class UcHistoryBar : UserControl
    {
        public UcHistoryBar()
        {
            InitializeComponent();
            //添加拖曳面板事件
        }

        public string TypeOfTree;
        public string CurBookName;

        public void LoadYears(string curBookName, string typeOfTree)
        {
            if (string.IsNullOrEmpty(curBookName))
            {
                return;
            }
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            WpYears.Tag = typeOfTree;

            Sv.ScrollToRightEnd();

            Gval.Flag.Loading = true;

            string tableName = typeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("CREATE TABLE IF NOT EXISTS Tree_{0} (Uid CHAR PRIMARY KEY, Pid CHAR, NodeName CHAR, isDir BOOLEAN, NodeContent TEXT, WordsCount INTEGER, IsExpanded  BOOLEAN);", tableName);
            sqlConn.ExecuteNonQuery(sql);
            sql = string.Format("SELECT * FROM Tree_{0} where Pid='';", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                Button BtnTag = new Button();
                BtnTag.Padding = new Thickness(2);
                BtnTag.Height = 30;
                BtnTag.Margin = new Thickness(5, 5, 0, 0);
                BtnTag.Click += BtnTag_Click;
                ContextMenu aMenu = new ContextMenu();
                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "删除";
                deleteMenu.Click += DeleteMenu_Click; ;
                aMenu.Items.Add(deleteMenu);
                BtnTag.ContextMenu = aMenu;
                aMenu.DataContext = BtnTag;
                BtnTag.Uid = reader["Uid"].ToString();
                BtnTag.Content = reader["NodeName"].ToString();
                WpYears.Children.Add(BtnTag);
            }
            reader.Close();
            sqlConn.Close();


            Gval.Flag.Loading = false;
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
            {
                scrollviewer.LineLeft();
                scrollviewer.LineLeft();
                scrollviewer.LineLeft();
            }
            else
            {
                scrollviewer.LineRight();
                scrollviewer.LineRight();
                scrollviewer.LineRight();
            }
            e.Handled = true;
        }

        private void BtnGoLeft_Click(object sender, RoutedEventArgs e)
        {
            Sv.PageLeft();
        }

        private void BtnGoRight_Click(object sender, RoutedEventArgs e)
        {
            Sv.PageRight();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnGoLeft.Visibility = Visibility.Visible;
            BtnGoRight.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnGoLeft.Visibility = Visibility.Hidden;
            BtnGoRight.Visibility = Visibility.Hidden;
        }

        Point _lastMouseDown;
        double lastOffset;
        private void Sv_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Mouse.SetCursor(Cursors.SizeWE);
                double distance = e.GetPosition(Sv).X - _lastMouseDown.X;
                Sv.ScrollToHorizontalOffset(lastOffset - distance);
            }
            else
            {
                lastOffset = Sv.HorizontalOffset;
            }
        }

        private void Sv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMouseDown = e.GetPosition(this);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TbTab_KeyDown(object sender, KeyEventArgs e)
        {

        }


    }
}
