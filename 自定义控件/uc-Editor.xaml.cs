using System;
using System.Collections.Generic;
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

namespace 脸滚键盘
{
    /// <summary>
    /// uc_Editor.xaml 的交互逻辑
    /// </summary>
    public partial class uc_Editor : UserControl
    {
        public uc_Editor()
        {
            InitializeComponent();
        }

        void LoadFromTextFile()
        {
            if (true == FileOperate.IsFileExists(Gval.Current.curItemPath))
            {
                tb.Text = FileOperate.ReadFromTxt(Gval.Current.curItemPath);
                chapterNameBox.Text = Gval.Current.curItem.Header.ToString();
                volumeNameBox.Text = Gval.Current.curVolumeItem.Header.ToString();
                bookNameBox.Text = Gval.Current.curBookItem.Header.ToString();
            }
        }

        /// <summary>
        /// DataContext绑定了当前指向的curItem，因此将其更改事件作为curItem的更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (null == Gval.Current.curBookItem)
                return;
            //获取当前文件名
            if (true == FileOperate.IsFileExists(Gval.Current.curItemPath))
            {
                LoadFromTextFile();
                uc.IsEnabled = true;
            }
            else
            {
                uc.IsEnabled = false;
            }
        }

        ////行号框的显示方法
        //private void ShowLineNumber()
        //{
        //    var x = string.Empty;
        //    int n = 0;
        //    double w = 2;
        //    for (var i = 0; i < tb.LineCount; i++)
        //    {
        //        //存在非空字符且不存在换行符（统计段落）
        //        if (IsHasWords(tb.GetLineText(i)))
        //        {
        //            if (i > 0)
        //            {
        //                if (tb.GetLineText(i - 1).Contains("\n"))
        //                {
        //                    n++;
        //                    x += n + "\n";
        //                }
        //                else
        //                {
        //                    x += "\n";
        //                }
        //            }
        //            else
        //            {
        //                n++;
        //                x += n + "\n";
        //            }
        //        }
        //        else
        //        {
        //            if (i == tb.GetLastVisibleLineIndex())
        //            {
        //                //最后一行
        //                n++;
        //                x += n;
        //            }
        //            else
        //            {
        //                x += "\n";
        //            }
        //        }
        //    }
        //    if (WordCount(n.ToString()) > 2)
        //    {
        //        w = WordCount(n.ToString());
        //    }
        //    tbl.Width = w * tb.FontSize;
        //    tbl.Text = x;
        //}

        void ShowTextInfo()
        {
            if (lb1 != null && lb2 != null)
            {
                lb1.Content = "段落：" + EditorOperate.GetMaxLineNum(tb).ToString();
                lb2.Content = "字数：" + EditorOperate.WordCount(tb.Text).ToString();
            }
        }

        private void tb_KeyUp(object sender, KeyEventArgs e)
        {
            //松开回车之后
            if (e.Key == Key.Return)
            {
                int a = tb.SelectionStart;
                int b = tb.Text.Length;
                string stra = tb.Text.Substring(0, a);
                string strb = tb.Text.Substring(a, b - a);
                tb.Text = stra + "\n　　" + strb.ToString();
                tb.Select(a + 3, 0);//光标
                SaveText();
            }
            //进行了删除之后
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                SaveText();
            }
            //逗号和句号的情况
            if (e.Key == Key.OemComma || e.Key == Key.OemPeriod)
            {
                SaveText();
            }
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            //因为开启了接受回车换行，这里不能再接收Enter，所以要转移到KeyUp
            if (e.Key == Key.F9)
            {
                Console.WriteLine("排版");
                EditorOperate.ReformatText(tb);
                SaveText();
            }
        }
        /// <summary>
        /// 执行方法：编辑区文字保存
        /// </summary>
        void SaveText()
        {
            FileOperate.WriteToTxt(Gval.Current.curItemPath, tb.Text);
            btnSaveText.Content = "";
            btnSaveText.IsEnabled = false;
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowTextInfo();
            if (uc.IsEnabled == true && tb.IsFocused == true)
            {
                btnSaveText.Content = "保存■";
                btnSaveText.IsEnabled = true;
            }
        }

        private void tb_Loaded(object sender, RoutedEventArgs e)
        {
            ShowTextInfo();

        }

        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            btnSaveText.Height = chapterNameBox.ActualHeight;
        }




        //标题栏获得焦点，进入重命名状态
        private void chapterNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //改名前的准备
            chapterNameBox.Tag = chapterNameBox.Text;
        }

        //标题栏失去焦点，结束重命名
        private void chapterNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TreeOperate.ReName.Do(Gval.Current.curTv, Gval.Current.curItem, chapterNameBox, Gval.Current.curUcTag);
        }


        private void chapterNameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //转移焦点之后触发chapterNameBox_LostFocus方法完成重命名
                tb.Focus();

                //光标移动至文末
            }
        }

        ////执行重命名的方法
        //private void DoneRename()
        //{
        //    if (CurrentItem != null)
        //    {
        //        TreeOperate.renameFinish(UcTree, CurrentItem, titleBox, WorkPath, WorkXmlName, BookXmlName);

        //        //在renameFinish当中隐藏了，重新显示titleBox
        //        titleBox.Visibility = Visibility.Visible;

        //        //更新当前选择的item指针
        //        CurrentItem = UcTree.SelectedItem as TreeViewItem;
        //        Console.WriteLine("原标题为：" + FullCurrentTxtName);
        //        //如果当前节点是文档类型
        //        if (TreeOperate.IsTreeViewItemAsDoc(CurrentItem))
        //        {
        //            FullCurrentTxtName = TreeOperate.GetCurrentRootPath(CurrentItem, WorkPath) + "/" + CurrentItem.Header + ".txt";
        //        }
        //        else
        //        {
        //            FullCurrentTxtName = null;
        //        }
        //        Console.WriteLine("变更为：" + FullCurrentTxtName);
        //    }
        //}

        ////标题栏失去焦点，执行重命名
        //private void titleBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    DoneRename();
        //}



    }
}
