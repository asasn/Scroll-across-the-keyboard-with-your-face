using NSMain.Bricks;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NSMain.Notes
{
    /// <summary>
    /// DesignToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WNotes : Window
    {
        public WNotes(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            GlobalVal.Uc.Notes = this;
        }

        string CurBookName;
        string TypeOfTree;



        public UNoteHorizontal CurCard
        {
            get { return (UNoteHorizontal)GetValue(CurCardProperty); }
            set { SetValue(CurCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurCardProperty =
            DependencyProperty.Register("CurCard", typeof(UNoteHorizontal), typeof(WNotes), new PropertyMetadata(null));




        public UNoteHorizontal PreviousCard
        {
            get { return (UNoteHorizontal)GetValue(PreviousCardProperty); }
            set { SetValue(PreviousCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCardProperty =
            DependencyProperty.Register("PreviousCard", typeof(UNoteHorizontal), typeof(WNotes), new PropertyMetadata(null));



        /// <summary>
        /// 更新索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <param name="Wp"></param>
        int UpdateIndex(int n, string tableName, CSqlitePlus cSqlite, WrapPanel Wp)
        {
            int numOfDel = 0;
            for (int i = n; i < Wp.Children.Count; i++)
            {
                (Wp.Children[i] as UNoteHorizontal).Index = Wp.Children.IndexOf(Wp.Children[i] as UNoteHorizontal);
                (Wp.Children[i] as UNoteHorizontal).StrIndex = string.Format("编号：{0}", (Wp.Children[i] as UNoteHorizontal).Index + 1);
            }
            string sql = string.Format("SELECT COUNT(IsDel) FROM {0} where IsDel=True;", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                numOfDel = Convert.ToInt32(reader["COUNT(IsDel)"]);
            }
            reader.Close();
            return numOfDel;
        }


        private void BtnAddScene_Click(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }



            UNoteHorizontal sCard = new UNoteHorizontal();
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
            sCard.StrTitile = string.Format("{0}", TbTitle.Text);
            sCard.GotFocus += SCard_GotFocus;
            sCard.LostFocus += SCard_LostFocus;
            TbTitle.Clear();

            //sCard.Index + NumOfDel这里给新纪录索引补上了被删除的数量
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            int numOfDel = UpdateIndex(sCard.Index, "随手记录表", cSqlite, WpNotes);
            sCard.Uid = Guid.NewGuid().ToString();
            string sql = string.Format("INSERT INTO  随手记录表 (Uid , 索引, 标题, 内容) VALUES ('{0}', '{1}', '{2}', '{3}');", sCard.Uid, sCard.Index + numOfDel, sCard.StrTitile.Replace("'", "''"), sCard.StrContent.Replace("'", "''"));
            cSqlite.ExecuteNonQuery(sql);
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
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("select * from 随手记录表 ORDER BY 索引;", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                UNoteHorizontal sCard = new UNoteHorizontal
                {
                    Uid = reader["Uid"].ToString(),
                    StrTitile = reader["标题"].ToString(),
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
                (WpNotes.Children[n] as UNoteHorizontal).Focus();
                ScMain.ScrollToHorizontalOffset((60 * WpNotes.Children.Count) - ScMain.ActualWidth / 2);
            }

        }

        private void SCard_LostFocus(object sender, RoutedEventArgs e)
        {
            PreviousCard = sender as UNoteHorizontal;
        }

        private void SCard_GotFocus(object sender, RoutedEventArgs e)
        {
            CurCard = sender as UNoteHorizontal;
            CurCard.Index = WpNotes.Children.IndexOf(CurCard);
            TbShowIndex.Text = CurCard.StrIndex;
            TbShowTitle.Text = CurCard.StrTitile;
            TbShowContent.Text = CurCard.StrContent;
            if (PreviousCard != null)
            {
                PreviousCard.BorderBrush = null;
                PreviousCard.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            CurCard.BorderBrush = Brushes.Orange;
            CurCard.BorderThickness = new Thickness(2, 2, 2, 2);
            BtnSave.IsEnabled = false;
            CurCard.Index = WpNotes.Children.IndexOf(CurCard);
            MySettings.Set(CurBookName, "WpNotesFocusIndex", CurCard.Index.ToString());
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
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];

            int n = WpNotes.Children.IndexOf(CurCard);
            WpNotes.Children.Remove(CurCard);
            //回收站sql = string.Format("DELETE FROM 随手记录表 where Uid='{0}';", CurCard.Uid);
            sql = string.Format("update 随手记录表 set IsDel=True where Uid='{0}';", CurCard.Uid);
            cSqlite.ExecuteNonQuery(sql);
            if (WpNotes.Children.Count > 0)
            {
                if (n == WpNotes.Children.Count)
                {
                    (WpNotes.Children[n - 1] as UNoteHorizontal).Focus();
                }
                else
                {
                    (WpNotes.Children[n] as UNoteHorizontal).Focus();
                }
            }
            else
            {
                TbShowIndex.Clear();
                TbShowTitle.Clear();
                TbShowContent.Clear();
            }

            _ = UpdateIndex(n, "随手记录表", cSqlite, WpNotes);
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
                    (WpNotes.Children[WpNotes.Children.IndexOf(CurCard) - 1] as UNoteHorizontal).Focus();
                }
            }
            if (e.Key == Key.Right || e.Key == Key.Down)
            {
                if (WpNotes.Children.IndexOf(CurCard) < WpNotes.Children.Count - 1)
                {
                    (WpNotes.Children[WpNotes.Children.IndexOf(CurCard) + 1] as UNoteHorizontal).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = WpNotes.Children.IndexOf(CurCard);
                if (i > 0)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpNotes.Children[i - 1] as UNoteHorizontal).StrContent;
                    (WpNotes.Children[i - 1] as UNoteHorizontal).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpNotes.Children[i - 1] as UNoteHorizontal).StrTitile;
                    (WpNotes.Children[i - 1] as UNoteHorizontal).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpNotes.Children[i - 1] as UcScenesCard).StrIndex;
                    //(WpNotes.Children[i - 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index--;
                    (WpNotes.Children[i - 1] as UNoteHorizontal).Index++;

                    CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i - 1] as UNoteHorizontal).Index, (WpNotes.Children[i - 1] as UNoteHorizontal).Uid);
                    cSqlite.ExecuteNonQuery(sql);

                    (WpNotes.Children[i - 1] as UNoteHorizontal).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = WpNotes.Children.IndexOf(CurCard);
                if (i < WpNotes.Children.Count - 1)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpNotes.Children[i + 1] as UNoteHorizontal).StrContent;
                    (WpNotes.Children[i + 1] as UNoteHorizontal).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpNotes.Children[i + 1] as UNoteHorizontal).StrTitile;
                    (WpNotes.Children[i + 1] as UNoteHorizontal).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpNotes.Children[i + 1] as UcScenesCard).StrIndex;
                    //(WpNotes.Children[i + 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index++;
                    (WpNotes.Children[i + 1] as UNoteHorizontal).Index--;

                    CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 随手记录表 set 索引='{0}' where Uid='{1}';", (WpNotes.Children[i + 1] as UNoteHorizontal).Index, (WpNotes.Children[i + 1] as UNoteHorizontal).Uid);
                    cSqlite.ExecuteNonQuery(sql);

                    (WpNotes.Children[i + 1] as UNoteHorizontal).Focus();
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
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("UPDATE 随手记录表 set 标题='{0}', 内容='{1}' where Uid='{2}';", CurCard.StrTitile.Replace("'", "''"), CurCard.StrContent.Replace("'", "''"), CurCard.Uid);
            cSqlite.ExecuteNonQuery(sql);
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
