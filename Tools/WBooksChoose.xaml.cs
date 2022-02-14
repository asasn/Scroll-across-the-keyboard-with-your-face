using NSMain.Bricks;
using System;
using System.Collections.ObjectModel;
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
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools["index"];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS Tree_{0} (Uid CHAR PRIMARY KEY, NodeName CHAR NOT NULL, Introduction CHAR, Price DOUBLE, CurrentYear INTEGER DEFAULT (0), IsDel BOOLEAN DEFAULT (false));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS Tree_{0}Uid ON Tree_{0}(Uid);", tableName);
            cSqlite.ExecuteNonQuery(sql);
            sql = string.Format("SELECT * FROM Tree_{0};", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                string BookUid = reader["Uid"].ToString();
                string BookName = reader["NodeName"].ToString();

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
                    Cursor = Cursors.Hand,
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
        void GetCurBookInfoForGlobalVal(string uid)
        {
            GlobalVal.CurrentBook.Uid = uid;
            string tableName = "allbooks";
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools["index"];
            string sql = string.Format("SELECT * FROM Tree_{0} where Uid='{1}';", tableName, GlobalVal.CurrentBook.Uid);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                TbUid.Text = reader["Uid"].ToString();
                TbName.Text = reader["NodeName"].ToString();
                TbIntroduction.Text = reader["Introduction"].ToString();
                TbPrice.Text = reader["Price"].ToString();
                TbCurrentYear.Text = reader["CurrentYear"].ToString();

                GlobalVal.CurrentBook.Name = reader["NodeName"].ToString();
                GlobalVal.CurrentBook.Introduction = reader["Introduction"].ToString();
                GlobalVal.CurrentBook.Price = Convert.ToDouble(reader["Price"]);
                GlobalVal.CurrentBook.CurrentYear = Convert.ToInt64(reader["CurrentYear"]);

            }
            reader.Close();


        }

        void GetCurBookCurrentYear()
        {
            if (GlobalVal.Uc.TreeHistory != null && GlobalVal.Uc.TreeHistory.Tv.Items.Count > 0)
            {
                if ((GlobalVal.Uc.TreeHistory.Tv.Items.GetItemAt(GlobalVal.Uc.TreeHistory.Tv.Items.Count - 1) as TreeViewPlus.CNodeModule.TreeViewNode).NodeName.Contains("年"))
                {
                    TbCurrentYear.Text = (GlobalVal.Uc.TreeHistory.Tv.Items.GetItemAt(GlobalVal.Uc.TreeHistory.Tv.Items.Count - 1) as TreeViewPlus.CNodeModule.TreeViewNode).NodeName.Split('年')[0];
                    GlobalVal.CurrentBook.CurrentYear = Convert.ToInt64(TbCurrentYear.Text);
                }
            }
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
                item.Background = Brushes.White;
            }
            HandyControl.Controls.Card bookCard = sender as HandyControl.Controls.Card;
            GlobalVal.Uc.TabControl.Items.Clear();

            //在数据库占用和重复连接之间选择了一个平衡。保持连接会导致文件占用，不能及时同步和备份，过多重新连接则是不必要的开销。
            foreach (CSqlitePlus cSqlite in GlobalVal.SQLClass.Pools.Values)
            {
                cSqlite.Close();
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

            bookCard.Background = Brushes.DodgerBlue;

            ImgShow.Source = (bookCard.Content as Image).Source;

            CSettings.Set("curBookUid", bookCard.Uid);
            CSettings.Set("curBookName", bookCard.Header.ToString());
            if (false == GlobalVal.SQLClass.Pools.ContainsKey(bookCard.Header.ToString()))
            {
                GlobalVal.SQLClass.Pools.Add(bookCard.Header.ToString(), new CSqlitePlus(GlobalVal.Path.Books, bookCard.Header.ToString() + ".db"));
            }

            GetCurBookInfoForGlobalVal(bookCard.Uid);

            GlobalVal.Uc.TreeBook.LoadBook(GlobalVal.CurrentBook.Name, "book");
            GlobalVal.Uc.TreeHistory.LoadBook(GlobalVal.CurrentBook.Name, "history");
            GlobalVal.Uc.TreeTask.LoadBook(GlobalVal.CurrentBook.Name, "task");

            GlobalVal.Uc.RoleCards.LoadCards(GlobalVal.CurrentBook.Name, "角色");
            GlobalVal.Uc.OtherCards.LoadCards(GlobalVal.CurrentBook.Name, "其他");
            GlobalVal.Uc.WorldCards.LoadCards(GlobalVal.CurrentBook.Name, "世界");

            GetCurBookCurrentYear();

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
            string newBookName = TbBuild.Text = CFileOperate.ReplaceFileName(TbBuild.Text);
            if (CFileOperate.IsFileExists(GlobalVal.Path.Books + "/" + TbBuild.Text + ".db") == true)
            {
                MessageBox.Show("该书籍已经存在\n请换一个新书名！", "Tip", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            string tableName = "allbooks";
            string guid = Guid.NewGuid().ToString();
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools["index"];
            string sql = string.Format("INSERT INTO Tree_{0} (Uid, NodeName, Introduction, Price, CurrentYear) VALUES ('{1}', '{2}', '{3}', {4}, {5});", tableName, guid, TbBuild.Text.Replace("'", "''"), "", 0, 2021);
            cSqlite.ExecuteNonQuery(sql);


            TbBuild.Clear();
            WpBooks.Children.Clear();
            Window_Loaded(null, null);

            if (false == GlobalVal.SQLClass.Pools.ContainsKey(newBookName))
            {
                GlobalVal.SQLClass.Pools.Add(newBookName, new CSqlitePlus(GlobalVal.Path.Books, newBookName + ".db"));
            }
            TryToBuildTreeTable("index", "material");
            TryToBuildNewProjectTable("index");
            TryToBuildNotesTable("index");
            TryToBuildSettingTable("index");
            TryToBuildCardTable("index", "角色");
            TryToBuildCardTable("index", "其他");
            TryToBuildCardTable("index", "世界");

            TryToBuildMapsTable(newBookName);
            TryToBuildScenesTable(newBookName);
            TryToBuildSettingTable(newBookName);

            TryToBuildCardTable(newBookName, "角色");
            TryToBuildCardTable(newBookName, "其他");
            TryToBuildCardTable(newBookName, "世界");
            TryToBuildNewBookCards(newBookName);

            TryToBuildTreeTable(newBookName, "book");
            TryToBuildTreeTable(newBookName, "history");
            TryToBuildTreeTable(newBookName, "task");
        }

        void TryToBuildNewProjectTable(string curBookName = "index")
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS 题材主表 (Uid CHAR PRIMARY KEY, 索引 INTEGER, 标题 CHAR, 内容 CHAR, IsDel BOOLEAN DEFAULT (false));");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 题材主表Uid ON 题材主表(Uid);");
            sql += string.Format("CREATE TABLE IF NOT EXISTS 题材从表 (Uid CHAR PRIMARY KEY, Pid CHAR REFERENCES 题材主表 (Uid) ON DELETE CASCADE ON UPDATE CASCADE, Tid CHAR, Text CHAR);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 题材从表Uid ON 题材从表(Uid);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 题材从表Pid ON 题材从表(Pid);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 题材从表Tid ON 题材从表(Tid);");
            cSqlite.ExecuteNonQuery(sql);
        }

        void TryToBuildTreeTable(string curBookName, string typeOfTree)
        {
            string tableName = typeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS Tree_{0} (Uid CHAR PRIMARY KEY, Pid CHAR, NodeName CHAR, isDir BOOLEAN, NodeContent TEXT, WordsCount INTEGER, IsExpanded BOOLEAN, IsChecked BOOLEAN, IsDel BOOLEAN DEFAULT (false));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS Tree_{0}Uid ON Tree_{0}(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS Tree_{0}Pid ON Tree_{0}(Pid);", tableName);
            cSqlite.ExecuteNonQuery(sql);
        }

        void TryToBuildScenesTable(string curBookName)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS 场记大纲表 (Uid CHAR PRIMARY KEY, 索引 INTEGER, 标题 CHAR, 内容 CHAR, IsDel BOOLEAN DEFAULT (false));");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 场记大纲表Uid ON 场记大纲表(Uid);");
            cSqlite.ExecuteNonQuery(sql);
        }

        void TryToBuildMapsTable(string curBookName)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS 地图地点表 (Uid CHAR PRIMARY KEY, Pid CHAR, 名称 CHAR, 备注 CHAR, PointX INTEGER DEFAULT (0), PointY INTEGER DEFAULT (0),IsDel BOOLEAN DEFAULT (false));");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 地图地点表Uid ON 地图地点表(Uid);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 地图地点表Pid ON 地图地点表(Pid);");
            cSqlite.ExecuteNonQuery(sql);
        }

        void TryToBuildNotesTable(string curBookName = "index")
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS 随手记录表 (Uid CHAR PRIMARY KEY, 索引 INTEGER, 标题 CHAR, 内容 CHAR, IsDel BOOLEAN DEFAULT (false));");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 随手记录表Uid ON 随手记录表(Uid);");
            cSqlite.ExecuteNonQuery(sql);
        }
        void TryToBuildCardTable(string curBookName, string typeOfTree)
        {
            string tableName = typeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}主表 (Uid CHAR PRIMARY KEY, 名称 CHAR UNIQUE, 备注 CHAR, 权重 INTEGER DEFAULT (0), 诞生年份 INTEGER DEFAULT (0), IsDel BOOLEAN DEFAULT (false));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}主表Uid ON {0}主表(Uid);", tableName);

            sql += string.Format("CREATE TABLE IF NOT EXISTS {0}属性表 (Uid CHAR PRIMARY KEY, Text CHAR NOT NULL UNIQUE);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}属性表Uid ON {0}属性表(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}属性表Text ON {0}属性表(Text);", tableName);

            sql += string.Format("CREATE TABLE IF NOT EXISTS {0}从表 (Uid CHAR PRIMARY KEY, Pid CHAR REFERENCES {0}主表 (Uid) ON DELETE CASCADE ON UPDATE CASCADE, Tid CHAR REFERENCES {0}属性表 (Uid) ON DELETE CASCADE ON UPDATE CASCADE, Text CHAR);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}从表Uid ON {0}从表(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}从表Pid ON {0}从表(Pid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}从表Tid ON {0}从表(Tid);", tableName);

            sql += string.Format("insert or ignore into {0}属性表 (Uid, Text) values ('{1}', '{2}');", tableName, Guid.NewGuid().ToString(), "别称");

            cSqlite.ExecuteNonQuery(sql);
        }

        void TryToBuildNewBookCards(string curBookName)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("insert or ignore into {0} (Uid, 名称) values ('{1}', '{2}');", "角色主表", Guid.NewGuid().ToString(), "主角");
            sql += string.Format("insert or ignore into {0} (Uid, 名称) values ('{1}', '{2}');", "世界主表", Guid.NewGuid().ToString(), "主题");
            sql += string.Format("insert or ignore into {0} (Uid, 名称) values ('{1}', '{2}');", "世界主表", Guid.NewGuid().ToString(), "风格");
            sql += string.Format("insert or ignore into {0} (Uid, 名称) values ('{1}', '{2}');", "世界主表", Guid.NewGuid().ToString(), "卖点");
            cSqlite.ExecuteNonQuery(sql);
        }

        void TryToBuildSettingTable(string curBookName)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string tableName = string.Empty;
            if (curBookName == "index")
            {
                tableName = "公共";
            }
            else
            {
                tableName = "本书";
            }

            string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}设置表 (Key CHAR PRIMARY KEY, Value CHAR);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}设置表Key ON {0}设置表(Key);", tableName);
            cSqlite.ExecuteNonQuery(sql);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (WpBooks.Tag == null)
            {
                return;
            }
            string tableName = "allbooks";
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools["index"];
            string sql = string.Format("UPDATE Tree_{0} set Introduction='{1}', Price={2}, CurrentYear={3} where Uid = '{4}';", tableName, TbIntroduction.Text.Replace("'", "''"), Convert.ToDouble(TbPrice.Text), Convert.ToInt32(TbCurrentYear.Text), GlobalVal.CurrentBook.Uid);
            cSqlite.ExecuteNonQuery(sql);

            GetCurBookInfoForGlobalVal(GlobalVal.CurrentBook.Uid);
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

            //回收站：FileOperate.DeleteFile(GlobalVal.Path.Books + "/" + (WpBooks.Tag as HandyControl.Controls.Card).Header.ToString() + ".db");
            DelCurBookBySql();
            (WpBooks.Tag as HandyControl.Controls.Card).Header.ToString();
            WpBooks.Children.Clear();
            Window_Loaded(null, null);
            (WpBooks.Tag as HandyControl.Controls.Card).Header.ToString();
            //关闭数据库连接并从字典中删除（注意执行此操作的位置，这里要避免取值被清空，或者关闭之后重新因为删除语句而打开）
            if (true == GlobalVal.SQLClass.Pools.ContainsKey((WpBooks.Tag as HandyControl.Controls.Card).Header.ToString()))
            {
                GlobalVal.SQLClass.Pools[(WpBooks.Tag as HandyControl.Controls.Card).Header.ToString()].Close();
                GlobalVal.SQLClass.Pools.Remove((WpBooks.Tag as HandyControl.Controls.Card).Header.ToString());
            }
            Console.WriteLine((WpBooks.Tag as HandyControl.Controls.Card).Header.ToString());
            GlobalVal.Uc.MainWin.TbkCurBookName2.Visibility = Visibility.Hidden;
            GlobalVal.Uc.MainWin.TbkCurBookName.Text = "<<<点击选择或者创建书籍";
            GlobalVal.Uc.MainWin.TbkCurBookName.Visibility = Visibility.Visible;

            GetCurBookInfoForGlobalVal();
            ClearBookControl();



        }

        /// <summary>
        /// 当前选择的书籍为空时，GlobalVal指向null
        /// </summary>
        /// <param name="uid"></param>
        private void GetCurBookInfoForGlobalVal()
        {
            GlobalVal.CurrentBook.Uid = null;
            GlobalVal.CurrentBook.Name = null;
            GlobalVal.CurrentBook.Introduction = null;
            GlobalVal.CurrentBook.Price = double.NaN;
            GlobalVal.CurrentBook.CurrentYear = 0;
            GlobalVal.CurrentBook.CurNode = null;

        }

        private void ClearBookControl()
        {  
            GlobalVal.Uc.TreeBook.Tv.ItemsSource = new ObservableCollection<TreeViewPlus.CNodeModule.TreeViewNode>();
            GlobalVal.Uc.TreeHistory.Tv.ItemsSource = new ObservableCollection<TreeViewPlus.CNodeModule.TreeViewNode>();
            GlobalVal.Uc.TreeTask.Tv.ItemsSource = new ObservableCollection<TreeViewPlus.CNodeModule.TreeViewNode>();
            GlobalVal.Uc.Searcher.ListBoxOfResults.Items.Clear();
            GlobalVal.Uc.RoleCards.WpCards.Children.Clear();
            GlobalVal.Uc.OtherCards.WpCards.Children.Clear();
            GlobalVal.Uc.WorldCards.WpCards.Children.Clear();
        }

        /// <summary>
        /// 从数据库中删除当前选定的书籍记录
        /// </summary>
        private static void DelCurBookBySql()
        {
            string tableName = "allbooks";
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools["index"];
            //回收站：string sql = string.Format("DELETE from Tree_{0} where Uid='{1}';", tableName, GlobalVal.CurrentBook.Uid);
            string sql = string.Format("update Tree_{0} set IsDel=True where Uid='{1}';", tableName, GlobalVal.CurrentBook.Uid);
            cSqlite.ExecuteNonQuery(sql);
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
                CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools["index"];
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, TbName.Text.Replace("'", "''"), GlobalVal.CurrentBook.Uid);
                cSqlite.ExecuteNonQuery(sql);


                GetCurBookInfoForGlobalVal(GlobalVal.CurrentBook.Uid);
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

        private void TbIntroduction_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
        }

        private void TbCurrentYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            long.TryParse(tb.Text, out long str);
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
