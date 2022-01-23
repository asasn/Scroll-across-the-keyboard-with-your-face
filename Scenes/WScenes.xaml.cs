using NSMain.Bricks;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NSMain.Scenes
{
    /// <summary>
    /// DesignToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WScenes : Window
    {
        public WScenes(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
        }

        string CurBookName;
        string TypeOfTree;



        public UScenes CurCard
        {
            get { return (UScenes)GetValue(CurCardProperty); }
            set { SetValue(CurCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurCardProperty =
            DependencyProperty.Register("CurCard", typeof(UScenes), typeof(WScenes), new PropertyMetadata(null));




        public UScenes PreviousCard
        {
            get { return (UScenes)GetValue(PreviousCardProperty); }
            set { SetValue(PreviousCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCardProperty =
            DependencyProperty.Register("PreviousCard", typeof(UScenes), typeof(WScenes), new PropertyMetadata(null));



        /// <summary>
        /// 更新索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <param name="Wp"></param>
        int UpdateIndex(int n, string tableName, CSqlitePlus sqlConn, WrapPanel Wp)
        {
            int numOfDel = 0;
            for (int i = n; i < Wp.Children.Count; i++)
            {
                (Wp.Children[i] as UScenes).Index = Wp.Children.IndexOf(Wp.Children[i] as UScenes);
                (Wp.Children[i] as UScenes).StrIndex = string.Format("第{0}幕", (Wp.Children[i] as UScenes).Index + 1);
            }
            string sql = string.Format("SELECT COUNT(IsDel) FROM {0} where IsDel=True;", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
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

            UScenes sCard = new UScenes();
            if (CurCard == null || WpScenes.Children.Count == 0)
            {
                //追加至末尾
                WpScenes.Children.Add(sCard);
            }
            else
            {
                //在指定位置插入
                WpScenes.Children.Insert(CurCard.Index + 1, sCard);
            }

            sCard.Index = WpScenes.Children.IndexOf(sCard);
            sCard.StrIndex = string.Format("第{0}幕", sCard.Index + 1);
            sCard.StrTitile = string.Format("{0}", TbTitle.Text);
            sCard.GotFocus += SCard_GotFocus;
            sCard.LostFocus += SCard_LostFocus;
            TbTitle.Clear();

            //sCard.Index + NumOfDel这里给新纪录索引补上了被删除的数量
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            int numOfDel = UpdateIndex(sCard.Index, "场记大纲表", sqlConn, WpScenes);
            sCard.Uid = Guid.NewGuid().ToString();
            string sql = string.Format("INSERT INTO  场记大纲表 (Uid , 索引, 标题, 内容) VALUES ('{0}', '{1}', '{2}', '{3}');", sCard.Uid, sCard.Index + numOfDel, sCard.StrTitile.Replace("'", "''"), sCard.StrContent.Replace("'", "''"));
            sqlConn.ExecuteNonQuery(sql);
            sCard.Focus();
        }

        /// <summary>
        /// 鼠标滚轮控制滚动（增加了对左右滚动的处理）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WpScenes_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

            WpScenes.RaiseEvent(eventArg);
            e.Handled = true;
        }



        private void WpScenes_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurBookName == null)
            {
                return;
            }
            WpScenes.Children.Clear();

            string tableName = TypeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("select * from 场记大纲表 ORDER BY 索引;", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                if ((bool)reader["IsDel"] == true)
                {
                    continue;
                }
                UScenes sCard = new UScenes
                {
                    Uid = reader["Uid"].ToString(),
                    StrTitile = reader["标题"].ToString(),
                    StrContent = reader["内容"].ToString()
                };
                WpScenes.Children.Add(sCard);
                sCard.Index = WpScenes.Children.IndexOf(sCard);
                sCard.StrIndex = string.Format("第{0}幕", sCard.Index + 1);
                sCard.GotFocus += SCard_GotFocus;
                sCard.LostFocus += SCard_LostFocus;
            }
            reader.Close();
            int n = Convert.ToInt32(MySettings.Get(CurBookName, "WpScenesFocusIndex"));
            if (WpScenes.Children.Count > 0)
            {
                (WpScenes.Children[n] as UScenes).Focus();
                ScMain.ScrollToHorizontalOffset((60 * WpScenes.Children.Count) - ScMain.ActualWidth / 2);
            }

        }

        private void SCard_LostFocus(object sender, RoutedEventArgs e)
        {
            PreviousCard = sender as UScenes;
        }

        private void SCard_GotFocus(object sender, RoutedEventArgs e)
        {
            CurCard = sender as UScenes;
            TbShowIndex.Text = CurCard.StrIndex;
            TbShowTitle.Text = CurCard.StrTitile;
            TbShowContent.Text = CurCard.StrContent;
            CurCard.BorderBrush = Brushes.Orange;
            CurCard.BorderThickness = new Thickness(2, 2, 2, 2);
            if (PreviousCard != null)
            {
                PreviousCard.BorderBrush = null;
                PreviousCard.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            BtnSave.IsEnabled = false;
            MySettings.Set(CurBookName, "WpScenesFocusIndex", CurCard.Index.ToString());
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

            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];

            int n = WpScenes.Children.IndexOf(CurCard);
            WpScenes.Children.Remove(CurCard);
            //回收站：string sql = string.Format("DELETE FROM 场记大纲表 where Uid='{0}';", CurCard.Uid);
            string sql = string.Format("update 场记大纲表 set IsDel=True where Uid='{0}';", CurCard.Uid);
            sqlConn.ExecuteNonQuery(sql);
            if (WpScenes.Children.Count > 0)
            {
                if (n == WpScenes.Children.Count)
                {
                    (WpScenes.Children[n - 1] as UScenes).Focus();
                }
                else
                {
                    (WpScenes.Children[n] as UScenes).Focus();
                }
            }
            else
            {
                TbShowIndex.Clear();
                TbShowTitle.Clear();
                TbShowContent.Clear();
            }

            _ = UpdateIndex(n, "场记大纲表", sqlConn, WpScenes);
        }

        private void WpScenes_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            if (e.Key == Key.Left || e.Key == Key.Up)
            {
                if (WpScenes.Children.IndexOf(CurCard) > 0)
                {
                    (WpScenes.Children[WpScenes.Children.IndexOf(CurCard) - 1] as UScenes).Focus();
                }
            }
            if (e.Key == Key.Right || e.Key == Key.Down)
            {
                if (WpScenes.Children.IndexOf(CurCard) < WpScenes.Children.Count - 1)
                {
                    (WpScenes.Children[WpScenes.Children.IndexOf(CurCard) + 1] as UScenes).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = WpScenes.Children.IndexOf(CurCard);
                if (i > 0)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpScenes.Children[i - 1] as UScenes).StrContent;
                    (WpScenes.Children[i - 1] as UScenes).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpScenes.Children[i - 1] as UScenes).StrTitile;
                    (WpScenes.Children[i - 1] as UScenes).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpScenes.Children[i - 1] as UcScenesCard).StrIndex;
                    //(WpScenes.Children[i - 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index--;
                    (WpScenes.Children[i - 1] as UScenes).Index++;

                    CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 场记大纲表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 场记大纲表 set 索引='{0}' where Uid='{1}';", (WpScenes.Children[i - 1] as UScenes).Index, (WpScenes.Children[i - 1] as UScenes).Uid);
                    sqlConn.ExecuteNonQuery(sql);

                    (WpScenes.Children[i - 1] as UScenes).Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = WpScenes.Children.IndexOf(CurCard);
                if (i < WpScenes.Children.Count - 1)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpScenes.Children[i + 1] as UScenes).StrContent;
                    (WpScenes.Children[i + 1] as UScenes).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpScenes.Children[i + 1] as UScenes).StrTitile;
                    (WpScenes.Children[i + 1] as UScenes).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpScenes.Children[i + 1] as UcScenesCard).StrIndex;
                    //(WpScenes.Children[i + 1] as UcScenesCard).StrIndex = temp3;

                    CurCard.Index++;
                    (WpScenes.Children[i + 1] as UScenes).Index--;

                    CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
                    string sql = string.Format("UPDATE 场记大纲表 set 索引='{0}' where Uid='{1}';", CurCard.Index, CurCard.Uid);
                    sql += string.Format("UPDATE 场记大纲表 set 索引='{0}' where Uid='{1}';", (WpScenes.Children[i + 1] as UScenes).Index, (WpScenes.Children[i + 1] as UScenes).Uid);
                    sqlConn.ExecuteNonQuery(sql);

                    (WpScenes.Children[i + 1] as UScenes).Focus();
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
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("UPDATE 场记大纲表 set 标题='{0}', 内容='{1}' where Uid='{2}';", CurCard.StrTitile.Replace("'", "''"), CurCard.StrContent.Replace("'", "''"), CurCard.Uid);
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
