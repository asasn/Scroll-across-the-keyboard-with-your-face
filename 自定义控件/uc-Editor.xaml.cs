using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml;

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

        /// <summary>
        /// 方法：展示状态栏段落和字数信息
        /// </summary>
        void ShowTextInfo()
        {
            if (lb1 != null && lb2 != null)
            {
                lb1.Content = "段落：" + textEditor.Document.Lines.Count.ToString();
                lb2.Content = "字数：" + EditorOperate.WordCount(textEditor.Text).ToString();
            }
        }


        /// <summary>
        /// 方法：编辑区文字保存
        /// </summary>
        void SaveText()
        {
            //FileOperate.WriteToTxt(Gval.Current.curItemPath, textEditor.Text);
            textEditor.Save(Gval.Current.curItemPath);
            btnSaveDoc.Content = "";
            btnSaveDoc.IsEnabled = false;
        }


        /// <summary>
        /// 方法：从文本文件中载入
        /// </summary>
        void LoadFromTextFile()
        {
            if (true == FileOperate.IsFileExists(Gval.Current.curItemPath))
            {
                //textEditor.Text = FileOperate.ReadFromTxt(Gval.Current.curItemPath);
                textEditor.Load(Gval.Current.curItemPath);
                chapterNameBox.Text = Gval.Current.curItem.Header.ToString();
                volumeNameBox.Text = Gval.Current.curVolumeItem.Header.ToString();
                bookNameBox.Text = Gval.Current.curBookItem.Header.ToString();
            }
        }


        /// <summary>
        /// 事件：DataContext更改（DataContext绑定了当前指向的curItem，因此将其更改事件作为curItem的更改事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (null == Gval.Current.curBookItem)
            {
                return;
            }
            else
            {
                SqliteOperate.Refresh();
            }
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

        /// <summary>
        /// 事件：编辑区按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_KeyDown(object sender, KeyEventArgs e)
        {
            //因为存在着“按住持续生效”的设定，所以改成在KeyDown处生效，免得一次按键生效两次
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                //同时按下了Ctrl + Z键，回撤
                textEditor.Undo();
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Y)
            {
                //同时按下了Ctrl + Y键，重做
                textEditor.Redo();
            }
        }

        /// <summary>
        /// 事件：编辑区按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int a = textEditor.SelectionStart;
                int b = textEditor.Text.Length;
                string strA = textEditor.Text.Substring(0, a);
                string strB = textEditor.Text.Substring(a, b - a);
                string strM = "\n　　";

                //重新赋值给编辑区
                textEditor.Text = strA + strM + strB.ToString();

                //光标移动至正确的编辑位置
                textEditor.SelectionStart = a + strM.Length;
                textEditor.LineDown();//滚动，行+1
                textEditor.LineDown();//滚动，行+1

                //自动保存（回车时）
                SaveText();
            }
            if (e.Key == Key.F9)
            {
                //进行排版
                EditorOperate.ReformatText(textEditor);
                SaveText();
            }
            //进行了删除之后
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                SaveText();
            }
            //逗号的情况
            if (e.Key == Key.OemComma)
            {
                SaveText();
            }
            //句号的情况
            if (e.Key == Key.OemPeriod)
            {
                SaveText();
            }

        }


        /// <summary>
        /// 事件：编辑框文字变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            ShowTextInfo();
            if (uc.IsEnabled == true && textEditor.TextArea.IsFocused == true)
            {
                btnSaveDoc.Content = "保存■";
                btnSaveDoc.IsEnabled = true;
            }
        }


        /// <summary>
        /// 事件：控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.Editor.Uc = this;
            btnSaveDoc.Height = chapterNameBox.ActualHeight;

            //快速搜索功能
            SearchPanel.Install(textEditor.TextArea);

            SetRules();
        }

        /// <summary>
        /// 根据文件设置语法规则
        /// </summary>
        public void SetRules()
        {
            textEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Base.AppPath, "Text.xshd");
            Stream xshd_stream = File.OpenRead(fullFileName);
            XmlTextReader xshd_reader = new XmlTextReader(xshd_stream);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
            xshd_reader.Close();
            xshd_stream.Close();

            TreeView[] tvs = { Gval.InfoCard.RoleTv, Gval.InfoCard.FactionTv, Gval.InfoCard.GoodsTv, Gval.InfoCard.CommonTv };

            foreach (TreeView tv in tvs)
            {
                if (tv != null)
                {
                    foreach (TreeViewItem item in tv.Items)
                    {
                        AddKeyword(item.Header.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 添加一个变色关键词
        /// </summary>
        /// <param name="keyword"></param>
        void AddKeyword(string keyword)
        {
            var spans = textEditor.SyntaxHighlighting.MainRuleSet.Spans;
            HighlightingSpan span = new HighlightingSpan();
            span.SpanColor = textEditor.SyntaxHighlighting.GetNamedColor("Keywords");
            span.StartExpression = new Regex(keyword);
            span.EndExpression = new Regex("");
            span.SpanColorIncludesStart = true;
            span.SpanColorIncludesEnd = true;
            spans.Add(span);
        }

        /// <summary>
        /// 事件：标题栏获得焦点，进入重命名状态（改名前的准备）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chapterNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            chapterNameBox.Tag = chapterNameBox.Text;
        }


        /// <summary>
        /// 事件：标题栏失去焦点，结束重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chapterNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TreeOperate.ReName.Do(Gval.Current.curTv, Gval.Current.curItem, chapterNameBox, Gval.Current.curUcTag);
        }


        /// <summary>
        /// 事件：章节名称栏按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chapterNameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //转移焦点，触发标题栏失去焦点事件
                textEditor.Focus();

                //光标移动至文末
                textEditor.SelectionStart = textEditor.Text.Length;
                textEditor.ScrollToLine(textEditor.LineCount);
                textEditor.ScrollToEnd();
            }
        }


        /// <summary>
        /// 事件：点击保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            SaveText();
        }


    }
}
