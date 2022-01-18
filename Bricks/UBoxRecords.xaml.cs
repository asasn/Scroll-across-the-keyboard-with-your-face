using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// UBoxRecords.xaml 的交互逻辑
    /// </summary>
    public partial class UBoxRecords : UserControl
    {
        public UBoxRecords()
        {
            InitializeComponent();
        }


        public string Pid
        {
            get { return (string)GetValue(PidProperty); }
            set { SetValue(PidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PidProperty =
            DependencyProperty.Register("Pid", typeof(string), typeof(UBoxRecords), new PropertyMetadata(null));

        public Button BtnSave
        {
            get { return (Button)GetValue(BtnSaveProperty); }
            set { SetValue(BtnSaveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BtnSave.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BtnSaveProperty =
            DependencyProperty.Register("BtnSave", typeof(Button), typeof(UBoxRecords), new PropertyMetadata(null));



        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(UBoxRecords), new PropertyMetadata(null));



        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(UBoxRecords), new PropertyMetadata(null));


        public void WpMain_Fill(object sender, RoutedEventArgs e)
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

            foreach (string t in wps)
            {
                URecord uRecord = new URecord();
                uRecord.Title = t;
                uRecord.Name = t;
                Binding boolBinding = new Binding
                {
                    Source = BtnSave,
                    Path = new PropertyPath("IsEnabled"),
                    Mode = BindingMode.TwoWay
                };
                uRecord.SetBinding(uRecord.IsCanSave, boolBinding);//对绑定目标的目标属性进行绑定   

                WpMain.Children.Add(uRecord);
            }

            CCards.FillMainInfo(CurBookName, TypeOfTree, this.WpMain.Children, Pid);

            //填充信息之后，将保存状态拨回，以实现初始化
            BtnSave.IsEnabled = false;
        }
    }
}
