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
using 脸滚键盘.信息卡和窗口;
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
            WpYears.Children.Clear();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            WpYears.Tag = typeOfTree;
            TbYear.Clear();
            TbYear.Visibility = Visibility.Hidden;
            TbYear.Uid = "";

            Uc.Tag = false;

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
                Button BtnTag = AddNode(reader["Uid"].ToString(), reader["NodeName"].ToString());
                WpYears.Children.Add(BtnTag);
            }
            reader.Close();
            sqlConn.Close();


            Gval.Flag.Loading = false;
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            Button BtnTag = (sender as MenuItem).DataContext as Button;
            WpYears.Children.Remove(BtnTag);

            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("DELETE from Tree_{0} where Uid='{1}';", tableName, BtnTag.Uid);
            sqlConn.ExecuteNonQuery(sql);
            sql = string.Format("DELETE from Tree_{0} where Pid='{1}';", tableName, BtnTag.Uid);
            sqlConn.ExecuteNonQuery(sql);
            sqlConn.Close();
        }

        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)Uc.Tag != true)
            {
                Uc.Tag = true;
                Button BtnYearTag = sender as Button;

                WpYears.Children.Clear();
                TbYear.Visibility = Visibility.Visible;
                TbYear.Text = BtnYearTag.Content.ToString();
                TbYear.Uid = BtnYearTag.Uid;

                string tableName = TypeOfTree;
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                string sql = string.Format("SELECT * FROM Tree_{0} where Pid='{1}';", tableName, BtnYearTag.Uid);
                SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
                while (reader.Read())
                {
                    Button BtnTag = AddNode(reader["Uid"].ToString(), reader["NodeName"].ToString());
                    WpYears.Children.Add(BtnTag);                    
                }
                reader.Close();
                sqlConn.Close();
            }
            else
            {
                Button BtnTag = sender as Button;
                if (BtnTag.BorderBrush != Brushes.DodgerBlue)
                {
                    BtnTag.BorderBrush = Brushes.DodgerBlue;
                }
                else
                {
                    BtnTag.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
                }
                foreach (Button btn in WpYears.Children)
                {
                    if (btn != BtnTag && BtnTag.BorderBrush == Brushes.DodgerBlue)
                    {
                        btn.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
                    }
                }

            }
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
            LoadYears(CurBookName, TypeOfTree);
        }

        private void BtnGoRight_Click(object sender, RoutedEventArgs e)
        {
            
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
            if (false == string.IsNullOrEmpty(TbTab.Text))
            {
                string tableName = TypeOfTree;
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");

                string guid = Guid.NewGuid().ToString();
                Button BtnTag = AddNode(guid, TbTab.Text);
                WpYears.Children.Add(BtnTag);

                string sql = string.Format("INSERT INTO Tree_{0} (Uid, Pid, NodeName, isDir, NodeContent, WordsCount, IsExpanded) VALUES ('{1}', '{2}', '{3}', {4}, '{5}', {6}, {7});", tableName, guid, TbYear.Uid, TbTab.Text, true, "", 0, 0);
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();
                sqlConn.Close();
                TbTab.Clear();
            }
        }

        private void TbTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAdd_Click(null, null);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            
            
        }

        Button AddNode(string guid, string content)
        {
            Button BtnTag = new Button();
            BtnTag.Padding = new Thickness(2);
            BtnTag.Margin = new Thickness(5, 5, 0, 0);
            BtnTag.Click += BtnTag_Click;
            ContextMenu aMenu = new ContextMenu();
            MenuItem deleteMenu = new MenuItem();
            deleteMenu.Header = "删除";
            deleteMenu.Click += DeleteMenu_Click; ;
            aMenu.Items.Add(deleteMenu);
            BtnTag.ContextMenu = aMenu;
            aMenu.DataContext = BtnTag;
            BtnTag.Uid = guid;
            BtnTag.Content = content;
            return BtnTag;
        }

        private void TbYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && false == string.IsNullOrEmpty(TbYear.Text))
            {
                string tableName = TypeOfTree;
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, TbYear.Text, TbYear.Uid);
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();
            }
        }
    }
}
