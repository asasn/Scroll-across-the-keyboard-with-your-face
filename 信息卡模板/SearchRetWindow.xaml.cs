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

namespace 脸滚键盘.信息卡模板
{
    /// <summary>
    /// SearchRetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SearchRetWindow : Window
    {
        public SearchRetWindow(TreeViewItem curItem, string ucTag, string keyWords)
        {
            InitializeComponent();

            KeyWords = keyWords;
            CurItem = curItem;
            UcTag = ucTag;            
            ucSearchRetWindow.DataContext = curItem;
            GetResultList();
        }

        string WorkPath;
        string KeyWords;
        string UcTag;
        string FullFileName;
        TreeViewItem CurItem;
        TreeViewItem curVolumeItem;
        TreeViewItem curBookItem;
        TreeView curTv;

        void RefreshBookItem()
        {
            //如果单只依靠绑定属性来传值，可能会发生DataContext改变了（触发本事件）而依赖属性CurItem未改变的情况
            //所以，使用this.DataContext作为CurItem的值是必需的
            CurItem = this.DataContext as TreeViewItem;

            curBookItem = TreeOperate.GetRootItem(CurItem);
            WorkPath = TreeOperate.GetItemPath(curBookItem, UcTag);
            if (curBookItem != null)
            {
                curTv = curBookItem.Parent as TreeView;
            }
            else
            {
                curTv = null;
            }
            curVolumeItem = TreeOperate.GetItemByLevel(CurItem, 2);
            FullFileName = TreeOperate.GetItemPath(CurItem, UcTag);

        }

        private void ucSearchRetWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshBookItem();
            if (null == curBookItem)
            {
                return;
            }
        }


        private void GetResultList()
        {
            DirectoryInfo theFolder;
            if (curBookItem !=null)
            {
                theFolder = new DirectoryInfo(WorkPath);
            }
            else
            {
                theFolder = new DirectoryInfo(Gval.Base.AppPath + "books");
            }

            foreach (DirectoryInfo curFolder in theFolder.GetDirectories())
            {
                //遍历文件
                foreach (FileInfo NextFile in curFolder.GetFiles())
                    if (NextFile.Extension == ".txt")
                    {
                        string[] sArray = KeyWords.Split(' ');
                        string ListItemName = String.Empty;
                        ListItemName += ' ';
                        foreach (var mystr in sArray)
                        {
                            if (false == string.IsNullOrEmpty(mystr))
                            {
                                if (IsInFile(NextFile.FullName, mystr))
                                {
                                    ListItemName += mystr + ' ';
                                }
                            }
                        }
                        if (ListItemName != " ")
                        {
                            ListBoxItem lbItem = new ListBoxItem();
                            lbItem.Content = NextFile.Name + ListItemName;
                            lb.Items.Add(lbItem);
                            string strFileName = GetFileName(lbItem.Content.ToString());
                            //获取对应的完整文件名，并储存在Uid属性当中
                            lbItem.Uid = WorkPath + '/' + curFolder.Name + '/' + strFileName;
                        }
                    }
            }

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
