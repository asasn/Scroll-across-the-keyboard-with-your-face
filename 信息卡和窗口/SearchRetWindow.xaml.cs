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
    public partial class SearchRetWindow : Window
    {
        public SearchRetWindow(TreeViewNode baseNode, ListBoxItem lbItem, string[] keyWords)
        {
            InitializeComponent();
            Matches = keyWords;

            SetKeyWordsColor();
            textEditor.Text = lbItem.DataContext.ToString();
        }
        public SearchRetWindow()
        {
        }

        /// <summary>
        /// 匹配结果数组（字符串数组）
        /// </summary>
        private string[] Matches;



        private void SetKeyWordsColor()
        {
            textEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Path.App, "Resourse/Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);            
            xshdReader.Close();
            xshdStream.Close();

            //清空文件内的自带规则
            textEditor.SyntaxHighlighting.MainRuleSet.Rules.Clear();
            textEditor.SyntaxHighlighting.MainRuleSet.Spans.Clear();

            IList<HighlightingRule> rules = textEditor.SyntaxHighlighting.MainRuleSet.Rules;
            foreach (string word in Matches)
            {
                AddKeyword(rules, word, "搜索");
            }
        }

        /// <summary>
        /// 添加一个变色关键词
        /// </summary>
        /// <param name="keyword"></param>
        void AddKeyword(IList<HighlightingRule> rules, string keyword, string colorName)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return;
            }
            HighlightingRule rule = new HighlightingRule();
            rule.Color = textEditor.SyntaxHighlighting.GetNamedColor(colorName);
            rule.Regex = new Regex(keyword);
            rules.Add(rule);
        } 

    }
}
