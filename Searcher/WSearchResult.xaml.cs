using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using NSMain.Bricks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace NSMain.Searcher
{
    /// <summary>
    /// SearchRetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WSearchResult : Window
    {
        public WSearchResult(ListBoxItem lbItem, string[] keyWords)
        {
            InitializeComponent();
            Matches = keyWords;

            SetKeyWordsColor();
            TextEditor.Text = lbItem.DataContext.ToString();
        }
        public WSearchResult()
        {
        }

        /// <summary>
        /// 匹配结果数组（字符串数组）
        /// </summary>
        private string[] Matches;



        private void SetKeyWordsColor()
        {
            TextEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(GlobalVal.Path.App, "Resourses/Text.xshd");
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
                CommonMethod.AddKeyWordForEditor(TextEditor, keyword, "搜索");
            }
        }



    }
}
