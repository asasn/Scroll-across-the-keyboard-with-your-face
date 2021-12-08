using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        public SearchRetWindow(TreeViewNode baseNode, string ucTag, string keyWords)
        {
            InitializeComponent();
            BaseNode = baseNode;
            UcTag = ucTag;
            KeyWords = keyWords;

            GetResultList(baseNode);
        }

        string KeyWords;
        string UcTag;
        TreeViewNode BaseNode;

        void AddToListBox(TreeViewNode node)
        {
            string[] sArray = KeyWords.Split(' ');
            string ListItemName = String.Empty;
            ListItemName += ' ';
            foreach (var mystr in sArray)
            {
                if (false == string.IsNullOrEmpty(mystr))
                {
                    if (IsInFile(node, mystr))
                    {
                        ListItemName += mystr + ' ';
                    }
                }
            }
            if (ListItemName != " ")
            {
                ListBoxItem lbItem = new ListBoxItem();
                lbItem.Content = node.NodeName + ListItemName;
                lb.Items.Add(lbItem);
                lbItem.DataContext = node.NodeContent;
            }
        }

        private void GetResultList(TreeViewNode baseNode)
        {
            if (baseNode.IsDir == true)
            {
                foreach (TreeViewNode node in baseNode.ChildNodes)
                {
                    GetResultList(node);
                }
            }
            else
            {
                AddToListBox(baseNode);
            }
        }


        //判断文件当中是否包含某个字符串
        private bool IsInFile(TreeViewNode node, string mystr)
        {
            string text = node.NodeContent;
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
                string[] keysArray = KeyWords.Split(' ');
                string lines = GetStrOnLines(lbItem.DataContext.ToString(), keysArray);
                textEditor.Text += lines + "\n";
            }
        }

        //从当前行当中判断字符串是否存在
        private string GetStrOnLines(string nodeContent, string[] keysArray)
        {
            int counter = 0;
            string lines = string.Empty;

            string[] sArray = nodeContent.Split(new char[] { '\n' });
            string[] sArrayNoEmpty = sArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            foreach (string line in sArrayNoEmpty)
            {
                foreach (string mystr in keysArray)
                {
                    if (line.Contains(mystr))
                    {
                        lines += line + "\n";
                        counter++;
                        break;
                    }
                }
            }
            return lines;
        }


        //鼠标双击
        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbItem = lb.SelectedItem as ListBoxItem;
            textEditor.Text = lbItem.DataContext.ToString();
        }

    }
}
