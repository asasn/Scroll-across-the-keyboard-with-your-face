using JiebaNet.Analyser;
using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// CollectToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CollectToolWindow : Window
    {
        public CollectToolWindow()
        {
            InitializeComponent();
        }

        public TreeViewNode TopNode
        {
            get { return (TreeViewNode)GetValue(TopNodeProperty); }
            set { SetValue(TopNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopNodeProperty =
            DependencyProperty.Register("TopNode", typeof(TreeViewNode), typeof(CollectToolWindow), new PropertyMetadata(new TreeViewNode { Uid = "", IsDir = true }));




        public TreeViewNode CurNode
        {
            get { return (TreeViewNode)GetValue(CurNodeProperty); }
            set { SetValue(CurNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurNodeProperty =
            DependencyProperty.Register("CurNode", typeof(TreeViewNode), typeof(CollectToolWindow), new PropertyMetadata(null));


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurNode = Gval.Uc.TreeBook.CurNode;
            TopNode = Gval.Uc.TreeBook.TopNode;
        }

        private void BtnCollect_Click(object sender, RoutedEventArgs e)
        {
            if (CurNode == null)
            {
                return;
            }

            var idf = new TfidfExtractor();
            foreach (string item in idf.ExtractTags(CurNode.NodeContent, 10))
            {
                TbKeyWords.Text += item + " ";
            }
            List<string> names = new List<string>();
            if (CurNode.IsDir == true)
            {
                foreach (TreeViewNode node in CurNode.ChildNodes)
                {
                    GetNames(names, node);
                }
            }
            else
            {
                GetNames(names, CurNode);
            }

            names = names.OrderBy(s => s.Length).ToList();
            foreach (string name in names)
            {
                if (IsName(names, name))
                {
                    TbNames.Text += name + " ";
                }
            }
        }

        void GetNames(List<string> names, TreeViewNode node)
        {



            string text = node.NodeContent;
            text = Regex.Replace(text, "　", "", RegexOptions.Multiline);

            string[] pArray =
            {
                    "^(.{2,4})(?=(豪气..|淡淡说|淡淡|好笑|苦笑|惊怒|提醒|豪迈|豪气)道)",
                    "^(.{2,4})(?=(亦|也|说|笑|惊|怒)道)",
                    "^(.{2,4})(?=微微..)",
                    "^(.{2,4})(?=坐在上首)",
                    "^(.{2,4})(?=深以为.)",
                    "^(.{2,4})(?=面色)",
                };
            foreach (string p in pArray)
            {
                MatchCollection ms = Regex.Matches(text, p, RegexOptions.Multiline);
                if (ms.Count > 0)
                {
                    foreach (Match m in ms)
                    {
                        text = Regex.Replace(text, m.Value, "【【【【" + m.Value + "】】】】", RegexOptions.Multiline);
                    }
                }
            }
            MatchCollection ms2 = Regex.Matches(text, "(?<=【【【【)(.+?)(?=】】】】)", RegexOptions.Multiline);
            if (ms2.Count > 0)
            {
                foreach (Match m in ms2)
                {
                    if (false == names.Contains(m.Value))
                    {
                        names.Add(m.Value);
                    }
                }
            }
        }

        bool IsName(List<string> names, string value)
        {
            foreach (string name in names)
            {
                if (name != value && name.Contains(value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
