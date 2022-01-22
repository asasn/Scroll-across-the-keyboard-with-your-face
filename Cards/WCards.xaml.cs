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
    public partial class WCards : Window
    {
        public struct ThisCard
        {
            public static string id;
            public static string weight;
            public static string realAge;
            public static string 诞生年份;
        }

        public WCards(string curBookName, string typeOfTree, Button btnParent)
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

            CurBookName = curBookName;
            TypeOfTree = typeOfTree;

            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };

            //根据外来调用传入的参数填充变量，以备给类成员方法使用
            Pid = btnParent.Uid;
            TbName.Text = PName = btnParent.Content.ToString();
            BtnParent = btnParent;
            UBtnSave = this.BtnSave;
            MyRecords.BtnSave = BtnSave;
            MyRecords.Pid = Pid;
        }




        public string PName
        {
            get { return (string)GetValue(PNameProperty); }
            set { SetValue(PNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PNameProperty =
            DependencyProperty.Register("PName", typeof(string), typeof(WCards), new PropertyMetadata(null));




        public string Pid
        {
            get { return (string)GetValue(PidProperty); }
            set { SetValue(PidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PidProperty =
            DependencyProperty.Register("Pid", typeof(string), typeof(WCards), new PropertyMetadata(null));



        public Button BtnParent
        {
            get { return (Button)GetValue(BtnParentProperty); }
            set { SetValue(BtnParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BtnParent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BtnParentProperty =
            DependencyProperty.Register("BtnParent", typeof(Button), typeof(WCards), new PropertyMetadata(null));





        public Button UBtnSave
        {
            get { return (Button)GetValue(UBtnSaveProperty); }
            set { SetValue(UBtnSaveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UBtnSave.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UBtnSaveProperty =
            DependencyProperty.Register("UBtnSave", typeof(Button), typeof(WCards), new PropertyMetadata(null));




        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WCards), new PropertyMetadata(null));



        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(WCards), new PropertyMetadata(null));

        /// <summary>
        /// 显示信息卡的主流程：从数据库中获取信息以填充卡片
        /// </summary>
        void GetDataAndFillCard()
        {
            if (false == string.IsNullOrEmpty(this.Pid))
            {
                ArrayList wps = new ArrayList();
                if (TypeOfTree == "角色")
                {
                    wps.Add("别称");
                    wps.Add("身份");
                    wps.Add("外观");
                    wps.Add("阶级");
                    wps.Add("所属");
                    wps.Add("物品");
                    wps.Add("能力");
                    wps.Add("经历");
                }
                if (TypeOfTree == "其他")
                {
                    wps.Add("别称");
                    wps.Add("描述");
                    wps.Add("阶级");
                    wps.Add("基类");
                    wps.Add("派生");
                    wps.Add("物品");
                    wps.Add("能力");
                    wps.Add("经历");
                }
                if (TypeOfTree == "世界")
                {
                    wps.Add("别称");
                    wps.Add("描述");
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
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("select * from {0}主表 where {0}id = '{1}';", tableName, this.Pid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                if (reader["备注"].ToString() != "")
                {
                    Tb备注.Text = reader["备注"].ToString();
                }
                if (reader["权重"].ToString() == "")
                {
                    ThisCard.weight = "0";
                }
                else
                {
                    ThisCard.weight = reader["权重"].ToString();
                }
                if (reader["诞生年份"].ToString() == "")
                {
                    TbBornYear.Text = "0";
                }
                else
                {
                    TbBornYear.Text = reader["诞生年份"].ToString();
                }

            }
            reader.Close();


            int realAge = GlobalVal.CurrentBook.CurrentYear - int.Parse(TbBornYear.Text);
            card.Header = "　权重：" + ThisCard.weight.ToString();
            TbRealYear.Text = realAge.ToString();
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
            int.TryParse(tb.Text, out int str);
            tb.Text = str.ToString();

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
            BtnSave.IsEnabled = false;
            string tableName = TypeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            SQLiteDataReader reader = sqlConn.ExecuteQuery(string.Format("select * from {0}主表 where 名称='{1}'", tableName, TbName.Text.Replace("'", "''")));
            while (reader.Read())
            {
                if (this.Pid != reader.GetString(0).ToString())
                {
                    MessageBox.Show("数据库中已经存在同名不同id条目，请修改成为其他名称！");
                    reader.Close();

                    return;
                }
            }
            reader.Close();

            if (false == string.IsNullOrEmpty(this.PName))
            {
                if (string.IsNullOrEmpty(ThisCard.weight))
                {
                    ThisCard.weight = 0.ToString();
                }
                if (string.IsNullOrEmpty(TbBornYear.Text))
                {
                    TbBornYear.Text = 0.ToString();
                }

                string sql = string.Format("update {0}主表 set 名称='{1}', 备注='{2}', 权重={3}, 诞生年份={4} where {0}id = '{5}';", tableName, TbName.Text.Replace("'", "''"), Tb备注.Text.Replace("'", "''"), ThisCard.weight, TbBornYear.Text, this.Pid);
                sqlConn.ExecuteNonQuery(sql);

                //传递给父容器
                BtnParent.Content = PName = TbName.Text;

                CCards.SaveMainInfo(CurBookName, TypeOfTree, MyRecords.WpMain.Children, Pid);
            }

            GlobalVal.Uc.RoleCards.RefreshKeyWords();
            GlobalVal.Uc.RoleCards.MarkNamesInChapter();
        }

        private void MyRecords_Loaded(object sender, RoutedEventArgs e)
        {   
            //填充窗口信息
            GetDataAndFillCard();
        }

    }
}
