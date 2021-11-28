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
using 脸滚键盘.控件方法类;

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
        bool OnYearsPanel;
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
            OnYearsPanel = true;


            Sv.ScrollToEnd();

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

        Button BtnSelected;
        int yIndex;
        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {
            BtnSelected = sender as Button;
            TextBlock tbk = UTreeView.FindVisualChild<TextBlock>(BtnSelected);
            if (OnYearsPanel == true)
            {
                yIndex = WpYears.Children.IndexOf(BtnSelected);
            }
            if (BtnSelected.BorderBrush != Brushes.DodgerBlue)
            {
                BtnSelected.BorderBrush = Brushes.DodgerBlue;
                if (OnYearsPanel == true)
                {
                    BtnGoRight.Visibility = Visibility.Visible;
                }
                //Point p = BtnSelected.TranslatePoint(new Point(), Bar);
                //TbSelected.Margin = new Thickness(p.X, 0, 0, 30);
                //TbSelected.Width = BtnSelected.ActualWidth;
                TbSelected.Visibility = Visibility.Visible;
                //TbSelected.Text = BtnSelected.Content.ToString();
                TbSelected.Text = tbk.Text;
                TbSelected.Uid = BtnSelected.Uid;
            }
            else
            {
                BtnSelected.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
                BtnGoRight.Visibility = Visibility.Hidden;
                TbSelected.Visibility = Visibility.Hidden;
            }
            foreach (Button btn in WpYears.Children)
            {
                if (btn != BtnSelected)
                {
                    btn.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
                }
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            //if (e.Delta > 0)
            //{
            //    scrollviewer.LineLeft();
            //    scrollviewer.LineLeft();
            //    scrollviewer.LineLeft();
            //}
            //else
            //{
            //    scrollviewer.LineRight();
            //    scrollviewer.LineRight();
            //    scrollviewer.LineRight();
            //}
            //if (e.Delta > 0)
            //{
            //    scrollviewer.LineUp();
            //    scrollviewer.LineUp();
            //    scrollviewer.LineUp();
            //}
            //else
            //{
            //    scrollviewer.LineDown();
            //    scrollviewer.LineDown();
            //    scrollviewer.LineDown();
            //}
            //e.Handled = true;
        }

        private void BtnGoLeft_Click(object sender, RoutedEventArgs e)
        {
            OnYearsPanel = true;
            BtnGoLeft.Visibility = Visibility.Hidden;
            BtnGoRight.Visibility = Visibility.Visible;
            LoadYears(CurBookName, TypeOfTree);
            BtnSelected = (WpYears.Children[yIndex] as Button);
            BtnSelected.BorderBrush = Brushes.DodgerBlue;
        }

        private void BtnGoRight_Click(object sender, RoutedEventArgs e)
        {
            TextBlock tbk = UTreeView.FindVisualChild<TextBlock>(BtnSelected);

            OnYearsPanel = false;
            BtnGoLeft.Visibility = Visibility.Visible;
            BtnGoRight.Visibility = Visibility.Hidden;

            WpYears.Children.Clear();
            TbYear.Visibility = Visibility.Visible;
            //TbYear.Text = BtnSelected.Content.ToString();
            TbYear.Text = tbk.Text;
            TbYear.Uid = BtnSelected.Uid;

            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("SELECT * FROM Tree_{0} where Pid='{1}';", tableName, BtnSelected.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                Button BtnTag = AddNode(reader["Uid"].ToString(), reader["NodeName"].ToString());
                WpYears.Children.Add(BtnTag);
            }
            reader.Close();
            sqlConn.Close();
        }


        Point _lastMouseDown;
        double lastOffset;
        private void Sv_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Mouse.SetCursor(Cursors.SizeNS);
                double distance = e.GetPosition(Sv).Y - _lastMouseDown.Y;
                Sv.ScrollToVerticalOffset(lastOffset - distance);
            }
            else
            {
                lastOffset = Sv.VerticalOffset;
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
            WrapPanel wp = new WrapPanel();
            BtnTag.Content = wp;
            Image img = new Image();
            img.VerticalAlignment = VerticalAlignment.Center;
            TextBlock tbk = new TextBlock();
            tbk.VerticalAlignment = VerticalAlignment.Center;
            wp.Children.Add(img);
            wp.Children.Add(tbk);
            tbk.Text = content;
            int x = 0;
            if (OnYearsPanel == true)
            {
                img.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resourse/ic_action_knight.png"));
            }
            else
            {
                img.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resourse/ic_action_attachment.png"));
                x = 25;
            }

            BtnTag.Padding = new Thickness(2);
            BtnTag.HorizontalAlignment = HorizontalAlignment.Left;
            BtnTag.Margin = new Thickness(5+x, 5, 0, 0);
            BtnTag.Click += BtnTag_Click;
            ContextMenu aMenu = new ContextMenu();
            MenuItem deleteMenu = new MenuItem();
            deleteMenu.Header = "删除";
            deleteMenu.Click += DeleteMenu_Click; ;
            aMenu.Items.Add(deleteMenu);
            BtnTag.ContextMenu = aMenu;
            aMenu.DataContext = BtnTag;
            BtnTag.Uid = guid;
            //BtnTag.Content = content;
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

        private void TbSelected_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBlock tbk = UTreeView.FindVisualChild<TextBlock>(BtnSelected);
                //BtnSelected.Content = TbSelected.Text;
                tbk.Text = TbSelected.Text;
                string tableName = TypeOfTree;
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, TbSelected.Text, TbSelected.Uid);
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();
            }
        }

        private void WpYears_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,

                Source = sender
            };

            WpYears.RaiseEvent(eventArg);
        }
    }
}
