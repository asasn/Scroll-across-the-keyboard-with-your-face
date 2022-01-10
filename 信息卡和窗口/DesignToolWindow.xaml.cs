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
    public partial class DesignToolWindow : Window
    {
        public DesignToolWindow(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
        }

        string CurBookName;
        string TypeOfTree;



        public UcScenesCard CurCard
        {
            get { return (UcScenesCard)GetValue(CurCardProperty); }
            set { SetValue(CurCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurCardProperty =
            DependencyProperty.Register("CurCard", typeof(UcScenesCard), typeof(DesignToolWindow), new PropertyMetadata(null));




        public UcScenesCard PreviousCard
        {
            get { return (UcScenesCard)GetValue(PreviousCardProperty); }
            set { SetValue(PreviousCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCardProperty =
            DependencyProperty.Register("PreviousCard", typeof(UcScenesCard), typeof(DesignToolWindow), new PropertyMetadata(null));



        private void BtnAddScene_Click(object sender, RoutedEventArgs e)
        {
            UcScenesCard sCard = new UcScenesCard();
            WpScenes.Children.Add(sCard);
            sCard.StrIndex = string.Format("第{0}幕", WpScenes.Children.IndexOf(sCard) + 1);
            sCard.StrTitile = string.Format("{0}", TbTitle.Text);
            sCard.GotFocus += SCard_GotFocus;
            sCard.LostFocus += SCard_LostFocus;
            TbTitle.Clear();
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
            string tableName = TypeOfTree;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            //尝试建立新表（IF NOT EXISTS）
            string sql = string.Format("CREATE TABLE IF NOT EXISTS 场记大纲表 (Uid CHAR PRIMARY KEY, 索引 INTEGER, 标题 CHAR, 内容 CHAR);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 场记大纲表Uid ON 场记大纲表(Uid);");
            sqlConn.ExecuteNonQuery(sql);

            sql = string.Format("select * from 场记大纲表 ORDER BY 索引;", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            WpScenes.Children.Clear();
            while (reader.Read())
            {
                UcScenesCard sCard = new UcScenesCard
                {
                    Uid = reader["Uid"].ToString(),
                    Index = Convert.ToInt32(reader["索引"]),
                    StrIndex = string.Format("第{0}幕", Convert.ToInt32(reader["索引"]) + 1),
                    StrTitile = reader["标题"].ToString(),
                    StrContent = reader["内容"].ToString()
                };
                WpScenes.Children.Add(sCard);
                sCard.GotFocus += SCard_GotFocus;
                sCard.LostFocus += SCard_LostFocus;
            }
            reader.Close();
        }


        private void SCard_LostFocus(object sender, RoutedEventArgs e)
        {
            PreviousCard = sender as UcScenesCard;
        }

        private void SCard_GotFocus(object sender, RoutedEventArgs e)
        {
            CurCard = sender as UcScenesCard;
            TbShowIndex.Text = string.Format("第{0}幕", WpScenes.Children.IndexOf(CurCard) + 1);
            TbShowTitle.Text = CurCard.StrTitile;
            TbShowContent.Text = CurCard.StrContent;

            CurCard.BorderBrush = Brushes.Orange;
            CurCard.BorderThickness = new Thickness(2, 2, 2, 2);
            if (PreviousCard != null)
            {
                PreviousCard.BorderBrush = null;
                PreviousCard.BorderThickness = new Thickness(0, 0, 0, 0);
            }
        }

        private void TbShowTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            CurCard.StrTitile = TbShowTitle.Text;

        }

        private void TbShowContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            CurCard.StrContent = TbShowContent.Text;
        }

        private void DelCard_Click(object sender, RoutedEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            for (int i = WpScenes.Children.IndexOf(CurCard) + 1; i < WpScenes.Children.Count; i++)
            {
                (WpScenes.Children[i] as UcScenesCard).StrIndex = string.Format("第{0}幕", WpScenes.Children.IndexOf(WpScenes.Children[i] as UcScenesCard));
            }
            WpScenes.Children.Remove(CurCard);
        }

        private void WpScenes_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurCard == null)
            {
                return;
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = WpScenes.Children.IndexOf(CurCard);
                if (i > 0)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpScenes.Children[i - 1] as UcScenesCard).StrContent;
                    (WpScenes.Children[i - 1] as UcScenesCard).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpScenes.Children[i - 1] as UcScenesCard).StrTitile;
                    (WpScenes.Children[i - 1] as UcScenesCard).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpScenes.Children[i - 1] as UcScenesCard).StrIndex;
                    //(WpScenes.Children[i - 1] as UcScenesCard).StrIndex = temp3;
                    (WpScenes.Children[i - 1] as UcScenesCard).TbIndex.Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = WpScenes.Children.IndexOf(CurCard);
                if (i < WpScenes.Children.Count - 1)
                {
                    string temp = CurCard.StrContent;
                    CurCard.StrContent = (WpScenes.Children[i + 1] as UcScenesCard).StrContent;
                    (WpScenes.Children[i + 1] as UcScenesCard).StrContent = temp;
                    string temp2 = CurCard.StrTitile;
                    CurCard.StrTitile = (WpScenes.Children[i + 1] as UcScenesCard).StrTitile;
                    (WpScenes.Children[i + 1] as UcScenesCard).StrTitile = temp2;
                    //string temp3 = CurCard.StrIndex;
                    //CurCard.StrIndex = (WpScenes.Children[i + 1] as UcScenesCard).StrIndex;
                    //(WpScenes.Children[i + 1] as UcScenesCard).StrIndex = temp3;
                    (WpScenes.Children[i + 1] as UcScenesCard).TbIndex.Focus();
                }
            }
        }
    }
}
