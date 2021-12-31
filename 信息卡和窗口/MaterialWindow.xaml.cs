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
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// MaterialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialWindow : Window
    {
        public MaterialWindow()
        {
            InitializeComponent();
        }

        private void CbVolume_Loaded(object sender, RoutedEventArgs e)
        {
            CbVolume.Items.Clear();
            LoadToComboBox(CbVolume);
            CbVolume.SelectedIndex = int.Parse(SettingsOperate.Get("VolumeIndex"));
        }

        private void CbChapter_Loaded(object sender, RoutedEventArgs e)
        {
            CbChapter.Items.Clear();
            LoadToComboBox(CbChapter, CbVolume);
            CbChapter.SelectedIndex = int.Parse(SettingsOperate.Get("ChapterIndex"));
        }

        private void CbVolume_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbChapter.Items.Clear();
            LoadToComboBox(CbChapter, CbVolume);
            SettingsOperate.Set("VolumeIndex", CbVolume.SelectedIndex.ToString());
        }

        private void CbChapter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbChapter.SelectedIndex == -1)
            {
                return;
            }
            UcEditor.DataContext = (CbChapter.SelectedItem as TextBlock).DataContext as TreeViewNode;
            UcEditor.LoadChapter("index", "material");
            SettingsOperate.Set("ChapterIndex", CbChapter.SelectedIndex.ToString());
        }


        void LoadToComboBox(ComboBox cb, ComboBox Pcb = null)
        {
            string pid = "";
            if (Pcb != null && Pcb.SelectedItem != null)
            {
                pid = (Pcb.SelectedItem as TextBlock).Uid;
            }
            string curBookName = "index";
            string tableName = "material";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, curBookName + ".db");
            string sql = string.Format("CREATE TABLE IF NOT EXISTS Tree_{0} (Uid CHAR PRIMARY KEY, Pid CHAR, NodeName CHAR, isDir BOOLEAN, NodeContent TEXT, WordsCount INTEGER, IsExpanded BOOLEAN, IsChecked BOOLEAN);", tableName);
            sqlConn.ExecuteNonQuery(sql);
            sql = string.Format("SELECT * FROM Tree_{0} where Pid='{1}';", tableName, pid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                TreeViewNode node = new TreeViewNode
                {
                    Uid = reader["Uid"].ToString(),
                    Pid = reader["Pid"].ToString(),
                    NodeName = reader["NodeName"].ToString(),
                    IsDir = (bool)reader["IsDir"],
                    NodeContent = reader["NodeContent"].ToString(),
                    WordsCount = Convert.ToInt32(reader["WordsCount"]),
                    IsExpanded = (bool)reader["IsExpanded"],
                    IsChecked = (bool)reader["IsChecked"],
                };
                TextBlock tbk = new TextBlock();
                tbk.Text = node.NodeName;
                tbk.Uid = node.Uid;
                tbk.DataContext = node;
                cb.Items.Add(tbk);
            }
            reader.Close();
            sqlConn.Close();
        }

    }
}