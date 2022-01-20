using NSMain.Bricks;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NSMain.Tools
{
    /// <summary>
    /// BooksChooseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WBooksChoose : Window
    {
        public WBooksChoose()
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
            DependencyProperty.Register("CurrentBookName", typeof(string), typeof(WBooksChoose), new PropertyMetadata(null));



        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string curBookUid = CSettings.Get("curBookUid");
            string tableName = "allbooks";
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools["index"];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS Tree_{0} (Uid CHAR PRIMARY KEY, Name CHAR, Price DOUBLE, BornYear INTEGER, CurrentYear INTEGER, IsDel BOOLEAN DEFAULT (false));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS Tree_{0}Uid ON Tree_{0}(Uid);", tableName);
            sqlConn.ExecuteNonQuery(sql);
            sql = string.Format("SELECT * FROM Tree_{0};", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                string BookUid = reader["Uid"].ToString();
                string BookName = reader["Name"].ToString();

                string imgPath = GlobalVal.Path.Books + "/" + BookName + ".jpg";
                if (false == CFileOperate.IsFileExists(imgPath))
                {
                    imgPath = GlobalVal.Path.Resourses + "/nullbookface.jpg";
                }
                Image imgBook = new Image
                {
                    Source = CFileOperate.GetImgObject(imgPath),
                    Width = 108,
                    Height = 162,
                };
                HandyControl.Controls.Card bookCard = new HandyControl.Controls.Card
                {
                    Margin = new Thickness(10, 10, 0, 0),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Uid = BookUid,
                    Header = BookName,
                    Content = imgBook,
                };

                bookCard.MouseLeftButtonDown += CardSelected;
                WpBooks.Children.Add(bookCard);
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
            GlobalVal.CurrentBook.Uid = uid;
            string tableName = "allbooks";
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools["index"];
            string sql = string.Format("SELECT * FROM Tree_{0} where Uid='{1}';", tableName, GlobalVal.CurrentBook.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                TbUid.Text = reader["Uid"].ToString();
                TbName.Text = reader["Name"].ToString();
                TbPrice.Text = reader["Price"].ToString();
                TbBornYear.Text = reader["BornYear"].ToString();
                TbCurrentYear.Text = reader["CurrentYear"].ToString();

                GlobalVal.CurrentBook.Name = reader["Name"].ToString();
                GlobalVal.CurrentBook.Price = Convert.ToDouble(reader["Price"]);
                GlobalVal.CurrentBook.BornYear = Convert.ToInt32(reader["BornYear"]);
                GlobalVal.CurrentBook.CurrentYear = Convert.ToInt32(reader["CurrentYear"]);
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
            GlobalVal.Uc.TabControl.Items.Clear();

            //在数据库占用和重复连接之间选择了一个平衡。保持连接会导致文件占用，不能及时同步和备份，过多重新连接则是不必要的开销。
            foreach (CSqlitePlus sqlConn in GlobalVal.SQLClass.Pools.Values)
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
            ImgShow.Source = (bookCard.Content as Image).Source;

            CSettings.Set("curBookUid", bookCard.Uid);
            CSettings.Set("curBookName", bookCard.Header.ToString());
            if (false == GlobalVal.SQLClass.Pools.ContainsKey(bookCard.Header.ToString()))
            {
                GlobalVal.SQLClass.Pools.Add(bookCard.Header.ToString(), new CSqlitePlus(GlobalVal.Path.Books, bookCard.Header.ToString() + ".db"));
            }
            GetBookInfoForGval(bookCard.Uid);
            GlobalVal.Uc.TreeBook.LoadBook(GlobalVal.CurrentBook.Name, "book");
            GlobalVal.Uc.TreeNote.LoadBook(GlobalVal.CurrentBook.Name, "note");
            //GlobalVal.Uc.HistoryBar.LoadYears(GlobalVal.CurrentBook.Name, "history");            
            GlobalVal.Uc.TreeTask.LoadBook(GlobalVal.CurrentBook.Name, "task");

            GlobalVal.Uc.RoleCards.LoadCards(GlobalVal.CurrentBook.Name, "角色");
            GlobalVal.Uc.OtherCards.LoadCards(GlobalVal.CurrentBook.Name, "其他");
            GlobalVal.Uc.WorldCards.LoadCards(GlobalVal.CurrentBook.Name, "世界");
            GlobalVal.Uc.MainWin.TbkCurBookName.Visibility = Visibility.Hidden;
            GlobalVal.Uc.MainWin.TbkCurBookName2.Text = GlobalVal.CurrentBook.Name;
            GlobalVal.Uc.MainWin.TbkCurBookName2.Visibility = Visibility.Visible;
        }

        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbBuild.Text))
            {
                return;
            }
            TbBuild.Text = CFileOperate.ReplaceFileName(TbBuild.Text);
            if (CFileOperate.IsFileExists(GlobalVal.Path.Books + "/" + TbBuild.Text + ".db") == true)
            {
                MessageBox.Show("该书籍已经存在\n请换一个新书名！", "Tip", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            string tableName = "allbooks";
            string guid = Guid.NewGuid().ToString();
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools["index"];
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
            string tableName = "allbooks";
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools["index"];
            string sql = string.Format("UPDATE Tree_{0} set Price={1}, BornYear={2}, CurrentYear={3} where Uid = '{4}';", tableName, Convert.ToDouble(TbPrice.Text), Convert.ToInt32(TbBornYear.Text), Convert.ToInt32(TbCurrentYear.Text), GlobalVal.CurrentBook.Uid);
            sqlConn.ExecuteNonQuery(sql);

            GetBookInfoForGval(GlobalVal.CurrentBook.Uid);
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
            if (true == GlobalVal.SQLClass.Pools.ContainsKey((WpBooks.Tag as HandyControl.Controls.Card).Header.ToString()))
            {
                GlobalVal.SQLClass.Pools[(WpBooks.Tag as HandyControl.Controls.Card).Header.ToString()].Close();
                GlobalVal.SQLClass.Pools.Remove((WpBooks.Tag as HandyControl.Controls.Card).Header.ToString());
            }
            //回收站：FileOperate.DeleteFile(GlobalVal.Path.Books + "/" + (WpBooks.Tag as HandyControl.Controls.Card).Header.ToString() + ".db");
            DelCurBookBySql();
            WpBooks.Children.Clear();
            Window_Loaded(null, null);

            GlobalVal.Uc.MainWin.TbkCurBookName2.Visibility = Visibility.Hidden;
            GlobalVal.Uc.MainWin.TbkCurBookName.Text = "<<<点击选择或者创建书籍";
            GlobalVal.Uc.MainWin.TbkCurBookName.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 从数据库中删除当前选定的书籍记录
        /// </summary>
        private static void DelCurBookBySql()
        {
            string tableName = "allbooks";
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools["index"];
            //回收站：string sql = string.Format("DELETE from Tree_{0} where Uid='{1}';", tableName, GlobalVal.CurrentBook.Uid);
            string sql = string.Format("update Tree_{0} set IsDel=True where Uid='{1}';", tableName, GlobalVal.CurrentBook.Uid);
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

            if (CFileOperate.IsFileExists(GlobalVal.Path.Books + "/" + TbName.Text + ".db") == true)
            {
                MessageBox.Show("该书籍已经存在\n请换一个新书名！", "Tip", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            else
            {
                //关闭数据库连接并从字典中删除
                if (true == GlobalVal.SQLClass.Pools.ContainsKey(GlobalVal.CurrentBook.Name))
                {
                    GlobalVal.SQLClass.Pools[GlobalVal.CurrentBook.Name].Close();
                    GlobalVal.SQLClass.Pools.Remove(GlobalVal.CurrentBook.Name);
                }
                TbName.Text = CFileOperate.ReplaceFileName(TbName.Text);
                string oldNameDB = GlobalVal.Path.Books + "/" + GlobalVal.CurrentBook.Name + ".db";
                string newNameDB = GlobalVal.Path.Books + "/" + TbName.Text + ".db";
                string oldNameJpg = GlobalVal.Path.Books + "/" + GlobalVal.CurrentBook.Name + ".jpg";
                string newNameJpg = GlobalVal.Path.Books + "/" + TbName.Text + ".jpg";
                GlobalVal.CurrentBook.Name = TbName.Text;
                CFileOperate.RenameFile(oldNameDB, newNameDB);
                CFileOperate.RenameFile(oldNameJpg, newNameJpg);

                string tableName = "allbooks";
                CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools["index"];
                string sql = string.Format("UPDATE Tree_{0} set Name='{1}' where Uid = '{2}';", tableName, TbName.Text.Replace("'", "''"), GlobalVal.CurrentBook.Uid);
                sqlConn.ExecuteNonQuery(sql);


                GetBookInfoForGval(GlobalVal.CurrentBook.Uid);
                (WpBooks.Tag as HandyControl.Controls.Card).Header = TbName.Text;

                if (false == GlobalVal.SQLClass.Pools.ContainsKey(TbName.Text))
                {
                    GlobalVal.SQLClass.Pools.Add(TbName.Text, new CSqlitePlus(GlobalVal.Path.Books, TbName.Text));
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
