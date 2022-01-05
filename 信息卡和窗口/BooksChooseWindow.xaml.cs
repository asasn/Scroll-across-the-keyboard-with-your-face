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
using 脸滚键盘.控件方法类;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// BooksChooseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BooksChooseWindow : Window
    {
        public BooksChooseWindow()
        {
            InitializeComponent();
        }



        public string CurrentBookName
        {
            get { return (string)GetValue(CurrentBookNameProperty); }
            set { SetValue(CurrentBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentBookNameProperty =
            DependencyProperty.Register("CurrentBookName", typeof(string), typeof(BooksChooseWindow), new PropertyMetadata(null));



        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string curBookUid = SettingsOperate.Get("curBookUid");          
            string tableName = "books";
            SqliteOperate sqlConn = Gval.SQLClass.Pools["index"];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS Tree_{0} (Uid CHAR PRIMARY KEY, Name CHAR, Price DOUBLE, BornYear INTEGER, CurrentYear INTEGER);", tableName);
            sqlConn.ExecuteNonQuery(sql);
            sql = string.Format("SELECT * FROM Tree_{0};", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                string BookUid = reader["Uid"].ToString();
                string BookName = reader["Name"].ToString();
                HandyControl.Controls.Card bookCard = new HandyControl.Controls.Card();
                WpBooks.Children.Add(bookCard);
                //bookCard.Effect = 
                bookCard.Margin = new Thickness(10, 10, 0, 0);
                bookCard.Width = 180;
                bookCard.Height = 240;
                bookCard.VerticalAlignment = VerticalAlignment.Stretch;
                bookCard.HorizontalAlignment = HorizontalAlignment.Stretch;
                bookCard.Uid = BookUid;
                bookCard.Header = BookName;

                string imgPath = Gval.Path.Books + "/" + BookName + ".jpg";

                bookCard.Content = FileOperate.GetImgObject(imgPath);

                bookCard.MouseLeftButtonDown += CardSelected;

                if (BookUid == curBookUid)
                {
                    ChoseBookChange(bookCard);
                }
            }
            reader.Close();
            
        }


        /// <summary>
        /// 根据Uid获取当前书籍信息，并填入Gval公共类当中
        /// </summary>
        /// <param name="uid"></param>
        void GetBookInfoForGval(string uid)
        {
            Gval.CurrentBook.Uid = uid;
            string tableName = "books";
            SqliteOperate sqlConn = Gval.SQLClass.Pools["index"];
            string sql = string.Format("SELECT * FROM Tree_{0} where Uid='{1}';", tableName, Gval.CurrentBook.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                TbUid.Text = reader["Uid"].ToString();
                TbName.Text = reader["Name"].ToString();
                TbPrice.Text = reader["Price"].ToString();
                TbBornYear.Text = reader["BornYear"].ToString();
                TbCurrentYear.Text = reader["CurrentYear"].ToString();

                Gval.CurrentBook.Name = reader["Name"].ToString();
                Gval.CurrentBook.Price = Convert.ToDouble(reader["Price"]);
                Gval.CurrentBook.BornYear = Convert.ToInt32(reader["BornYear"]);
                Gval.CurrentBook.CurrentYear = Convert.ToInt32(reader["CurrentYear"]);
            }
            reader.Close();
            
        }

        /// <summary>
        /// 选择当前书籍卡片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardSelected(object sender, MouseButtonEventArgs e)
        {
            foreach (HandyControl.Controls.Card item in WpBooks.Children)
            {
                item.BorderBrush = null;
                item.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            HandyControl.Controls.Card bookCard = sender as HandyControl.Controls.Card;
            Gval.Uc.TabControl.Items.Clear();
            
            //在数据库占用和重复连接之间选择了一个平衡。保持连接会导致文件占用，不能及时同步和备份，过多重新连接则是不必要的开销。
            foreach (SqliteOperate sqlConn in Gval.SQLClass.Pools.Values)
            {
                sqlConn.Close();
            }

            ChoseBookChange(bookCard);
        }

        /// <summary>
        /// 根据选择的书籍卡片执行一些切换书籍的操作
        /// </summary>
        /// <param name="bookCard"></param>
        void ChoseBookChange(HandyControl.Controls.Card bookCard)
        {
            WpBooks.Tag = bookCard;

            bookCard.BorderBrush = Brushes.DodgerBlue;
            bookCard.BorderThickness = new Thickness(0, 5, 0, 5);

            SettingsOperate.Set("curBookUid", bookCard.Uid);
            SettingsOperate.Set("curBookName", bookCard.Header.ToString());
            if (false == Gval.SQLClass.Pools.ContainsKey(bookCard.Header.ToString()))
            {
                Gval.SQLClass.Pools.Add(bookCard.Header.ToString(), new SqliteOperate(Gval.Path.Books, bookCard.Header.ToString() + ".db"));
            }
            GetBookInfoForGval(bookCard.Uid);
            Gval.Uc.TreeBook.LoadBook(Gval.CurrentBook.Name, "book");
            Gval.Uc.TreeNote.LoadBook(Gval.CurrentBook.Name, "note");
            //Gval.Uc.HistoryBar.LoadYears(Gval.CurrentBook.Name, "history");            
            Gval.Uc.TreeTask.LoadBook(Gval.CurrentBook.Name, "task");
            CardOperate.TryToBuildBaseTable(Gval.CurrentBook.Name, "角色");
            Gval.Uc.RoleCards.LoadCards(Gval.CurrentBook.Name, "角色");
            CardOperate.TryToBuildBaseTable(Gval.CurrentBook.Name, "其他");
            Gval.Uc.OtherCards.LoadCards(Gval.CurrentBook.Name, "其他");

            Gval.Uc.MWindow.TbkCurBookName.Visibility = Visibility.Hidden;
            Gval.Uc.MWindow.TbkCurBookName2.Text = Gval.CurrentBook.Name;
            Gval.Uc.MWindow.TbkCurBookName2.Visibility = Visibility.Visible;
        }

        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbBuild.Text))
            {
                return;
            }
            TbBuild.Text = FileOperate.ReplaceFileName(TbBuild.Text);
            if (FileOperate.IsFileExists(Gval.Path.Books + "/" + TbBuild.Text + ".db") == true)
            {
                MessageBox.Show("该书籍已经存在\n请换一个新书名！", "Tip", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            string tableName = "books";
            string guid = Guid.NewGuid().ToString();
            SqliteOperate sqlConn = Gval.SQLClass.Pools["index"];
            string sql = string.Format("INSERT INTO Tree_{0} (Uid, Name, Price, BornYear, CurrentYear) VALUES ('{1}', '{2}', {3}, {4}, {5});", tableName, guid, TbBuild.Text.Replace("'", "''"), 0, 2000, 2021);
            sqlConn.ExecuteNonQuery(sql);
            

            TbBuild.Clear();
            WpBooks.Children.Clear();
            Window_Loaded(null, null);
        }



        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbPrice.Text) || string.IsNullOrWhiteSpace(TbBornYear.Text) || string.IsNullOrWhiteSpace(TbCurrentYear.Text) || WpBooks.Tag == null)
            {
                return;
            }
            string tableName = "books";
            SqliteOperate sqlConn = Gval.SQLClass.Pools["index"];
            string sql = string.Format("UPDATE Tree_{0} set Price={1}, BornYear={2}, CurrentYear={3} where Uid = '{4}';", tableName, Convert.ToDouble(TbPrice.Text), Convert.ToInt32(TbBornYear.Text), Convert.ToInt32(TbCurrentYear.Text), Gval.CurrentBook.Uid);
            sqlConn.ExecuteNonQuery(sql);
            
            GetBookInfoForGval(Gval.CurrentBook.Uid);
        }

        private void BtnDelBook_Click(object sender, RoutedEventArgs e)
        {
            if (WpBooks.Tag == null)
            {
                return;
            }
            MessageBoxResult dr = MessageBox.Show("真的要进行删除吗？\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (dr == MessageBoxResult.Cancel)
            {
                return;
            }

            //关闭数据库连接并从字典中删除
            if (true == Gval.SQLClass.Pools.ContainsKey((WpBooks.Tag as HandyControl.Controls.Card).Header.ToString()))
            {
                Gval.SQLClass.Pools[(WpBooks.Tag as HandyControl.Controls.Card).Header.ToString()].Close();
                Gval.SQLClass.Pools.Remove((WpBooks.Tag as HandyControl.Controls.Card).Header.ToString());
            }
            FileOperate.DeleteFile(Gval.Path.Books + "/" + (WpBooks.Tag as HandyControl.Controls.Card).Header.ToString() + ".db");
            DelCurBookBySql();
            WpBooks.Children.Clear();
            Window_Loaded(null, null);

            Gval.Uc.MWindow.TbkCurBookName2.Visibility = Visibility.Hidden;
            Gval.Uc.MWindow.TbkCurBookName.Text = "<<<点击选择或者创建书籍";
            Gval.Uc.MWindow.TbkCurBookName.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 从数据库中删除当前选定的书籍记录
        /// </summary>
        private static void DelCurBookBySql()
        {
            string tableName = "books";
            SqliteOperate sqlConn = Gval.SQLClass.Pools["index"];
            string sql = string.Format("DELETE from Tree_{0} where Uid='{1}';", tableName, Gval.CurrentBook.Uid);
            sqlConn.ExecuteNonQuery(sql);
        }

        private void TbCurBookBornYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int.TryParse(tb.Text, out int str);
            tb.Text = str.ToString();
        }

        private void TbCurBookCurrentYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int.TryParse(tb.Text, out int str);
            tb.Text = str.ToString();
        }

        private void TbCurBookPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            double.TryParse(tb.Text, out double str);
            tb.Text = str.ToString();
        }

        private void BtnName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbName.Text) || WpBooks.Tag == null)
            {
                return;
            }

            if (FileOperate.IsFileExists(Gval.Path.Books + "/" + TbName.Text + ".db") == true)
            {
                MessageBox.Show("该书籍已经存在\n请换一个新书名！", "Tip", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            else
            {
                //关闭数据库连接并从字典中删除
                if (true == Gval.SQLClass.Pools.ContainsKey(Gval.CurrentBook.Name))
                {
                    Gval.SQLClass.Pools[Gval.CurrentBook.Name].Close();
                    Gval.SQLClass.Pools.Remove(Gval.CurrentBook.Name);
                }
                TbName.Text = FileOperate.ReplaceFileName(TbName.Text);
                string oldName = Gval.Path.Books + "/" + Gval.CurrentBook.Name + ".db";
                string newName = Gval.Path.Books + "/" + TbName.Text + ".db";
                Gval.CurrentBook.Name = TbName.Text;
                FileOperate.RenameDoc(oldName, newName);
                string tableName = "books";
                SqliteOperate sqlConn = Gval.SQLClass.Pools["index"];
                string sql = string.Format("UPDATE Tree_{0} set Name='{1}' where Uid = '{2}';", tableName, TbName.Text.Replace("'", "''"), Gval.CurrentBook.Uid);
                sqlConn.ExecuteNonQuery(sql);
                

                GetBookInfoForGval(Gval.CurrentBook.Uid);
                (WpBooks.Tag as HandyControl.Controls.Card).Header = TbName.Text;

                if (false == Gval.SQLClass.Pools.ContainsKey(Gval.CurrentBook.Name))
                {
                    Gval.SQLClass.Pools.Add(Gval.CurrentBook.Name, new SqliteOperate(Gval.Path.Books, Gval.CurrentBook.Name + ".db")) ;
                }
            }
        }

        /// <summary>
        /// 防止输入数字之外的其他文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            double.TryParse(tb.Text, out double str);
            tb.Text = str.ToString();
        }

        private void TbBornYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int.TryParse(tb.Text, out int str);
            tb.Text = str.ToString();
        }

        private void TbCurrentYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int.TryParse(tb.Text, out int str);
            tb.Text = str.ToString();
        }

        private void TbBuild_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnBuild.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
