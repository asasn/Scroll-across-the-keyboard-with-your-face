using NSMain.Bricks;
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

namespace NSMain.Cards
{
    /// <summary>
    /// WCardEdit.xaml 的交互逻辑
    /// </summary>
    public partial class WCardEdit : Window
    {
        public WCardEdit(string curBookName, string typeOfTree)
        {
            InitializeComponent();

            if (curBookName == "index")
            {
                this.Left = GlobalVal.Uc.TreeHistory.TranslatePoint(new Point(), GlobalVal.Uc.MainWin).X;
                this.Top = 115;
            }
            else
            {
                this.Left = GlobalVal.Uc.Searcher.TranslatePoint(new Point(), GlobalVal.Uc.MainWin).X;
                this.Top = 115;
            }
            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;

        }



        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WCardEdit), new PropertyMetadata(null));



        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(WCardEdit), new PropertyMetadata(null));


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void URecord_Loaded(object sender, RoutedEventArgs e)
        {
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string tableName = TypeOfTree;
            string sql;

            sql = string.Format("SELECT * FROM {0}属性表;", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                if (reader["Text"].ToString() == "别称")
                {
                    continue;
                }
                UTip uTip = new UTip(MyURecord, reader["Text"].ToString());
                uTip.Uid = reader["Uid"].ToString();
            }
            reader.Close();

            //填充信息之后，将保存状态拨回，以实现初始化
            BtnSave.IsEnabled = false;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            LbShow.Visibility = Visibility.Hidden;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string tableName = TypeOfTree;
            string sql = string.Empty;
            foreach (UTip tipBox in MyURecord.WpMain.Children)
            {
                foreach (UTip tipBox2 in MyURecord.WpMain.Children)
                {
                    int i = MyURecord.WpMain.Children.IndexOf(tipBox);
                    int n = MyURecord.WpMain.Children.IndexOf(tipBox2);
                    if (i != n && tipBox.Text == tipBox2.Text && false == string.IsNullOrEmpty(tipBox.Text))
                    {
                        tipBox.Tag = false;
                        LbShow.Visibility = Visibility.Visible;
                        LbShow.Content = "存在重复项，不予保存";
                        break;
                    }
                }
                if (tipBox.Text == "别称")
                {
                    tipBox.Tag = false;
                    LbShow.Visibility = Visibility;
                    LbShow.Content = "存在重复项，不予保存";
                }
                if (false == string.IsNullOrEmpty(tipBox.Text) && LbShow.Visibility == Visibility.Visible)
                {
                    return;
                }
                if (string.IsNullOrEmpty(tipBox.Text))
                {
                    tipBox.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrEmpty(tipBox.Uid))
                {
                    if (false == string.IsNullOrEmpty(tipBox.Text))
                    {
                        //将外面带入的sql语句提交，并且清空
                        cSqlite.ExecuteNonQuery(sql);
                        sql = string.Empty;
                        string guid = tipBox.Uid = Guid.NewGuid().ToString();
                        sql = string.Format("insert or ignore into {0}属性表 (Uid, Text) values ('{1}', '{2}');", tableName, guid, tipBox.Text.Replace("'", "''"));
                        cSqlite.ExecuteNonQuery(sql);
                        sql = string.Empty; //注意清空，以免影响后续语句运行
                    }
                }
                else
                {
                    //存在记录，为空时删除，不为空时更新
                    if (string.IsNullOrEmpty(tipBox.Text))
                    {
                        sql += string.Format("delete from {0}属性表 where Uid='{1}';", tableName, tipBox.Uid);
                    }
                    else
                    {
                        if ((bool)tipBox.Tag == true)
                        {
                            sql += string.Format("update {0}属性表 set Text='{1}' where Uid='{2}';", tableName, tipBox.Text.Replace("'", "''"), tipBox.Uid);
                        }
                    }

                }
                tipBox.Tag = false;
            }
            cSqlite.ExecuteNonQuery(sql);
            BtnSave.IsEnabled = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
