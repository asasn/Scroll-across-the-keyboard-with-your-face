using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using 脸滚键盘.控件方法类;
using 脸滚键盘.自定义控件;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Mw_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.MWindow = this;
            Gval.Uc.BooksPanel = this.BooksPanel;

        }

        private void UcTreeBook_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeBook = sender as UcTreeBook;
        }

        private void UcTreeMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeMaterial = sender as UcTreeBook;
            Gval.Uc.TreeMaterial.LoadBook("index", "material");
        }

        private void UcTreeNote_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeNote = sender as UcTreeBook;
        }

        private void UcTreeTask_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TreeTask = sender as UcTreeTask;
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.TabControl = sender as HandyControl.Controls.TabControl;
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            foreach (HandyControl.Controls.TabItem tabItem in Gval.Uc.TabControl.Items)
            {
                tabItem.Focus();
                UEditor.TabItemClosing(tabItem, e);
            }
            Application.Current.Shutdown();
        }

        private void RoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.RoleCards = sender as UcCards;
        }

        private void OtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.OtherCards = sender as UcCards;
        }

        private void PublicRoleCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicRoleCards = sender as UcCards;
            Gval.Uc.PublicRoleCards.WpCards.Children.Clear();
            Gval.Uc.PublicRoleCards.LoadCards("index", "角色");
            CardOperate.TryToBuildBaseTable("index", "角色");
        }

        private void PublicOtherCards_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Uc.PublicOtherCards = sender as UcCards;
            Gval.Uc.PublicOtherCards.WpCards.Children.Clear();
            Gval.Uc.PublicOtherCards.LoadCards("index", "其他");
            CardOperate.TryToBuildBaseTable("index", "其他");
        }


        #region 向上/向下调整节点


        /// <summary>
        /// 事件：向上调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            UTreeView.NodeMoveUp(Gval.CurrentBook.Name, "book", (TreeViewNode)Gval.Uc.TreeBook.Tv.SelectedItem, Gval.Uc.TreeBook.TreeViewNodeList);
        }

        /// <summary>
        /// 事件：向下调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            UTreeView.NodeMoveDown(Gval.CurrentBook.Name, "book", (TreeViewNode)Gval.Uc.TreeBook.Tv.SelectedItem, Gval.Uc.TreeBook.TreeViewNodeList);
        }


        #endregion


        #region 书籍目录抽屉

        /// <summary>
        /// 展示书籍目录抽屉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowBooks_Click(object sender, RoutedEventArgs e)
        {
            DrawerLeftInContainer.IsOpen = !DrawerLeftInContainer.IsOpen;
        }

        private void DrawerLeftInContainer_Loaded(object sender, RoutedEventArgs e)
        {
            //如果处在设计模式中则返回
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) { return; }

            Gval.Uc.BooksDrawer = sender as HandyControl.Controls.Drawer;
            Gval.CurrentBook.Uid = SettingsOperate.Get("curBookUid");
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
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

                if (BookUid == Gval.CurrentBook.Uid)
                {
                    ChoseBookChange(bookCard);
                }
            }
            reader.Close();
            sqlConn.Close();
        }

        /// <summary>
        /// 根据Uid获取当前书籍信息，并填入Gval公共类当中
        /// </summary>
        /// <param name="uid"></param>
        void GetBookInfoForGval(string uid)
        {
            Gval.CurrentBook.Uid = uid;
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
            string sql = string.Format("SELECT * FROM Tree_{0} where Uid='{1}';", tableName, Gval.CurrentBook.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                TbCurBookName.Text = reader["Name"].ToString();
                TbCurBookPrice.Text = reader["Price"].ToString();
                TbCurBookBornYear.Text = reader["BornYear"].ToString();
                TbCurBookCurrentYear.Text = reader["CurrentYear"].ToString();

                Gval.CurrentBook.Name = reader["Name"].ToString();
                Gval.CurrentBook.Price = Convert.ToDouble(reader["Price"]);
                Gval.CurrentBook.BornYear = Convert.ToInt32(reader["BornYear"]);
                Gval.CurrentBook.CurrentYear = Convert.ToInt32(reader["CurrentYear"]);
            }
            reader.Close();
            sqlConn.Close();
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

            DrawerTbk.Text = "当前书籍：" + bookCard.Header.ToString();
            TbkCurBookName.Text = bookCard.Header.ToString();
            SettingsOperate.Set("curBookUid", bookCard.Uid);
            SettingsOperate.Set("curBookName", bookCard.Header.ToString());
            GetBookInfoForGval(bookCard.Uid);
            Gval.Uc.TreeBook.LoadBook(Gval.CurrentBook.Name, "book");
            Gval.Uc.TreeNote.LoadBook(Gval.CurrentBook.Name, "note");
            //Gval.Uc.HistoryBar.LoadYears(Gval.CurrentBook.Name, "history");            
            Gval.Uc.TreeTask.LoadBook(Gval.CurrentBook.Name, "task");            
            CardOperate.TryToBuildBaseTable(Gval.CurrentBook.Name, "角色");
            Gval.Uc.RoleCards.LoadCards(Gval.CurrentBook.Name, "角色");
            CardOperate.TryToBuildBaseTable(Gval.CurrentBook.Name, "其他");
            Gval.Uc.OtherCards.LoadCards(Gval.CurrentBook.Name, "其他");
        }

        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (TbNewBookName.IsEnabled == false)
            {
                TbNewBookName.IsEnabled = true;
                TbNewBookName.Text = "新书籍";
                TbNewBookName.SelectAll();
                BtnBuild.Content = "取消创建";
            }
            else
            {
                BtnBuild.Content = "创建新书籍";
                TbNewBookName.Clear();
                TbNewBookName.IsEnabled = false;
            }

        }

        private void TbNewBookName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string tableName = "books";
                string guid = Guid.NewGuid().ToString();
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
                string sql = string.Format("INSERT INTO Tree_{0} (Uid, Name, Price, BornYear, CurrentYear) VALUES ('{1}', '{2}', {3}, {4}, {5});", tableName, guid, TbNewBookName.Text, 0, 2000, 2021);
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();

                TbNewBookName.Clear();
                TbNewBookName.IsEnabled = false;
                BtnBuild.Content = "创建新书籍";
                WpBooks.Children.Clear();
                DrawerLeftInContainer_Loaded(null, null);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string oldName = Gval.Path.Books + "/" + Gval.CurrentBook.Name + ".db";
            string newName = Gval.Path.Books + "/" + TbCurBookName.Text + ".db";
            Gval.CurrentBook.Name = TbCurBookName.Text;
            if (FileOperate.IsFileExists(Gval.Path.Books + "/" + TbCurBookName.Text + ".db") == true)
            {
                MessageBoxResult dr = MessageBox.Show("该书籍已经存在\n请换一个新书名或者删除旧数据库（谨慎）！", "Tip", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                string tableName = "books";
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
                string sql = string.Format("UPDATE Tree_{0} set Price={1}, BornYear={2}, CurrentYear={3} where Uid = '{4}';", tableName, Convert.ToDouble(TbCurBookPrice.Text), Convert.ToInt32(TbCurBookBornYear.Text), Convert.ToInt32(TbCurBookCurrentYear.Text), Gval.CurrentBook.Uid);
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();
                GetBookInfoForGval(Gval.CurrentBook.Uid);
                return;
            }
            else
            {
                SQLiteConnection.ClearAllPools();
                FileOperate.renameDoc(oldName, newName);
                string tableName = "books";
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
                string sql = string.Format("UPDATE Tree_{0} set Name='{1}', Price={2}, BornYear={3}, CurrentYear={4} where Uid = '{5}';", tableName, TbCurBookName.Text, Convert.ToDouble(TbCurBookPrice.Text), Convert.ToInt32(TbCurBookBornYear.Text), Convert.ToInt32(TbCurBookCurrentYear.Text), Gval.CurrentBook.Uid);
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();

                GetBookInfoForGval(Gval.CurrentBook.Uid);
                DrawerTbk.Text = "当前书籍：" + TbCurBookName.Text;
                (WpBooks.Tag as HandyControl.Controls.Card).Header = TbCurBookName.Text;
                TbkCurBookName.Text = TbCurBookName.Text;
            }
        }

        private void BtnDelBook_Click(object sender, RoutedEventArgs e)
        {
            UTreeView.DelCurBookBySql();
            WpBooks.Children.Clear();
            DrawerLeftInContainer_Loaded(null, null);
        }

        private void TbCurBookBornYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int str;
            int.TryParse(tb.Text, out str);
            tb.Text = str.ToString();
        }

        private void TbCurBookCurrentYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int str;
            int.TryParse(tb.Text, out str);
            tb.Text = str.ToString();
        }

        private void TbCurBookPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            double str;
            double.TryParse(tb.Text, out str);
            tb.Text = str.ToString();
        }

        #endregion

        private void NameTool_Click(object sender, RoutedEventArgs e)
        {
            NameToolWindow ntWin = new NameToolWindow();
            ntWin.ShowDialog();
        }
    }
}
