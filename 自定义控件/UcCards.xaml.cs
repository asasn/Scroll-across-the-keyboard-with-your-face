using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcRoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class UcCards : UserControl
    {
        public UcCards()
        {
            InitializeComponent();
        }

        public string TypeOfTree;
        public string CurBookName;

        /// <summary>
        /// 刷新关键词着色
        /// </summary>
        void RefreshKeyWords()
        {
            foreach (HandyControl.Controls.TabItem tabItem in Gval.Uc.TabControl.Items)
            {
                UcEditor ucEditor = tabItem.Content as UcEditor;
                ucEditor.SetRules();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (false == string.IsNullOrEmpty(TbTab.Text))
            {
                string tableName = TypeOfTree;
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                SQLiteDataReader reader = sqlConn.ExecuteQuery(string.Format("select * from {0}主表 where 名称='{1}'", tableName, TbTab.Text));
                while (reader.Read())
                {
                    MessageBox.Show("数据库中已经存在同名条目，请修改成为其他名称！");
                    reader.Close();
                    sqlConn.Close();
                    return;
                }
                reader.Close();

                string guid = Guid.NewGuid().ToString();
                Button BtnTag = AddNode(guid, TbTab.Text);
                WpCards.Children.Add(BtnTag);

                TbTab.Clear();

                string sql = string.Format("insert or ignore into {0}主表 ({0}id, 名称) values ('{1}', '{2}');", tableName, BtnTag.Uid, BtnTag.Content.ToString());
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();

                RefreshKeyWords();
            }

        }

        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {
            Button BtnTag = sender as Button;
            if (TypeOfTree == "角色")
            {
                RoleCard roleCard = new RoleCard(CurBookName, TypeOfTree, BtnTag);
                roleCard.Show();
            }
            if (TypeOfTree == "其他")
            {
                InfoCard infoCard = new InfoCard(CurBookName, TypeOfTree, BtnTag);
                infoCard.Show();
            }
        }

        public void LoadCards(string curBookName, string typeOfTree)
        {
            if (string.IsNullOrEmpty(curBookName))
            {
                return;
            }

            WpCards.Children.Clear();

            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            WpCards.Tag = typeOfTree;

            //初始化顶层节点数据
            TreeViewNode TopNode = new TreeViewNode
            {
                Uid = "",
                IsDir = true
            };


            Gval.Flag.Loading = true;

            string tableName = typeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("SELECT * FROM {0}主表 ORDER BY 权重 DESC", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                Button BtnTag = AddNode(reader[string.Format("{0}id", tableName)].ToString(), reader["名称"].ToString());
                WpCards.Children.Add(BtnTag);
            }
            reader.Close();
            sqlConn.Close();


            Gval.Flag.Loading = false;
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            Button BtnTag = (sender as MenuItem).DataContext as Button;
            Console.WriteLine(BtnTag.Uid);
            WpCards.Children.Remove(BtnTag);

            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("DELETE from {0}主表 where {0}id='{1}';", tableName, BtnTag.Uid);
            sqlConn.ExecuteNonQuery(sql);
            sqlConn.Close();

            RefreshKeyWords();
        }

        private void WpCards_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TbTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAdd_Click(null, null);
            }
        }

        Button AddNode(string guid, string content)
        {
            Button BtnTag = new Button();
            BtnTag.Height = 25;
            BtnTag.Padding = new Thickness(2);
            BtnTag.Margin = new Thickness(5, 5, 2, 2);
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

            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("SELECT * FROM {0}别称表 where {0}id='{1}';", tableName, guid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                string name = reader["别称"].ToString();
                BtnTag.DataContext += name + " ";
            }
            BtnTag.DataContext += "";//防止为空，避免之后的判断
            if (false == string.IsNullOrWhiteSpace(BtnTag.DataContext.ToString()))
            {
                BtnTag.ToolTip = BtnTag.DataContext.ToString();
            }
            reader.Close();
            sqlConn.Close();
            return BtnTag;
        }
    }
}
