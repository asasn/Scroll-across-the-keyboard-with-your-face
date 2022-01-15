using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
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
using System.Xml;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// SearchRetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WindowSearchRet : Window
    {
        public WindowSearchRet(ListBoxItem lbItem, string[] keyWords)
        {
            InitializeComponent();
            Matches = keyWords;

            SetKeyWordsColor();
            TextEditor.Text = lbItem.DataContext.ToString();
        }
        public WindowSearchRet()
        {
        }

        /// <summary>
        /// 匹配结果数组（字符串数组）
        /// </summary>
        private string[] Matches;



        private void SetKeyWordsColor()
        {
            TextEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Path.App, "Resourses/Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();

            //读取文件内的自带规则
            //IList<HighlightingRule> rules = textEditor.SyntaxHighlighting.MainRuleSet.Rules;
            //IList<HighlightingSpan> spans = textEditor.SyntaxHighlighting.MainRuleSet.Spans;
            //清空文件内的自带规则
            TextEditor.SyntaxHighlighting.MainRuleSet.Rules.Clear();
            TextEditor.SyntaxHighlighting.MainRuleSet.Spans.Clear();

            foreach (string keyword in Matches)
            {
                Common.AddKeyWordForEditor(TextEditor, keyword, "搜索");
            }
        }



    }
}
