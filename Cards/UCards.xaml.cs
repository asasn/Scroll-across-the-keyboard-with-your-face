using NSMain.Bricks;
using NSMain.Editor;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static NSMain.TreeViewPlus.CNodeModule;

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

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TypeOfTree == null)
            {
                return;
            }
            //查找是否已经存在相关的条目
            foreach (Button card in WpCards.Children)
            {
                bool IsContains = false;
                if (card.Content.ToString() == TbTab.Text)
                {
                    IsContains = true;
                }
                if (card.ToolTip != null && string.IsNullOrWhiteSpace(TbTab.Text) == false)
                {
                    string[] sArray = card.ToolTip.ToString().Split(new char[] { ' ' });
                    foreach (string ss in sArray)
                    {
                        if (ss == TbTab.Text)
                        {
                            IsContains = true;
                            break;
                        }
                    }
                }

                if (IsContains == true)
                {
                    card.BorderBrush = Brushes.Gold;
                }
                else
                {
                    card.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TypeOfTree == null || string.IsNullOrEmpty(TbTab.Text))
            {
                return;
            }
            string tableName = TypeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            SQLiteDataReader reader = cSqlite.ExecuteQuery(string.Format("SELECT 名称 FROM (SELECT 名称 FROM {0}主表 UNION SELECT Text FROM {0}从表 where Tid=(select Uid from {0}属性表 where Text='别称'))  where 名称='{1}' ORDER BY LENGTH(名称) DESC;", tableName, TbTab.Text.Replace("'", "''")));
            while (reader.Read())
            {
                MessageBox.Show("数据库中已经存在同名条目，请修改成为其他名称！");
                reader.Close();
                return;
            }
            reader.Close();

            string guid = Guid.NewGuid().ToString();
            Button BtnTag = AddNode(guid, TbTab.Text);
            WpCards.Children.Add(BtnTag);

            TbTab.Clear();

            string sql = string.Format("insert or ignore into {0}主表 (Uid, 名称) values ('{1}', '{2}');", tableName, BtnTag.Uid, BtnTag.Content.ToString().Replace("'", "''"));
            cSqlite.ExecuteNonQuery(sql);


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
            WrapPanel[] wps = { GlobalVal.Uc.RoleCards.WpCards, GlobalVal.Uc.OtherCards.WpCards, GlobalVal.Uc.WorldCards.WpCards };
            foreach (WrapPanel wp in wps)
            {
                foreach (Button button in wp.Children)
                {
                    if (curNode.NodeContent.Contains(button.Content.ToString()) || IsContainsNickname(button.ToolTip, curNode.NodeContent))
                    {
                        button.Background = Brushes.LightGoldenrodYellow;
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
            WCard wCards = new WCard(CurBookName, TypeOfTree, BtnTag);
            wCards.ShowDialog();
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


            string tableName = typeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM {0}主表 ORDER BY 权重 DESC", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                Button BtnTag = AddNode(reader["Uid"].ToString(), reader["名称"].ToString());
                WpCards.Children.Add(BtnTag);
            }
            reader.Close();

            RefreshKeyWords();
            MarkNamesInChapter();
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            Button BtnTag = (sender as MenuItem).DataContext as Button;
            Console.WriteLine(BtnTag.Uid);
            WpCards.Children.Remove(BtnTag);

            string tableName = TypeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("update {0}主表 set IsDel=True where Uid='{1}';", tableName, BtnTag.Uid);
            cSqlite.ExecuteNonQuery(sql);


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
                BtnSearch_Click(null, null);
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
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("SELECT * FROM {0}从表 where Pid='{1}' and Tid=(select Uid from {0}属性表 where Text='别称');", tableName, guid);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                button.ToolTip += reader["Text"].ToString() + " ";
            }
            reader.Close();
            return button;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            WCardEdit we = new WCardEdit(CurBookName, TypeOfTree);
            we.ShowDialog();
        }
    }
}
