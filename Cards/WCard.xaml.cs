using NSMain.Bricks;
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NSMain.Cards
{
    /// <summary>
    /// WCards.xaml 的交互逻辑
    /// </summary>
    public partial class WCard : Window
    {
        public struct ThisCard
        {
            public static string Uid;
            public static string Weight;
            public static string RealAge;
            public static string BornYear;
        }

        public WCard(string curBookName, string typeOfTree, UCard uCard)
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


            //根据外来调用传入的参数填充变量，以备给类成员方法使用
            Pid = uCard.Uid;
            TbName.Text = PName = uCard.Content.ToString();
            BtnParent = uCard;
            UBtnSave = this.BtnSave;
            MyRecords.BtnSave = BtnSave;
            MyRecords.Pid = Pid;

            uCard.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
        }




        public string PName
        {
            get { return (string)GetValue(PNameProperty); }
            set { SetValue(PNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PNameProperty =
            DependencyProperty.Register("PName", typeof(string), typeof(WCard), new PropertyMetadata(null));




        public string Pid
        {
            get { return (string)GetValue(PidProperty); }
            set { SetValue(PidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PidProperty =
            DependencyProperty.Register("Pid", typeof(string), typeof(WCard), new PropertyMetadata(null));



        public UCard BtnParent
        {
            get { return (UCard)GetValue(BtnParentProperty); }
            set { SetValue(BtnParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BtnParent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BtnParentProperty =
            DependencyProperty.Register("BtnParent", typeof(UCard), typeof(WCard), new PropertyMetadata(null));





        public Button UBtnSave
        {
            get { return (Button)GetValue(UBtnSaveProperty); }
            set { SetValue(UBtnSaveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UBtnSave.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UBtnSaveProperty =
            DependencyProperty.Register("UBtnSave", typeof(Button), typeof(WCard), new PropertyMetadata(null));




        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WCard), new PropertyMetadata(null));



        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(WCard), new PropertyMetadata(null));





        /// <summary>
        /// 显示信息卡的主流程：从数据库中获取信息以填充卡片
        /// </summary>
        void GetDataAndFillCard()
        {
            if (false == string.IsNullOrEmpty(this.Pid))
            {
                ArrayList wps = new ArrayList();

                string tableName = TypeOfTree;
                CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                string sql = string.Format("select * from {0}属性表;", tableName);
                SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
                while (reader.Read())
                {
                    if (reader["Text"].ToString() == "别称")
                    {
                        continue;
                    }
                    wps.Add(new CCards.属性条目()
                    {
                        Uid = reader["Uid"].ToString(),
                        Text = reader["Text"].ToString(),
                    });
                }
                reader.Close();


                if (TypeOfTree == "世界")
                {
                    R1.MinHeight = 0;
                }

                MyRecords.WpMain_Build(wps);

                FillBaseInfo();
                CCards.FillMainInfo(CurBookName, TypeOfTree, MyRecords.WpMain.Children, Pid);

                //填充信息之后，将保存状态拨回，以实现初始化
                BtnSave.IsEnabled = false;
            }
        }

        void FillBaseInfo()
        {
            string tableName = TypeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("select * from {0}主表 where Uid='{1}';", tableName, this.Pid);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                if (reader["备注"].ToString() != "")
                {
                    Tb备注.Text = reader["备注"].ToString();
                }
                if (string.IsNullOrEmpty(reader["权重"].ToString()) == true)
                {
                    ThisCard.Weight = "0";
                }
                else
                {
                    ThisCard.Weight = reader["权重"].ToString();
                }
                if (string.IsNullOrEmpty(reader["诞生年份"].ToString()) == true)
                {
                    TbBornYear.Text = null;
                }
                else
                {
                    TbBornYear.Text = reader["诞生年份"].ToString();
                }

            }
            reader.Close();

            card.Header = " 权重：" + ThisCard.Weight.ToString();
            if (string.IsNullOrEmpty(TbBornYear.Text) == false)
            {
                long realAge = GlobalVal.CurrentBook.CurrentYear - long.Parse(TbBornYear.Text);
                TbRealYear.Text = realAge.ToString();
            }


            if (string.IsNullOrEmpty(TbBornYear.Text) == true)
            {
                GridYear.Visibility = Visibility.Collapsed;
                R2.MinHeight = 0;
            }
        }

        private void Nickname_Loaded(object sender, RoutedEventArgs e)
        {
            string tableName = TypeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            cSqlite = GlobalVal.SQLClass.Pools[CurBookName];

            WrapPanel wp = Nickname.WpMain;
            string sql = string.Format("select * from {0}属性表 where Text='别称';", tableName);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                Nickname.Uid = reader["Uid"].ToString();
            }
            sql = string.Format("select * from {0}从表 where Pid='{1}' AND Tid='{2}';", tableName, this.Pid, Nickname.Uid);
            reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                UTip tipBox = new UTip(Nickname, reader["Text"].ToString());
                tipBox.Uid = reader["Uid"].ToString();
            }
            reader.Close();
            if (Nickname.WpMain.Children.Count == 0)
            {
                Nickname.Visibility = Visibility.Collapsed;
                R3.MinHeight = 0;
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = true;

        }

        private void TbBornYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            bool hasValue = long.TryParse(tb.Text, out long str);
            if (hasValue == true)
            {
                tb.Text = str.ToString();
            }
            else
            {
                tb.Text = null;
            }


            BtnSave.IsEnabled = true;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (BtnSave.IsEnabled == true)
            {
                MessageBoxResult dr = MessageBox.Show("有数据尚未保存\n要在退出前保存更改吗？", "Tip", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (dr == MessageBoxResult.Yes)
                {
                    BtnSave_Click(null, null);
                }
                if (dr == MessageBoxResult.No)
                {

                }
                if (dr == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            string tableName = TypeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            SQLiteDataReader reader = cSqlite.ExecuteQuery(string.Format("select * from {0}主表 where 名称='{1}'", tableName, TbName.Text.Replace("'", "''")));
            while (reader.Read())
            {
                if (this.Pid != reader["Uid"].ToString())
                {
                    MessageBox.Show("数据库中已经存在同名不同id条目，请修改成为其他名称！");
                    reader.Close();

                    return;
                }
            }
            reader.Close();

            if (false == string.IsNullOrEmpty(this.PName))
            {
                string sql = string.Empty;
                if (string.IsNullOrEmpty(ThisCard.Weight))
                {
                    ThisCard.Weight = 0.ToString();
                }
                if (string.IsNullOrEmpty(TbBornYear.Text) == true)
                {
                    ThisCard.BornYear = "null";
                    TbRealYear.Text = null;
                }
                else
                {
                    ThisCard.BornYear = TbBornYear.Text;
                }

                sql = string.Format("update {0}主表 set 名称='{1}', 备注='{2}', 权重='{3}', 诞生年份={4} where Uid='{5}';", tableName, TbName.Text.Replace("'", "''"), Tb备注.Text.Replace("'", "''"), ThisCard.Weight, ThisCard.BornYear, this.Pid);
                cSqlite.ExecuteNonQuery(sql);

                //传递给父容器
                BtnParent.Content = PName = TbName.Text;
                CCards.SaveNickName(CurBookName, TypeOfTree, Nickname, Pid);
                int w = CCards.SaveMainInfo(CurBookName, TypeOfTree, MyRecords.WpMain.Children, Pid);
                CCards.ReNewWeight(CurBookName, TypeOfTree, w, Pid);


                //先清空ToolTip的内容
                BtnParent.ToolTip = null;
                cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                sql = string.Format("SELECT Text FROM {0}从表 where Pid='{1}' AND Tid=(select Uid from {0}属性表 where Text='别称');", tableName, BtnParent.Uid);
                reader = cSqlite.ExecuteQuery(sql);
                while (reader.Read())
                {
                    BtnParent.ToolTip += reader["Text"].ToString() + " ";
                }
                reader.Close();


                sql = string.Format("SELECT * FROM {0}主表 where Uid='{1}';", tableName, this.Pid);
                reader = cSqlite.ExecuteQuery(sql);
                while (reader.Read())
                {
                    ThisCard.Weight = reader["权重"].ToString();
                }
                reader.Close();
                card.Header = " 权重：" + ThisCard.Weight.ToString();
                if (string.IsNullOrEmpty(TbBornYear.Text) == false)
                {
                    long realAge = GlobalVal.CurrentBook.CurrentYear - long.Parse(TbBornYear.Text);
                    TbRealYear.Text = realAge.ToString();
                }
            }

            GlobalVal.Uc.RoleCards.RefreshKeyWords();
            GlobalVal.Uc.RoleCards.MarkNamesInChapter();
            BtnSave.IsEnabled = false;
        }

        private void MyRecords_Loaded(object sender, RoutedEventArgs e)
        {
            //填充窗口信息
            GetDataAndFillCard();

            if (MyRecords.WpMain.Children.Count == 0)
            {
                MyRecords.Visibility = Visibility.Collapsed;
            }
            bool hasShow = false;
            foreach (URecord uRecord in MyRecords.WpMain.Children)
            {
                if (uRecord.WpMain.Children.Count == 0)
                {
                    uRecord.Visibility = Visibility.Collapsed;
                }
                else
                {
                    hasShow = true;
                }
            }
            if (hasShow == false)
            {
                GridMain.Visibility = Visibility.Collapsed;
                R4.MinHeight = 0;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Common.Scroll.ScrollIt(sender, e);
        }

        bool IsShow = false;
        private void BtnSee_Click(object sender, RoutedEventArgs e)
        {
            if (IsShow == true)
            {
                if (string.IsNullOrEmpty(TbBornYear.Text) == true)
                {
                    GridYear.Visibility = Visibility.Collapsed;
                    R2.MinHeight = 0;
                }
                if (Nickname.WpMain.Children.Count == 0)
                {
                    Nickname.Visibility = Visibility.Collapsed;
                    R2.MinHeight = 0;
                }
                bool hasShow = false;
                foreach (URecord uRecord in MyRecords.WpMain.Children)
                {
                    if (uRecord.WpMain.Children.Count == 0)
                    {
                        uRecord.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        hasShow = true;
                    }
                }
                if (hasShow == false)
                {
                    GridMain.Visibility = Visibility.Collapsed;
                    R4.MinHeight = 0;
                }
                IsShow = false;
                ExpandPath.RenderTransform = new RotateTransform(-90);
            }
            else
            {
                if (string.IsNullOrEmpty(TbBornYear.Text) == true)
                {
                    GridYear.Visibility = Visibility.Visible;
                    R2.MinHeight = 0;
                }
                if (Nickname.WpMain.Children.Count == 0)
                {
                    Nickname.Visibility = Visibility.Visible;
                }
                foreach (URecord uRecord in MyRecords.WpMain.Children)
                {
                    if (uRecord.WpMain.Children.Count == 0)
                    {
                        uRecord.Visibility = Visibility.Visible;
                    }
                }
                GridMain.Visibility = Visibility;
                IsShow = true;
                ExpandPath.RenderTransform = new RotateTransform();
            }
        }
    }
}
