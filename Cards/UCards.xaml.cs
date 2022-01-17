using NSMain.Bricks;
using NSMain.Editor;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static NSMain.Bricks.CTreeView;

namespace NSMain.Cards
{
    /// <summary>
    /// UcRoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class UCards : UserControl
    {
        public UCards()
        {
            InitializeComponent();
        }

        public string TypeOfTree;
        public string CurBookName;

        /// <summary>
        /// 刷新关键词着色
        /// </summary>
        public void RefreshKeyWords()
        {
            foreach (HandyControl.Controls.TabItem tabItem in GlobalVal.Uc.TabControl.Items)
            {
                UEditor ucEditor = tabItem.Content as UEditor;
                ucEditor.SetEditorColorRules();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TypeOfTree == null || string.IsNullOrEmpty(TbTab.Text))
            {
                return;
            }
            string tableName = TypeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            SQLiteDataReader reader = sqlConn.ExecuteQuery(string.Format("select * from {0}主表 where 名称='{1}'", tableName, TbTab.Text.Replace("'", "''")));
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                MessageBox.Show("数据库中已经存在同名条目，请修改成为其他名称！");
                reader.Close();

                return;
            }
            reader.Close();

            string guid = Guid.NewGuid().ToString();
            Button BtnTag = AddNode(guid, TbTab.Text);
            WpCards.Children.Add(BtnTag);

            TbTab.Clear();

            string sql = string.Format("insert or ignore into {0}主表 ({0}id, 名称) values ('{1}', '{2}');", tableName, BtnTag.Uid, BtnTag.Content.ToString().Replace("'", "''"));
            sqlConn.ExecuteNonQuery(sql);


            RefreshKeyWords();
            MarkNamesInChapter();

        }

        /// <summary>
        /// 标记Card中button
        /// </summary>
        public void MarkNamesInChapter()
        {
            if (CurBookName == null)
            {
                return;
            }
            TreeViewNode curNode = GlobalVal.CurrentBook.CurNode;
            if (curNode == null)
            {
                curNode = new TreeViewNode();
            }
            WrapPanel[] wps = { GlobalVal.Uc.RoleCards.WpCards, GlobalVal.Uc.OtherCards.WpCards };
            foreach (WrapPanel wp in wps)
            {
                foreach (Button button in wp.Children)
                {
                    //先清空ToolTip的内容
                    button.ToolTip = null;
                    string tableName = wp.Tag.ToString();
                    CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
                    string sql = string.Format("SELECT 别称 FROM {0}别称表 where {0}id='{1}';", tableName, button.Uid);
                    SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
                    while (reader.Read())
                    {
                        button.ToolTip += reader["别称"].ToString() + " ";
                    }
                    reader.Close();
                    if (curNode.NodeContent.Contains(button.Content.ToString()) || IsContainsNickname(button.ToolTip, curNode.NodeContent))
                    {
                        button.Background = Brushes.Cornsilk;
                    }
                    else
                    {
                        //button.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
                        button.Background = Brushes.White;
                    }
                }
            }
        }

        /// <summary>
        /// 判断文章内是否包含别称
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="nodeContent"></param>
        /// <returns></returns>
        bool IsContainsNickname(object toolTip, string nodeContent)
        {
            if (toolTip == null)
            {
                return false;
            }
            string[] sArray = toolTip.ToString().Split(new char[] { ' ' });
            foreach (string ss in sArray)
            {
                if (false == string.IsNullOrWhiteSpace(ss.Trim()) && nodeContent.Contains(ss.Trim()))
                {
                    return true;
                }
            }
            return false;
        }

        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {
            Button BtnTag = sender as Button;
            if (TypeOfTree == "角色")
            {
                //RoleCard roleCard = new RoleCard(CurBookName, TypeOfTree, BtnTag);
                //roleCard.Show();
            }
            if (TypeOfTree == "其他")
            {
                //InfoCard infoCard = new InfoCard(CurBookName, TypeOfTree, BtnTag);
                //infoCard.Show();

            }

            WCards wCards = new WCards(CurBookName, TypeOfTree, BtnTag);
            wCards.Show();
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
            //TreeViewNode TopNode = new TreeViewNode
            //{
            //    Uid = "",
            //    IsDir = true
            //};


            CardOperate.TryToBuildBaseTable(curBookName, typeOfTree);

            string tableName = typeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM {0}主表 ORDER BY 权重 DESC", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                Button BtnTag = AddNode(reader[string.Format("{0}id", tableName)].ToString(), reader["名称"].ToString());
                WpCards.Children.Add(BtnTag);
            }
            reader.Close();

        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            Button BtnTag = (sender as MenuItem).DataContext as Button;
            Console.WriteLine(BtnTag.Uid);
            WpCards.Children.Remove(BtnTag);

            string tableName = TypeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            //回收站：string sql = string.Format("DELETE from {0}主表 where {0}id='{1}';", tableName, BtnTag.Uid);
            string sql = string.Format("update {0}主表 set IsDel=True where {0}id='{1}';", tableName, BtnTag.Uid);
            sqlConn.ExecuteNonQuery(sql);


            RefreshKeyWords();
            MarkNamesInChapter();
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
            Button button = new Button
            {
                Uid = guid,
                Content = content,
                Height = 25,
                Padding = new Thickness(2),
                Margin = new Thickness(5, 5, 2, 2),
            };
            button.Click += BtnTag_Click;

            ContextMenu aMenu = new ContextMenu
            {
                DataContext = button
            };

            MenuItem deleteMenu = new MenuItem
            {
                Header = "删除"
            };
            deleteMenu.Click += DeleteMenu_Click;
            aMenu.Items.Add(deleteMenu);
            button.ContextMenu = aMenu;

            string tableName = TypeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("SELECT * FROM {0}别称表 where {0}id='{1}';", tableName, guid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                button.ToolTip += reader["别称"].ToString() + " ";
            }
            reader.Close();
            return button;
        }
    }
}
