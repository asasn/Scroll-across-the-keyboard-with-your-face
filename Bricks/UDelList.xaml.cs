using System;
using System.Collections;
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
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM {0} where IsDel=True;", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                memberData.Add(new Member()
                {
                    Uid = reader["Uid"].ToString(),
                    Name = reader["标题"].ToString(),
                    Owner = string.Format("{0}/{1}", curBookName, tableName),
                    CurBookName = curBookName,
                    TableName = tableName,
                });
            }
            reader.Close();
        }

        public void LoadNodeList(string curBookName, string tableName)
        {
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM {0} where IsDel=True;", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                memberData.Add(new Member()
                {
                    Uid = reader["Uid"].ToString(),
                    Name = reader["NodeName"].ToString(),
                    Owner = string.Format("{0}/{1}", curBookName, tableName),
                    CurBookName = curBookName,
                    TableName = tableName,
                });
            }
            reader.Close();
        }


        public void Udl1_Loaded()
        {
            memberData.Clear();
            LoadNoteList("index", "随手记录表");
            LoadNodeList("index", "Tree_material");
            DG.DataContext = memberData;
        }

        public void Udl2_Loaded()
        {
            memberData.Clear();
            LoadNoteList(GlobalVal.CurrentBook.Name, "场记大纲表");
            LoadNodeList(GlobalVal.CurrentBook.Name, "Tree_book");
            LoadNodeList(GlobalVal.CurrentBook.Name, "Tree_history");
            LoadNodeList(GlobalVal.CurrentBook.Name, "Tree_task");
            DG.DataContext = memberData;
        }


        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            //从集合中倒序删除，防止索引号错误引起的跳过
            for (int i = DG.SelectedItems.Count - 1; i >= 0; i--)
            {
                Member item = DG.SelectedItems[i] as Member;
                CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[item.CurBookName];
                string sql = string.Format("update {0} set IsDel=False where Uid='{1}';", item.TableName, item.Uid);
                sqlConn.ExecuteNonQuery(sql);
                memberData.Remove(item);
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //从集合中倒序删除，防止索引号错误引起的跳过
            for (int i = DG.SelectedItems.Count - 1; i >= 0; i--)
            {
                Member item = DG.SelectedItems[i] as Member;
                CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[item.CurBookName];
                string sql = string.Format("DELETE FROM {0} where Uid='{1}';", item.TableName, item.Uid);
                sqlConn.ExecuteNonQuery(sql);
                memberData.Remove(item);
            }
        }

        private void DG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DG.SelectedItem = null;
        }
    }
}
