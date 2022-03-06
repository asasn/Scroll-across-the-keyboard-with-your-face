using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace NSMain.Bricks
{
    /// <summary>
    /// UDelList.xaml 的交互逻辑
    /// </summary>
    public partial class UDelList : UserControl
    {
        public UDelList()
        {
            InitializeComponent();
        }



        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(UDelList), new PropertyMetadata(null));



        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(UDelList), new PropertyMetadata(null));



        public ObservableCollection<Member> memberData = new ObservableCollection<Member>();
        public class Member
        {
            public string Uid { get; set; }
            public string Name { get; set; }
            public string Owner { get; set; }
            public string CurBookName { get; set; }
            public string TableName { get; set; }
        }

        public void LoadNoteList(string curBookName, string tableName)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM {0} where IsDel=True;", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                memberData.Add(new Member()
                {
                    Uid = reader["Uid"].ToString(),
                    Name = reader["标题"].ToString(),
                    Owner = string.Format("{0}", tableName),
                    CurBookName = curBookName,
                    TableName = tableName,
                });
            }
            reader.Close();
        }

        public void LoadTreeList(string curBookName, string tableName)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM Tree_{0} where IsDel=True;", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                memberData.Add(new Member()
                {
                    Uid = reader["Uid"].ToString(),
                    Name = reader["NodeName"].ToString(),
                    Owner = string.Format("{0}", tableName),
                    CurBookName = curBookName,
                    TableName = tableName,
                });
            }
            reader.Close();
        }

        public void LoadCardList(string curBookName, string tableName)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM {0}主表 where IsDel=True;", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                memberData.Add(new Member()
                {
                    Uid = reader["Uid"].ToString(),
                    Name = reader["名称"].ToString(),
                    Owner = string.Format("{0}", tableName),
                    CurBookName = curBookName,
                    TableName = tableName,
                });
            }
            reader.Close();
        }

        private void DG_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }
            bool designTime = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
            if (designTime)
                return;//设计模式下返回，不再执行代码
            memberData.Clear();
            if (CurBookName == "index")
            {
                LoadNoteList("index", "随手记录表");
                LoadNoteList("index", "题材主表");
                LoadTreeList("index", "allbooks");
                LoadTreeList("index", "material");
                LoadCardList("index", "角色");
                LoadCardList("index", "其他");
                LoadCardList("index", "世界");
            }
            else
            {
                LoadNoteList(GlobalVal.CurrentBook.Name, "场记大纲表");
                LoadTreeList(GlobalVal.CurrentBook.Name, "book");
                LoadTreeList(GlobalVal.CurrentBook.Name, "history");
                LoadTreeList(GlobalVal.CurrentBook.Name, "task");
                LoadCardList(GlobalVal.CurrentBook.Name, "角色");
                LoadCardList(GlobalVal.CurrentBook.Name, "其他");
                LoadCardList(GlobalVal.CurrentBook.Name, "世界");
            }
            DG.DataContext = memberData;
        }


        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }
            ArrayList arrayList = new ArrayList();

            //从集合中倒序删除，防止索引号错误引起的跳过
            for (int i = DG.SelectedItems.Count - 1; i >= 0; i--)
            {
                Member item = DG.SelectedItems[i] as Member;
                CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[item.CurBookName];
                if (false == arrayList.Contains(item.TableName))
                {
                    arrayList.Add(item.TableName);
                }
                string sql = string.Empty;
                if (item.TableName == "场记大纲表" || item.TableName == "随手记录表" || item.TableName == "题材主表")
                {
                    sql = string.Format("update {0} set IsDel=False where Uid='{1}';", item.TableName, item.Uid);
                }
                if (item.TableName == "角色" || item.TableName == "其他" || item.TableName == "世界")
                {
                    sql = string.Format("update {0}主表 set IsDel=False where Uid='{1}';", item.TableName, item.Uid);
                }
                if (item.TableName == "book" || item.TableName == "history" || item.TableName == "task" || item.TableName == "allbooks" || item.TableName == "material")
                {
                    sql = string.Format("update Tree_{0} set IsDel=False where Uid='{1}';", item.TableName, item.Uid);
                }
                memberData.Remove(item);
                cSqlite.ExecuteNonQuery(sql);
            }

            foreach (string tableName in arrayList)
            {
                if (CurBookName == "index")
                {
                    if (tableName == "material")
                    {
                        GlobalVal.Uc.TreeMaterial.LoadBook("index", "material");
                    }
                    if (tableName == "角色")
                    {
                        GlobalVal.Uc.PublicRoleCards.LoadCards("index", tableName);
                    }
                    if (tableName == "其他")
                    {
                        GlobalVal.Uc.PublicOtherCards.LoadCards("index", tableName);
                    }
                    if (tableName == "世界")
                    {
                        GlobalVal.Uc.PublicWorldCards.LoadCards("index", tableName);
                    }
                }
                else
                {
                    if (tableName == "book")
                    {
                        GlobalVal.Uc.TreeBook.LoadBook(GlobalVal.CurrentBook.Name, tableName);
                    }
                    if (tableName == "history")
                    {
                        GlobalVal.Uc.TreeHistory.LoadBook(GlobalVal.CurrentBook.Name, tableName);
                    }
                    if (tableName == "task")
                    {
                        GlobalVal.Uc.TreeTask.LoadBook(GlobalVal.CurrentBook.Name, tableName);
                    }
                    if (tableName == "角色")
                    {
                        GlobalVal.Uc.RoleCards.LoadCards(GlobalVal.CurrentBook.Name, tableName);
                    }
                    if (tableName == "其他")
                    {
                        GlobalVal.Uc.OtherCards.LoadCards(GlobalVal.CurrentBook.Name, tableName);
                    }
                    if (tableName == "世界")
                    {
                        GlobalVal.Uc.WorldCards.LoadCards(GlobalVal.CurrentBook.Name, tableName);
                    }
                }
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }
            //从集合中倒序删除，防止索引号错误引起的跳过
            for (int i = DG.SelectedItems.Count - 1; i >= 0; i--)
            {
                Member item = DG.SelectedItems[i] as Member;
                CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[item.CurBookName];
                string sql = string.Empty;
                if (item.TableName == "场记大纲表" || item.TableName == "随手记录表" || item.TableName == "题材主表")
                {
                    sql = string.Format("UPDATE {0} SET 索引=索引-1 where 索引 > (SELECT 索引 FROM {0} where Uid='{1}');", item.TableName, item.Uid);
                    sql += string.Format("DELETE FROM {0} where Uid='{1}';", item.TableName, item.Uid);
                }
                if (item.TableName == "角色" || item.TableName == "其他" || item.TableName == "世界")
                {
                    sql = string.Format("DELETE FROM {0}主表 where Uid='{1}';", item.TableName, item.Uid);
                }
                if (item.TableName == "book" || item.TableName == "history" || item.TableName == "task" || item.TableName == "allbooks" || item.TableName == "material")
                {
                    sql = string.Format("DELETE FROM Tree_{0} where Uid='{1}';", item.TableName, item.Uid);
                }
                cSqlite.ExecuteNonQuery(sql);
                memberData.Remove(item);
                if (item.TableName == "allbooks")
                {
                    CFileOperate.DeleteFile(GlobalVal.Path.Books + "/" + item.Name + ".db");
                }
            }
        }

        private void DG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DG.SelectedItem = null;
        }



        private void BtnZip_Click(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }
            GlobalVal.SQLClass.Pools[CurBookName].Vacuum();
            LbSize_Loaded(null, null);
        }


        private void LbSize_Loaded(object sender, RoutedEventArgs e)
        {
            System.IO.FileInfo fileInfo = null;
            try
            {
                fileInfo = new System.IO.FileInfo(GlobalVal.Path.Books + "\\" + CurBookName + ".db");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (fileInfo != null && fileInfo.Exists)
            {
                decimal fileSize = Math.Round(Convert.ToDecimal(fileInfo.Length / 1024.0 / 1024.0), 2, MidpointRounding.AwayFromZero);
                LbSize.Content = string.Format("数据库大小：{0}mb", fileSize);
            }
        }
    }
}
