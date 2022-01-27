using NSMain.Bricks;
using NSMain.Cards;
using System;
using System.Collections;
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

namespace NSMain.Tools
{
    /// <summary>
    /// WPuzzles.xaml 的交互逻辑
    /// </summary>
    public partial class WNewProjects : Window
    {

        public WNewProjects(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            GlobalVal.Uc.NewProjects = this;
        }

        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WNewProjects), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(WNewProjects), new PropertyMetadata(null));




        private void BtnAddScene_Click(object sender, RoutedEventArgs e)
        {
            WpMain.AddCard(TbTitle.Text);
            TbTitle.Clear();
        }



        private void TbShowTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WpMain.CurCard == null)
            {
                return;
            }
            BtnSave.IsEnabled = true;
        }

        private void TbShowContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WpMain.CurCard == null)
            {
                return;
            }
            BtnSave.IsEnabled = true;
        }

        void SaveThis(UIElementCollection wrapPanels, string pid)
        {
            string tableName = "题材";
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            int w = 0;
            string sql = string.Empty;
            foreach (URecord uRecord in wrapPanels)
            {
                WrapPanel wp = uRecord.WpMain;
                sql = string.Empty;
                foreach (UTip tipBox in wp.Children)
                {
                    if (string.IsNullOrEmpty(tipBox.Uid))
                    {
                        if (false == string.IsNullOrEmpty(tipBox.Text))
                        {
                            //将外面带入的sql语句提交，并且清空
                            cSqlite.ExecuteNonQuery(sql);
                            sql = string.Empty;

                            //编辑框不为空，插入，这里的sql语句使用单条语句，以便获取最后填入的id
                            string guid = tipBox.Uid = Guid.NewGuid().ToString();
                            sql = string.Format("insert or ignore into {0}从表 (Uid, Pid, Tid, Text) values ('{1}', '{2}', '{3}', '{4}');", tableName, guid, pid, uRecord.Uid, tipBox.Text.Replace("'", "''"));
                            cSqlite.ExecuteNonQuery(sql);
                            sql = string.Empty; //注意清空，以免影响后续语句运行                            
                        }
                    }
                    else
                    {
                        //存在记录，为空时删除，不为空时更新
                        if (string.IsNullOrEmpty(tipBox.Text))
                        {
                            sql += string.Format("delete from {0}从表 where Uid='{1}';", tableName, tipBox.Uid);
                            w--;
                        }
                        else
                        {
                            if ((bool)tipBox.Tag == true)
                            {
                                sql += string.Format("update {0}从表 set Text='{1}' where Uid='{2}' AND Pid='{3}' AND Tid='{4}';", tableName, tipBox.Text.Replace("'", "''"), tipBox.Uid, pid, uRecord.Uid);
                            }
                        }
                    }
                    w++;
                    tipBox.Tag = false;
                }
                cSqlite.ExecuteNonQuery(sql);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (WpMain.CurCard == null)
            {
                return;
            }
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("UPDATE 题材主表 set 标题='{0}', 内容='{1}' where Uid='{2}';", TbShowTitle.Text.Replace("'", "''"), TbShowContent.Text.Replace("'", "''"), WpMain.CurCard.Uid);
            cSqlite.ExecuteNonQuery(sql);
            SaveThis(R1.WpMain.Children, WpMain.CurCard.Uid);
            SaveThis(R2.WpMain.Children, WpMain.CurCard.Uid);
            BtnSave.IsEnabled = false;
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

        void AddToHeaderList(ArrayList wps, string header)
        {
            wps.Add(new CCards.属性条目()
            {
                Uid = header,
                Text = header,
            });
        }

        private void R1_Loaded(object sender, RoutedEventArgs e)
        {
            R1.WpMain.Children.Clear();
            R1.BtnSave = BtnSave;
            ArrayList wps = new ArrayList();
            AddToHeaderList(wps, "主题");
            AddToHeaderList(wps, "风格");
            AddToHeaderList(wps, "卖点");
            AddToHeaderList(wps, "角色");
            AddToHeaderList(wps, "体系");
            AddToHeaderList(wps, "金手指");
            R1.WpMain_Build(wps);
            CCards.FillMainInfo(CurBookName, "题材", R1.WpMain.Children, WpMain.CurCard.Uid);
            BtnSave.IsEnabled = false;
        }

        private void R2_Loaded(object sender, RoutedEventArgs e)
        {
            R2.WpMain.Children.Clear();
            R2.BtnSave = BtnSave;
            ArrayList wps = new ArrayList();
            AddToHeaderList(wps, "大纲");
            R2.WpMain_Build(wps);
            CCards.FillMainInfo(CurBookName, "题材", R2.WpMain.Children, WpMain.CurCard.Uid);
            BtnSave.IsEnabled = false;
        }

        private void WpMain_GotFocus(object sender, RoutedEventArgs e)
        {
            R1_Loaded(null, null);
            R2_Loaded(null, null);
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
                    e.Cancel = true;
                }
            }
        }
    }
}
