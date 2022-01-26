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

namespace NSMain.Bricks
{
    /// <summary>
    /// UNotes.xaml 的交互逻辑
    /// </summary>
    public partial class UNotes : WrapPanel
    {
        public UNotes()
        {
            InitializeComponent();
        }

        public int Toward
        {
            get { return (int)GetValue(TowardProperty); }
            set { SetValue(TowardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Toward.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TowardProperty =
            DependencyProperty.Register("Toward", typeof(int), typeof(UNotes), new PropertyMetadata(0));




        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(UNotes), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(UNotes), new PropertyMetadata(null));



        public UNote CurCard
        {
            get { return (UNote)GetValue(CurCardProperty); }
            set { SetValue(CurCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurCardProperty =
            DependencyProperty.Register("CurCard", typeof(UNote), typeof(UNotes), new PropertyMetadata(null));



        public UNote PreviousCard
        {
            get { return (UNote)GetValue(PreviousCardProperty); }
            set { SetValue(PreviousCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCardProperty =
            DependencyProperty.Register("PreviousCard", typeof(UNote), typeof(UNotes), new PropertyMetadata(null));




        public bool IsCanSave
        {
            get { return (bool)GetValue(IsCanSaveProperty); }
            set { SetValue(IsCanSaveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCanSave.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCanSaveProperty =
            DependencyProperty.Register("IsCanSave", typeof(bool), typeof(UNotes), new PropertyMetadata(false));





        /// <summary>
        /// 更新索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <param name="Wp"></param>
        void UpdateIndex(int n, string tableName, CSqlitePlus cSqlite, WrapPanel Wp)
        {
            for (int i = n; i < Wp.Children.Count; i++)
            {
                (Wp.Children[i] as UNote).Index = Wp.Children.IndexOf(Wp.Children[i] as UNote);
                (Wp.Children[i] as UNote).StrIndex = string.Format("编号：{0}", (Wp.Children[i] as UNote).Index + 1);
            }
        }

        private void Command_DelCard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }

            string sql = string.Empty;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];

            int n = WpNotes.Children.IndexOf(CurCard);
            WpNotes.Children.Remove(CurCard);
            string tableName = string.Empty;
            if (CurBookName == "index")
            {
                tableName = "随手记录表";
            }
            else
            {
                tableName = "场记大纲表";
            }
            sql = string.Format("update {0} set IsDel=True where Uid='{1}';", tableName, CurCard.Uid);
            cSqlite.ExecuteNonQuery(sql);
            if (WpNotes.Children.Count > 0)
            {
                if (n == WpNotes.Children.Count)
                {
                    (WpNotes.Children[n - 1] as UNote).Focus();
                }
                else
                {
                    (WpNotes.Children[n] as UNote).Focus();
                }
            }
            else
            {
                CurCard = null;
            }

            UpdateIndex(n, tableName, cSqlite, WpNotes);

        }

        private void WpNotes_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }

            WpNotes.Children.Clear();
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Empty;
            string tableName = string.Empty;
            if (CurBookName == "index")
            {
                tableName = "随手记录表";
            }
            else
            {
                tableName = "场记大纲表";
            }
            sql = string.Format("select * from {0} ORDER BY 索引;", tableName);

            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                UNote sCard = new UNote
                {
                    Toward = this.Toward,
                    Uid = reader["Uid"].ToString(),
                    StrTitle = reader["标题"].ToString(),
                    StrContent = reader["内容"].ToString()
                };
                WpNotes.Children.Add(sCard);
                sCard.Index = WpNotes.Children.IndexOf(sCard);
                sCard.StrIndex = string.Format("编号：{0}", sCard.Index + 1);
                sCard.GotFocus += SCard_GotFocus;
                sCard.LostFocus += SCard_LostFocus;

            }
            reader.Close();
            int n = Convert.ToInt32(MySettings.Get(CurBookName, "WpNotesFocusIndex"));
            if (WpNotes.Children.Count > 0)
            {
                (WpNotes.Children[n] as UNote).Focus();
            }
        }

        private void SCard_LostFocus(object sender, RoutedEventArgs e)
        {
            PreviousCard = sender as UNote;
        }

        private void SCard_GotFocus(object sender, RoutedEventArgs e)
        {
            CurCard = sender as UNote;
            if (PreviousCard != null)
            {
                PreviousCard.BorderBrush = null;
                PreviousCard.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            CurCard.BorderBrush = Brushes.Orange;
            CurCard.BorderThickness = new Thickness(2, 2, 2, 2);
            CurCard.Index = WpNotes.Children.IndexOf(CurCard);
            MySettings.Set(CurBookName, "WpNotesFocusIndex", CurCard.Index.ToString());
            this.IsCanSave = false;
        }

        private void WpNotes_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            if (e.Key == Key.Left || e.Key == Key.Up)
            {
                if (WpNotes.Children.IndexOf(CurCard) > 0)
                {
                    (WpNotes.Children[WpNotes.Children.IndexOf(CurCard) - 1] as UNote).Focus();
                }
            }
            if (e.Key == Key.Right || e.Key == Key.Down)
            {
                if (WpNotes.Children.IndexOf(CurCard) < WpNotes.Children.Count - 1)
                {
                    (WpNotes.Children[WpNotes.Children.IndexOf(CurCard) + 1] as UNote).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = WpNotes.Children.IndexOf(CurCard);
                if (i > 0)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpNotes.Children[i - 1] as UNote).StrContent;
                    (WpNotes.Children[i - 1] as UNote).StrContent = temp;
                    string temp2 = CurCard.StrTitle;
                    CurCard.StrTitle = (WpNotes.Children[i - 1] as UNote).StrTitle;
                    (WpNotes.Children[i - 1] as UNote).StrTitle = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpNotes.Children[i - 1] as UcScenesCard).StrIndex;
                    //(WpNotes.Children[i - 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index--;
                    (WpNotes.Children[i - 1] as UNote).Index++;

                    CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i - 1] as UNote).Index, (WpNotes.Children[i - 1] as UNote).Uid);
                    cSqlite.ExecuteNonQuery(sql);

                    (WpNotes.Children[i - 1] as UNote).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = WpNotes.Children.IndexOf(CurCard);
                if (i < WpNotes.Children.Count - 1)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpNotes.Children[i + 1] as UNote).StrContent;
                    (WpNotes.Children[i + 1] as UNote).StrContent = temp;
                    string temp2 = CurCard.StrTitle;
                    CurCard.StrTitle = (WpNotes.Children[i + 1] as UNote).StrTitle;
                    (WpNotes.Children[i + 1] as UNote).StrTitle = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpNotes.Children[i + 1] as UcScenesCard).StrIndex;
                    //(WpNotes.Children[i + 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index++;
                    (WpNotes.Children[i + 1] as UNote).Index--;

                    CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i + 1] as UNote).Index, (WpNotes.Children[i + 1] as UNote).Uid);
                    cSqlite.ExecuteNonQuery(sql);

                    (WpNotes.Children[i + 1] as UNote).Focus();
                }
            }
        }


        public void AddCard(string title)
        {
            UNote sCard = new UNote();
            sCard.Toward = this.Toward;
            if (CurCard == null || WpNotes.Children.Count == 0)
            {
                //追加至末尾
                WpNotes.Children.Add(sCard);
            }
            else
            {
                //在指定位置插入
                WpNotes.Children.Insert(CurCard.Index + 1, sCard);
            }

            sCard.Index = WpNotes.Children.IndexOf(sCard);
            sCard.StrIndex = string.Format("编号：{0}", sCard.Index + 1);
            sCard.StrTitle = string.Format("{0}", title);
            sCard.GotFocus += SCard_GotFocus;
            sCard.LostFocus += SCard_LostFocus;

            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            sCard.Uid = Guid.NewGuid().ToString();

            string sql = string.Empty;
            string tableName = string.Empty;
            if (CurBookName == "index")
            {
                tableName = "随手记录表";
            }
            else
            {
                tableName = "场记大纲表";
            }
            UpdateIndex(sCard.Index, tableName, cSqlite, WpNotes);
            sql = string.Format("UPDATE {0} SET 索引=索引+1 where 索引 > (SELECT 索引 FROM {0} where Uid = '{1}');", tableName, CurCard.Uid);
            sql += string.Format("INSERT INTO {0} (Uid , 索引, 标题, 内容) VALUES ('{1}', (SELECT 索引 FROM {0} where Uid = '{2}') + 1, '{3}', '{4}');", tableName, sCard.Uid, CurCard.Uid, sCard.StrTitle.Replace("'", "''"), sCard.StrContent.Replace("'", "''"));
            cSqlite.ExecuteNonQuery(sql);
            sCard.Focus();
        }
    }
}
