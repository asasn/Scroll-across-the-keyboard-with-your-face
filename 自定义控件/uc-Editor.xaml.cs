using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                textEditor.Text = FileOperate.ReadFromTxt(Gval.Current.curItemPath);
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


        void ShowTextInfo()
        {
            if (lb1 != null && lb2 != null)
            {
                lb1.Content = "段落：" + textEditor.Document.Lines.Count.ToString();
                lb2.Content = "字数：" + EditorOperate.WordCount(textEditor.Text).ToString();
            }
        }


        private void textEditor_KeyUp(object sender, KeyEventArgs e)
        {
            //松开回车之后
            if (e.Key == Key.Return)
            {
                int a = textEditor.SelectionStart;
                int b = textEditor.Text.Length;
                string stra = textEditor.Text.Substring(0, a);
                string strb = textEditor.Text.Substring(a, b - a);
                textEditor.Text = stra + "\n　　" + strb.ToString();
                textEditor.Select(a + 3, 0);//光标
                SaveText();
            }
            ////进行了删除之后
            //if (e.Key == Key.Delete || e.Key == Key.Back)
            //{
            //    SaveText();
            //}
            ////逗号和句号的情况
            //if (e.Key == Key.OemComma || e.Key == Key.OemPeriod)
            //{
            //    SaveText();
            //}
        }

        private void textEditor_KeyDown(object sender, KeyEventArgs e)
        {
            //因为开启了接受回车换行，这里不能再接收Enter，所以要转移到KeyUp
            if (e.Key == Key.F9)
            {
                Console.WriteLine("排版");
                EditorOperate.ReformatText(textEditor);
                SaveText();
            }
        }

        /// <summary>
        /// 执行方法：编辑区文字保存
        /// </summary>
        void SaveText()
        {
            FileOperate.WriteToTxt(Gval.Current.curItemPath, textEditor.Text);
            btnSaveText.Content = "";
            btnSaveText.IsEnabled = false;
        }



        //编辑框文字变动事件
        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            ShowTextInfo();
            if (uc.IsEnabled == true)
            {
                btnSaveText.Content = "保存■";
                btnSaveText.IsEnabled = true;
            }
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
                textEditor.Focus();

                //光标移动至文末
            }
        }

        private void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            SaveText();
        }


    }
}
