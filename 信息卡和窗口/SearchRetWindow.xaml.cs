using System;
using System.Collections.Generic;
using System.IO;
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
    /// SearchRetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SearchRetWindow : Window
    {
        public SearchRetWindow(TreeViewNode curNode, TreeViewNode topNode, string ucTag, string keyWords)
        {
            InitializeComponent();
            CurNode = curNode;
            TopNode = topNode;
            UcTag = ucTag;
            KeyWords = keyWords;

            Console.WriteLine(curNode.NodeName);

            GetResultList();
        }

        string KeyWords;
        string UcTag;
        TreeViewNode CurNode;
        TreeViewNode TopNode;

        //void RefreshBookItem()
        //{
        //    //如果单只依靠绑定属性来传值，可能会发生DataContext改变了（触发本事件）而依赖属性CurItem未改变的情况
        //    //所以，使用this.DataContext作为CurItem的值是必需的
        //    CurItem = this.DataContext as TreeViewItem;

        //    curRootItem = TreeOperate.GetRootItem(CurItem);
        //    WorkPath = TreeOperate.GetItemPath(curRootItem, UcTag);
        //    if (curRootItem != null)
        //    {
        //        curTv = curRootItem.Parent as TreeView;
        //    }
        //    else
        //    {
        //        curTv = null;
        //    }
        //    curVolumeItem = TreeOperate.GetItemByLevel(CurItem, 1);
        //    FullFileName = TreeOperate.GetItemPath(CurItem, UcTag);

        //}

        private void ucSearchRetWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }


        private void GetResultList()
        {


        }

        //从ListBoxItem当中获得真正的章节名
        private string GetFileName(string itemValue)
        {
            if (!string.IsNullOrEmpty(itemValue))
            {
                Regex regex = new Regex(@".txt");
                return regex.Split(itemValue)[0] + ".txt";
            }
            else
            {
                return null;
            }
        }

        //判断文件当中是否包含某个字符串
        private bool IsInFile(string FilePath, string mystr)
        {
            string text = System.IO.File.ReadAllText(@FilePath);
            if (text.Contains(mystr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //选择改变事件
        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textEditor.Text = String.Empty;
            ListBoxItem lbItem = lb.SelectedItem as ListBoxItem;
            if (false == string.IsNullOrEmpty(lb.SelectedItem.ToString()))
            {
                textEditor.Text += lbItem.Uid + "\n";
                string[] sArray = KeyWords.Split(' ');
                foreach (var mystr in sArray)
                {
                    string lines = GetStrOnLines(lbItem.Uid, mystr);
                    textEditor.Text += lines + "\n";
                }
            }
        }

        //从当前行当中判断字符串是否存在
        private string GetStrOnLines(string FilePath, string mystr)
        {
            int counter = 0;
            string line;
            string lines = string.Empty;
            StreamReader file = new StreamReader(@FilePath);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(mystr))
                {
                    lines += line + "\n";
                    counter++;
                }
            }
            file.Close();
            Console.WriteLine(line);
            return lines;
        }



        //鼠标双击
        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbItem = lb.SelectedItem as ListBoxItem;
            if (!string.IsNullOrEmpty(lb.SelectedItem.ToString()))
            {
                //默认直接打开
                //System.Diagnostics.Process.Start(strFullFileName);
                //指定打开方式
                System.Diagnostics.Process.Start(@"D:/mysoftware/EmEditor/EmEditor.exe", lbItem.Uid);
            }

        }


    }
}
