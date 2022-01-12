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
using 脸滚键盘.自定义控件;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// DesignToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WindowNotesTool : Window
    {
        public WindowNotesTool(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
        }

        string CurBookName;
        string TypeOfTree;



        public UcontrolNotes CurCard
        {
            get { return (UcontrolNotes)GetValue(CurCardProperty); }
            set { SetValue(CurCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurCardProperty =
            DependencyProperty.Register("CurCard", typeof(UcontrolNotes), typeof(WindowNotesTool), new PropertyMetadata(null));




        public UcontrolNotes PreviousCard
        {
            get { return (UcontrolNotes)GetValue(PreviousCardProperty); }
            set { SetValue(PreviousCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCardProperty =
            DependencyProperty.Register("PreviousCard", typeof(UcontrolNotes), typeof(WindowNotesTool), new PropertyMetadata(null));



        private void BtnAddScene_Click(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }

            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            string sql;

            UcontrolNotes sCard = new UcontrolNotes();
            if (CurCard == null || WpNotes.Children.Count == 0)
            {
                //追加至末尾
                WpNotes.Children.Add(sCard);
            }
            else
            {
                //在指定位置插入
                WpNotes.Children.Insert(CurCard.Index + 1, sCard);


                int n = sCard.Index;
                //更新索引
                for (int i = n; i < WpNotes.Children.Count; i++)
                {
                    (WpNotes.Children[i] as UcontrolNotes).Index = WpNotes.Children.IndexOf(WpNotes.Children[i] as UcontrolNotes);
                    (WpNotes.Children[i] as UcontrolNotes).StrIndex = string.Format("编号：{0}", (WpNotes.Children[i] as UcontrolNotes).Index + 1);
                    sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i] as UcontrolNotes).Index, (WpNotes.Children[i] as UcontrolNotes).Uid);
                    sqlConn.ExecuteNonQuery(sql);
                }
            }
            sCard.Index = WpNotes.Children.IndexOf(sCard);
            sCard.StrIndex = string.Format("编号：{0}", WpNotes.Children.IndexOf(sCard) + 1);
            sCard.StrTitile = string.Format("{0}", TbTitle.Text);
            sCard.GotFocus += SCard_GotFocus;
            sCard.LostFocus += SCard_LostFocus;
            TbTitle.Clear();

            sCard.Uid = Guid.NewGuid().ToString();
            sql = string.Format("INSERT INTO  随手记录表 (Uid , 索引, 标题, 内容) VALUES ('{0}', '{1}', '{2}', '{3}');", sCard.Uid, sCard.Index, sCard.StrTitile.Replace("'", "''"), sCard.StrContent.Replace("'", "''"));
            sqlConn.ExecuteNonQuery(sql);
            sCard.Focus();
        }

        /// <summary>
        /// 鼠标滚轮控制滚动（增加了对左右滚动的处理）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WpNotes_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,

                Source = sender
            };
            if (e.Delta > 0)
            {
                ScMain.LineLeft();
                ScMain.LineLeft();
                ScMain.LineLeft();
            }
            else
            {
                ScMain.LineRight();
                ScMain.LineRight();
                ScMain.LineRight();
            }
            if (e.Delta > 0)
            {
                ScMain.LineUp();
                ScMain.LineUp();
                ScMain.LineUp();
            }
            else
            {
                ScMain.LineDown();
                ScMain.LineDown();
                ScMain.LineDown();
            }

            WpNotes.RaiseEvent(eventArg);
            e.Handled = true;
        }

        private void WpNotes_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }
            WpNotes.Children.Clear();
            string tableName = TypeOfTree;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            //尝试建立新表（IF NOT EXISTS）
            string sql = string.Format("CREATE TABLE IF NOT EXISTS 随手记录表 (Uid CHAR PRIMARY KEY, 索引 INTEGER, 标题 CHAR, 内容 CHAR);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 随手记录表Uid ON 随手记录表(Uid);");
            sqlConn.ExecuteNonQuery(sql);

            sql = string.Format("select * from 随手记录表 ORDER BY 索引;", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                UcontrolNotes sCard = new UcontrolNotes
                {
                    Uid = reader["Uid"].ToString(),
                    Index = Convert.ToInt32(reader["索引"]),
                    StrIndex = string.Format("编号：{0}", Convert.ToInt32(reader["索引"]) + 1),
                    StrTitile = reader["标题"].ToString(),
                    StrContent = reader["内容"].ToString()
                };
                WpNotes.Children.Add(sCard);
                sCard.GotFocus += SCard_GotFocus;
                sCard.LostFocus += SCard_LostFocus;
            }
            reader.Close();
            if (WpNotes.Children.Count > 0)
            {
                (WpNotes.Children[WpNotes.Children.Count - 1] as UcontrolNotes).Focus();
                ScMain.ScrollToHorizontalOffset((60 * WpNotes.Children.Count) - ScMain.ActualWidth / 2);
            }
        }

        private void SCard_LostFocus(object sender, RoutedEventArgs e)
        {
            PreviousCard = sender as UcontrolNotes;
        }

        private void SCard_GotFocus(object sender, RoutedEventArgs e)
        {
            CurCard = sender as UcontrolNotes;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            string sql = string.Format("SELECT * FROM 随手记录表 where Uid='{0}';", CurCard.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                CurCard.Uid = reader["Uid"].ToString();
                CurCard.Index = Convert.ToInt32(reader["索引"]);
                TbShowIndex.Text = CurCard.StrIndex = string.Format("编号：{0}", Convert.ToInt32(reader["索引"]) + 1);
                TbShowTitle.Text = CurCard.StrTitile = reader["标题"].ToString();
                TbShowContent.Text = CurCard.StrContent = reader["内容"].ToString();
            }
            reader.Close();

            CurCard.BorderBrush = Brushes.Orange;
            CurCard.BorderThickness = new Thickness(2, 2, 2, 2);
            if (PreviousCard != null)
            {
                PreviousCard.BorderBrush = null;
                PreviousCard.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            BtnSave.IsEnabled = false;
        }

        private void TbShowTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            BtnSave.IsEnabled = true;
        }

        private void TbShowContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            BtnSave.IsEnabled = true;
        }

        private void Command_DelCard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }

            string sql;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];

            int n = WpNotes.Children.IndexOf(CurCard);
            WpNotes.Children.Remove(CurCard);
            sql = string.Format("DELETE FROM 随手记录表 where Uid='{0}';", CurCard.Uid);
            sqlConn.ExecuteNonQuery(sql);
            if (WpNotes.Children.Count > 0)
            {
                if (n == WpNotes.Children.Count)
                {
                    (WpNotes.Children[n - 1] as UcontrolNotes).Focus();
                }
                else
                {
                    (WpNotes.Children[n] as UcontrolNotes).Focus();
                }
            }
            else
            {
                TbShowIndex.Clear();
                TbShowTitle.Clear();
                TbShowContent.Clear();
            }

            //更新索引
            for (int i = n; i < WpNotes.Children.Count; i++)
            {
                (WpNotes.Children[i] as UcontrolNotes).Index = WpNotes.Children.IndexOf(WpNotes.Children[i] as UcontrolNotes);
                (WpNotes.Children[i] as UcontrolNotes).StrIndex = string.Format("编号：{0}", (WpNotes.Children[i] as UcontrolNotes).Index + 1);
                sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i] as UcontrolNotes).Index, (WpNotes.Children[i] as UcontrolNotes).Uid);
                sqlConn.ExecuteNonQuery(sql);
            }
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
                    (WpNotes.Children[WpNotes.Children.IndexOf(CurCard) - 1] as UcontrolNotes).Focus();
                }
            }
            if (e.Key == Key.Right || e.Key == Key.Down)
            {
                if (WpNotes.Children.IndexOf(CurCard) < WpNotes.Children.Count - 1)
                {
                    (WpNotes.Children[WpNotes.Children.IndexOf(CurCard) + 1] as UcontrolNotes).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = WpNotes.Children.IndexOf(CurCard);
                if (i > 0)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpNotes.Children[i - 1] as UcontrolNotes).StrContent;
                    (WpNotes.Children[i - 1] as UcontrolNotes).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpNotes.Children[i - 1] as UcontrolNotes).StrTitile;
                    (WpNotes.Children[i - 1] as UcontrolNotes).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpNotes.Children[i - 1] as UcScenesCard).StrIndex;
                    //(WpNotes.Children[i - 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index--;
                    (WpNotes.Children[i - 1] as UcontrolNotes).Index++;

                    SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i - 1] as UcontrolNotes).Index, (WpNotes.Children[i - 1] as UcontrolNotes).Uid);
                    sqlConn.ExecuteNonQuery(sql);

                    (WpNotes.Children[i - 1] as UcontrolNotes).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = WpNotes.Children.IndexOf(CurCard);
                if (i < WpNotes.Children.Count - 1)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpNotes.Children[i + 1] as UcontrolNotes).StrContent;
                    (WpNotes.Children[i + 1] as UcontrolNotes).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpNotes.Children[i + 1] as UcontrolNotes).StrTitile;
                    (WpNotes.Children[i + 1] as UcontrolNotes).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpNotes.Children[i + 1] as UcScenesCard).StrIndex;
                    //(WpNotes.Children[i + 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index++;
                    (WpNotes.Children[i + 1] as UcontrolNotes).Index--;

                    SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i + 1] as UcontrolNotes).Index, (WpNotes.Children[i + 1] as UcontrolNotes).Uid);
                    sqlConn.ExecuteNonQuery(sql);

                    (WpNotes.Children[i + 1] as UcontrolNotes).Focus();
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            CurCard.StrTitile = TbShowTitle.Text;
            CurCard.StrContent = TbShowContent.Text;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            string sql = string.Format("UPDATE 随手记录表 set 标题='{0}', 内容='{1}' where Uid='{2}';", CurCard.StrTitile.Replace("'", "''"), CurCard.StrContent.Replace("'", "''"), CurCard.Uid);
            sqlConn.ExecuteNonQuery(sql);
            BtnSave.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void window_Closed(object sender, EventArgs e)
        {

        }

        private void ScMain_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Console.WriteLine(ScMain.HorizontalOffset);
        }

        private void TbTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAddScene.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void TbShowTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && BtnSave.IsEnabled == true)
            {
                BtnSave.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
