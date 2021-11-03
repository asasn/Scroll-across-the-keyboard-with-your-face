using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.公共操作类.TreeOperate;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcTreeBook.xaml 的交互逻辑
    /// </summary>
    public partial class UcTreeBook : UserControl
    {
        public UcTreeBook()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据源：节点列表
        /// </summary>
        ObservableCollection<TreeViewNode> TreeViewNodeList = new ObservableCollection<TreeViewNode>();


        private void Tv_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBook();
        }

        void LoadBook()
        {
            //数据初始化
            TreeViewNodeList = new ObservableCollection<TreeViewNode>();

            //数据源加载节点列表
            Tv.ItemsSource = TreeViewNodeList;

            //初始化顶层节点数据
            TreeViewNode TopNode = new TreeViewNode
            {
                Uid = "",
                IsDir = true
            };

            TreeViewNode one = AddNewNode(TreeViewNodeList, TopNode);
            AddNewNode(TreeViewNodeList, TopNode);
            AddNewNode(TreeViewNodeList, TopNode);

            AddNewNode(TreeViewNodeList, one);
            AddNewNode(TreeViewNodeList, one);
            AddNewNode(TreeViewNodeList, one);

            Gval.Flag.Loading = true;

            //从数据库中载入数据
            //Load(TreeViewNodeList, TopNode);

            AddButtonNode(TreeViewNodeList, TopNode);
            Gval.Flag.Loading = false;
        }



        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TreeViewMenu_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

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
            SettingsOperate.Set("curBookUid", bookCard.Uid);
            SettingsOperate.Set("curBookName", bookCard.Header.ToString());

            GetBookInfoForGval(bookCard.Uid);

            //Gval.ucNote.TvLoad();
            //Gval.ucTask.TvLoad();
            //Gval.ucRoleCard.TvLoad();
            //Gval.ucFactionCard.TvLoad();
            //Gval.ucGoodsCard.TvLoad();
            //Gval.ucCommonCard.TvLoad();
            //Gval.ucEditor.IsEnabled = false;
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

        #endregion

        private void TbNewBookName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string tableName = "books";
                string guid = Guid.NewGuid().ToString();
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
                string sql = string.Format("INSERT INTO Tree_{0} (Uid, Name, Price, BornYear, CurrentYear) VALUES ('{1}', '{2}', {3}, {4}, {5});", tableName, guid, TbNewBookName.Text, 0, 2000, 2021);
                sqlConn.ExecuteNonQuery(sql);

                TbNewBookName.Clear();
                TbNewBookName.IsEnabled = false;
                BtnBuild.Content = "创建新书籍";
                WpBooks.Children.Clear();
                DrawerLeftInContainer_Loaded(null, null);
            }
        }


        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
            string sql = string.Format("UPDATE Tree_{0} set Name='{1}', Price={2}, BornYear={3}, CurrentYear={4} where Uid = '{5}';", tableName, TbCurBookName.Text, Convert.ToDouble(TbCurBookPrice.Text), Convert.ToInt32(TbCurBookBornYear.Text), Convert.ToInt32(TbCurBookCurrentYear.Text), Gval.CurrentBook.Uid);
            sqlConn.ExecuteNonQuery(sql);

            GetBookInfoForGval(Gval.CurrentBook.Uid);
            DrawerTbk.Text = "当前书籍：" + TbCurBookName.Text;
            (WpBooks.Tag as HandyControl.Controls.Card).Header = TbCurBookName.Text;
        }

        private void BtnDelBook_Click(object sender, RoutedEventArgs e)
        {
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
            string sql = string.Format("DELETE from Tree_{0} where Uid='{1}';", tableName, Gval.CurrentBook.Uid);
            sqlConn.ExecuteNonQuery(sql);

            WpBooks.Children.Clear();
            DrawerLeftInContainer_Loaded(null, null);
        }
    }
}
