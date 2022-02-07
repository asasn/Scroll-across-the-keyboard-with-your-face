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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NSMain.Cards
{
    /// <summary>
    /// UCard.xaml 的交互逻辑
    /// </summary>
    public partial class UCard : Button
    {
        public UCard(string guid, string title, string typeOfTree, string curBookName, UCards uCards)
        {
            InitializeComponent();
            this.Uid = guid;
            this.Content = title;
            ParentUc = uCards;
            TypeOfTree = typeOfTree;
            CurBookName = curBookName;
            string tableName = typeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("SELECT * FROM {0}从表 where Pid='{1}' and Tid=(select Uid from {0}属性表 where Text='别称');", tableName, guid);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                this.ToolTip += reader["Text"].ToString() + " ";
            }
            reader.Close();
        }



        public UCards ParentUc
        {
            get { return (UCards)GetValue(ParentUcProperty); }
            set { SetValue(ParentUcProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentUc.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentUcProperty =
            DependencyProperty.Register("ParentUc", typeof(UCards), typeof(UCard), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(UCard), new PropertyMetadata(null));




        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(UCard), new PropertyMetadata(null));




        private void Command_DelCard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            ParentUc.WpCards.Children.Remove(this);

            string tableName = TypeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("update {0}主表 set IsDel=True where Uid='{1}';", tableName, this.Uid);
            cSqlite.ExecuteNonQuery(sql);


            ParentUc.RefreshKeyWords();
            ParentUc.MarkNamesInChapter();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WCard wCards = new WCard(CurBookName, TypeOfTree, this);
            wCards.ShowDialog();
        }
    }
}
